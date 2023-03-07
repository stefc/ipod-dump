// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using stefc.itunes;

Console.WriteLine("Hello, World!");

var fs = File.OpenRead("../iTunes/iTunesDB");

var chunks = ImmutableDictionary<long, ChunkProxy>.Empty;

var formatSize = (uint size) => {
    if (size < 1024)
        return $"{size} Bytes";
    if (size < (1024 * 1024))
        return $"{size/1024.0:N1} Kb";
    return $"{size/1024.0/1024.0:N1} Mb"; 
};



var histogram = ImmutableDictionary<ChunkType, int>.Empty;

bool eof = false;
do {
    var current = new ChunkRaw(fs);
    histogram = histogram.ContainsKey(current.Id) ? histogram.SetItem(current.Id, histogram[current.Id] + 1) :  histogram.Add(current.Id, 1);
    chunks = chunks.Add(current.Position, ChunkFactory.Create(current));
    eof = current.Id == ChunkType.a28a;
} while (!eof);

foreach (var x in histogram)
{
    System.Console.WriteLine($"{x.Key} = {x.Value}");
}

foreach (var chunk in chunks.Values.OrderBy( c => c.Position))
{
    switch (chunk)
    {
        case MhbdProxy mhbd:
            System.Console.WriteLine($"Header - {formatSize(mhbd.FileSize)} ");
            System.Console.WriteLine($"v{mhbd.VersionMajor}.{mhbd.VersionMinor}.{mhbd.VersionPatch}");
            break;
        case MhsdProxy mhsd:
            System.Console.WriteLine($"Section ('{mhsd.SectionType}') {formatSize(mhsd.ChildSize)} ");
            break;
        case MhltProxy mhlt:
            System.Console.WriteLine($"Tape");
            System.Console.WriteLine($"{mhlt.NumChildren} Tapes");
            break;
        case MhlpProxy mhlp:
            System.Console.WriteLine($"Playlists");
            System.Console.WriteLine($"{mhlp.NumChildren} Playlists");
            break;
        case MhlaProxy mhla:
            System.Console.WriteLine($"(mbla)");
            System.Console.WriteLine($"{mhla.NumChildren}");
            break;
        case MhitProxy mhit: 
           System.Console.WriteLine($"Song");
           System.Console.WriteLine($"[#{mhit.SongID:x2}] {formatSize(mhit.FileSize)} {mhit.Duration.ToString(@"hh\:mm")} {mhit.Creation.ToShortDateString()}");
           break;
        case MhipProxy mhip: 
           System.Console.WriteLine($"Playlist-Item");
           System.Console.WriteLine($"[#{mhip.SongID:x2}] {mhip.Creation.ToShortDateString()} {mhip.Creation.ToShortTimeString()}");
           break;
        case MhodProxy mhod:
            //if (mhod.TextType == TextType.SongTitle)  {
                System.Console.WriteLine($"{mhod.TextType} {mhod.Text}");
            //}
           break;
        default:
           System.Console.WriteLine($"({chunk.Id})");
           break;
    }
}

