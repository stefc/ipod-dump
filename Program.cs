// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using stefc.itunes;

Console.WriteLine("Hello, World!");

var fs = File.OpenRead("../iTunes/iTunesDB");

// var chunk = new byte[12]; 

var chunkNo = 0;

var chunks = ImmutableDictionary<long, ChunkProxy>.Empty;


var histogram = ImmutableDictionary<ChunkType, int>.Empty;
var root = new ChunkRaw(fs);
ChunkRaw parent = root;
ChunkRaw current = root;
if (current.Id == ChunkType.mhbd)
{
    chunks = chunks.Add(current.Position, new MhbdProxy(current));
}
while (current != null && current.Id != ChunkType.a28a)
{

    while (current.Position > parent.Sibling)
    {
        parent = parent.Parent;
    }

    if (histogram.ContainsKey(current.Id))
    {
        histogram = histogram.SetItem(current.Id, histogram[current.Id] + 1);
    }
    else
    {
        histogram = histogram.Add(current.Id, 1);
    }

    // System.Console.WriteLine($"#{chunkNo} at {current.Position:X4}");
    // System.Console.WriteLine($"{current.Id}: {current.ChunkSize:X4} - {current.N1:X4}");

    current = new ChunkRaw(fs, parent);

    if (current.Id == ChunkType.mhbd)
    {
        chunks = chunks.Add(current.Position, new MhbdProxy(current));
    }
    else if (current.Id == ChunkType.mhsd)
    {
        chunks = chunks.Add(current.Position, new MhsdProxy(current));
    }
    else if (current.Id == ChunkType.mhlt)
    {
        chunks = chunks.Add(current.Position, new MhltProxy(current));
    }
    else if (current.Id == ChunkType.mhlp)
    {
        chunks = chunks.Add(current.Position, new MhlpProxy(current));
    }
    else if (current.Id == ChunkType.mhla)
    {
        chunks = chunks.Add(current.Position, new MhlaProxy(current));
    }
    else if (current.Id == ChunkType.mhit)
    {
        chunks = chunks.Add(current.Position, new MhitProxy(current));
    } 
    else if (current.Id == ChunkType.mhod)
    {
        chunks = chunks.Add(current.Position, new MhodProxy(current));
    } else {
        chunks = chunks.Add(current.Position, new ChunkProxy(current));
    }
}


/*
var read = fs.Read(chunk, 0, chunk.Length);
while (read == chunk.Length ) {

    var id = Parser.ChunkId(chunk);
    if (id == ChunkType.a28a) break;

    if (histogram.ContainsKey(id)) {
        histogram = histogram.SetItem(id, histogram[id] + 1);
    }
    else 
    {
        histogram = histogram.Add(id, 1);
    }

    var chunkSize = BitConverter.ToUInt32(chunk, 4);
    var n1 = BitConverter.ToUInt32(chunk, 8);
    System.Console.WriteLine($"#{chunkNo} at {fs.Position-chunk.Length:X4}");
    System.Console.WriteLine($"{id}: {chunkSize:X4} - {n1:X4}");
    

    System.Console.WriteLine();
    
    // this only works on 
    fs.Seek((id == ChunkType.mhod ? n1 : chunkSize) - chunk.Length, SeekOrigin.Current);

    // next chunk
    read = fs.Read(chunk, 0, chunk.Length);
    chunkNo++;
} */

foreach (var x in histogram)
{
    System.Console.WriteLine($"{x.Key} = {x.Value}");
}

foreach (var chunk in chunks.Values.OrderBy( c => c.Position))
{
    switch (chunk)
    {
        case MhbdProxy mhbd:
            System.Console.WriteLine("(mbhd)");
            System.Console.WriteLine($"v{mhbd.VersionMajor}.{mhbd.VersionMinor}.{mhbd.VersionPatch}");
            break;
        case MhsdProxy mhsd:
            System.Console.WriteLine($"{new String(' ', chunk.Level)}(mbsd)");
            System.Console.WriteLine($"{new String(' ', chunk.Level)}{mhsd.ChildSize:X4} {mhsd.ChildType}");
            break;
        case MhltProxy mhlt:
            System.Console.WriteLine($"{new String(' ', chunk.Level)}(mblt)");
            System.Console.WriteLine($"{new String(' ', chunk.Level)}{mhlt.NumChildren}");
            break;
        case MhlpProxy mhlp:
            System.Console.WriteLine($"{new String(' ', chunk.Level)}(mblp)");
            System.Console.WriteLine($"{new String(' ', chunk.Level)}{mhlp.NumChildren}");
            break;
        case MhlaProxy mhla:
            System.Console.WriteLine($"{new String(' ', chunk.Level)}(mbla)");
            System.Console.WriteLine($"{new String(' ', chunk.Level)}{mhla.NumChildren}");
            break;
        case MhitProxy mhit: 
           System.Console.WriteLine($"{new String(' ', chunk.Level)}(mhit)");
           System.Console.WriteLine($"{new String(' ', chunk.Level)} {mhit.FileSize/1024.0/1024.0:N1} Mb {mhit.Duration.ToString(@"hh\:mm")} {mhit.Creation.ToShortDateString()}");
           break;
        case MhodProxy mhod: 
           System.Console.WriteLine($"{new String(' ', chunk.Level)}(mhod)");
           System.Console.WriteLine($"{new String(' ', chunk.Level)} {mhod.TextType} {mhod.TotalSize:X4} {mhod.Foo}-{mhod.Length} {mhod.Text}");
           break;
        default:
           System.Console.WriteLine($"{new String(' ', chunk.Level)}({chunk.Id})");
           break;
    }
}





