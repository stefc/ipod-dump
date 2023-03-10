// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using stefc.itunes;

Console.WriteLine("Hello, World!");

var fs = File.OpenRead("../green/iTunesDB");

var chunks = ImmutableDictionary<long, ChunkProxy>.Empty;

var formatSize = (uint size) =>
{
    if (size < 1024)
        return $"{size} Bytes";
    if (size < (1024 * 1024))
        return $"{size / 1024.0:N1} Kb";
    return $"{size / 1024.0 / 1024.0:N1} Mb";
};


var histogram = ImmutableDictionary<ChunkType, int>.Empty;

bool eof = false;
do
{
    var current = new ChunkRaw(fs);
    histogram = histogram.ContainsKey(current.Id) ? histogram.SetItem(current.Id, histogram[current.Id] + 1) : histogram.Add(current.Id, 1);
    chunks = chunks.Add(current.Position, ChunkFactory.Create(current));
    eof = current.Id == ChunkType.a28a;
} while (!eof);

var songs = chunks.Values.OfType<MhitProxy>()
    .SelectMany(hit => chunks.Values.OfType<MhodProxy>().Where(hod => hod.Position > hit.Position && hod.Position < hit.Position + hit.TotalSize - 1)
        .Where(hod => hod.TextType == TextType.SongTitle),
    (hit, hod) => KeyValuePair.Create(hit.SongID, hod.Text)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


foreach (var x in histogram)
{
    System.Console.WriteLine($"{x.Key} = {x.Value}");
}

foreach (var chunk in chunks.Values.OrderBy(c => c.Position))
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
            System.Console.WriteLine($"{mhlt.NumChildren} Tracks");
            break;
        case MhlpProxy mhlp:
            System.Console.WriteLine($"{mhlp.NumChildren} Playlists");
            break;
        case MhypProxy mhyp:
            if (mhyp.NumChildren > 0)
            {
                System.Console.WriteLine($"Folder");
                System.Console.WriteLine($"{mhyp.NumChildren} Items");
            }
            break;
        case MhlaProxy mhla:
            System.Console.WriteLine($"{mhla.NumChildren} Artists");
            break;
        case MhitProxy mhit:
            // System.Console.WriteLine($"{mhit.Position}..{mhit.Position+mhit.TotalSize-1}");
            System.Console.WriteLine($"[#{mhit.SongID:x2}] {formatSize(mhit.FileSize)} {mhit.Duration.ToString(@"hh\:mm")} {mhit.Creation.ToShortDateString()}");
            System.Console.WriteLine($"{songs[mhit.SongID]}'");
            break;
        case MhipProxy mhip:
            System.Console.WriteLine($"{songs[mhip.SongID]}'");
            break;
        case MhiaProxy mhia:
            System.Console.WriteLine($"Artist-Info");
            System.Console.WriteLine($"[#{mhia.ArtistID:x2}] {mhia.TextCount}");
            break;
        case MhodProxy mhod:
            if (!string.IsNullOrEmpty(mhod.Text))
            {
                System.Console.WriteLine($"{mhod.TextType}:  '{mhod.Text}'");
            }
            break;
        default:
            System.Console.WriteLine($"({chunk.Id})");
            break;
    }
}

