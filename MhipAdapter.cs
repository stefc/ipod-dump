namespace stefc.itunes;

public class MhipAdapter(ChunkRaw chunk) : ChunkAdapter(chunk)
{
    public uint SongID => this.chunk.N5;

    public DateTime Creation => new DateTime(1904, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddSeconds(this.chunk.N6);
}