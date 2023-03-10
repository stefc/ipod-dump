namespace stefc.itunes;

public class ITunesDump
{

    private readonly IDictionary<long, ChunkProxy> chunks;
    private readonly IDictionary<uint, string> songs;

    public ITunesDump(IDictionary<long, ChunkProxy> chunks)
    {
        this.chunks = chunks;


        this.songs = this.chunks.Values.OfType<MhitProxy>()
            .SelectMany(hit => chunks.Values.OfType<MhodProxy>().Where(hod => hod.Position > hit.Position && hod.Position < hit.Position + hit.TotalSize - 1)
                .Where(hod => hod.TextType == TextType.SongTitle),
                (hit, hod) => KeyValuePair.Create(hit.SongID, hod.Text)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private string FormatSize(uint size)
    {
        if (size < 1024)
            return $"{size} Bytes";
        if (size < (1024 * 1024))
            return $"{size / 1024.0:N1} Kb";
        return $"{size / 1024.0 / 1024.0:N1} Mb";
    }

    public void Dump()
    {

        foreach (var chunk in this.chunks.Values.OrderBy(c => c.Position))
        {
            switch (chunk)
            {
                case MhbdProxy mhbd:
                    System.Console.WriteLine($"Header - {this.FormatSize(mhbd.FileSize)} ");
                    System.Console.WriteLine($"v{mhbd.VersionMajor}.{mhbd.VersionMinor}.{mhbd.VersionPatch}");
                    break;
                case MhsdProxy mhsd:
                    System.Console.WriteLine($"Section ('{mhsd.SectionType}') {this.FormatSize(mhsd.ChildSize)} ");
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
                    System.Console.WriteLine($"[#{mhit.SongID:x2}] {this.FormatSize(mhit.FileSize)} {mhit.Duration.ToString(@"hh\:mm")} {mhit.Creation.ToShortDateString()}");
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

    }
}