namespace stefc.itunes;

public class MhypAdapter(ChunkRaw chunk) : ChunkAdapter(chunk)
{
    public uint NumChildren => this.chunk.N3;

    public DateTime Creation => new DateTime(1904, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddSeconds(this.chunk.N5);
}