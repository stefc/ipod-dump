namespace stefc.itunes;

public class ITunesDump
{

    private readonly IDictionary<long, ChunkAdapter> chunks;
    private readonly IDictionary<uint, string> songs;

    public ITunesDump(IDictionary<long, ChunkAdapter> chunks)
    {
        this.chunks = chunks;


        this.songs = this.chunks.Values.OfType<MhitAdapter>()
            .SelectMany(hit => chunks.Values.OfType<MhodAdapter>().Where(hod => hod.Position > hit.Position && hod.Position < hit.Position + hit.TotalSize - 1)
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

    private void Dump(ChunkAdapter chunk) => Console.WriteLine($"{chunk.Id}[{chunk.Position:X5}:{chunk.Sibling:X5}] / {chunk.ChunkSize:X3}:{chunk.Length:X3} ");

    public void Dump()
    {

        foreach (var chunk in this.chunks.Values.OrderBy(c => c.Position))
        {
            switch (chunk)
            {
                case MhbdAdapter mhbd:
                    Console.WriteLine($"Header - {FormatSize(mhbd.FileSize)}");
                    Dump(chunk);
                    Console.WriteLine($"v{mhbd.VersionMajor}.{mhbd.VersionMinor}.{mhbd.VersionPatch}");
                    break;
                case MhsdAdapter mhsd:
                    Console.WriteLine($"Section ('{mhsd.SectionType}') {this.FormatSize(mhsd.ChildSize)}");
                    Dump(chunk);
                    break;
                case MhltAdapter mhlt:
                    Console.WriteLine($"{mhlt.NumChildren} Tracks ");
                    Dump(chunk);
                    break;
                case MhlpAdapter mhlp:
                    Console.WriteLine($"{mhlp.NumChildren} Playlists ");
                    Dump(chunk);
                    break;
                case MhypAdapter mhyp:
                    if (mhyp.NumChildren > 0)
                    {
                        Console.WriteLine($"Folder");
                        Dump(chunk);
                        Console.WriteLine($"{mhyp.NumChildren} Items");
                    }
                    break;
                case MhlaAdapter mhla:
                    Console.WriteLine($"{mhla.NumChildren} Artists");
                    Dump(chunk);
                    break;
                case MhitAdapter mhit:
                    // Console.WriteLine($"{mhit.Position}..{mhit.Position+mhit.TotalSize-1}");
                    Console.WriteLine($"[#{mhit.SongID:x2}] {this.FormatSize(mhit.FileSize)} {mhit.Duration.ToString(@"hh\:mm")} {mhit.Creation.ToShortDateString()}");
                    Dump(chunk);
                    Console.WriteLine($"{songs[mhit.SongID]}'");
                    break;
                case MhipAdapter mhip:
                    Console.WriteLine($"{songs[mhip.SongID]}'");
                    Dump(chunk);
                    break;
                case MhiaAdapter mhia:
                    Console.WriteLine($"Artist-Info");
                    Dump(chunk);
                    Console.WriteLine($"[#{mhia.ArtistID:x2}] {mhia.TextCount}");
                    break;
                case MhodAdapter mhod:
                    if (!string.IsNullOrEmpty(mhod.Text))
                    {
                        Console.WriteLine($"{mhod.TextType}:  '{mhod.Text}'");
                        Dump(chunk);
                    }
                    break;
                default:
                    Console.WriteLine($"({chunk.Id})");
                    Dump(chunk);
                    break;
            }
        }

    }
}