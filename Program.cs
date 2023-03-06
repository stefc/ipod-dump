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
    parent = current.Position < parent.Sibling ? current.Parent : parent.Parent; 
    
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
    else if (current.Id == ChunkType.mhip)
    {
        chunks = chunks.Add(current.Position, new MhipProxy(current));
    } 
    else if (current.Id == ChunkType.mhod)
    {
        chunks = chunks.Add(current.Position, new MhodProxy(current));
    } else {
        chunks = chunks.Add(current.Position, new ChunkProxy(current));
    }
}

foreach (var x in histogram)
{
    System.Console.WriteLine($"{x.Key} = {x.Value}");
}

foreach (var chunk in chunks.Values.OrderBy( c => c.Position))
{
    switch (chunk)
    {
        case MhbdProxy mhbd:
            System.Console.WriteLine("Header");
            System.Console.WriteLine($"v{mhbd.VersionMajor}.{mhbd.VersionMinor}.{mhbd.VersionPatch}");
            break;
        case MhsdProxy mhsd:
            System.Console.WriteLine($"{new String(' ', chunk.Level)}Section");
            System.Console.WriteLine($"{new String(' ', chunk.Level)}{mhsd.ChildType}");
            break;
        case MhltProxy mhlt:
            System.Console.WriteLine($"{new String(' ', chunk.Level)}Tape");
            System.Console.WriteLine($"{new String(' ', chunk.Level)}{mhlt.NumChildren} Tapes");
            break;
        case MhlpProxy mhlp:
            System.Console.WriteLine($"{new String(' ', chunk.Level)}Playlists");
            System.Console.WriteLine($"{new String(' ', chunk.Level)}{mhlp.NumChildren} Playlists");
            break;
        case MhlaProxy mhla:
            System.Console.WriteLine($"{new String(' ', chunk.Level)}(mbla)");
            System.Console.WriteLine($"{new String(' ', chunk.Level)}{mhla.NumChildren}");
            break;
        case MhitProxy mhit: 
           System.Console.WriteLine($"{new String(' ', chunk.Level)}Song");
           System.Console.WriteLine($"{new String(' ', chunk.Level)} [#{mhit.SongID:x2}] {mhit.FileSize/1024.0/1024.0:N1} Mb {mhit.Duration.ToString(@"hh\:mm")} {mhit.Creation.ToShortDateString()}");
           break;
        case MhipProxy mhip: 
           System.Console.WriteLine($"{new String(' ', chunk.Level)}Playlist-Item");
           System.Console.WriteLine($"{new String(' ', chunk.Level)} [#{mhip.SongID:x2}] {mhip.Creation.ToShortDateString()} {mhip.Creation.ToShortTimeString()}");
           break;
        case MhodProxy mhod:
            //if (mhod.TextType == TextType.SongTitle)  {
                System.Console.WriteLine($"{new String(' ', chunk.Level)} {mhod.TextType} {mhod.Text}");
            //}
           break;
        default:
           System.Console.WriteLine($"{new String(' ', chunk.Level)}({chunk.Id})");
           break;
    }
}





