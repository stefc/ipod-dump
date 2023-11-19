namespace stefc.itunes;

public class MhitAdapter(ChunkRaw chunk) : ChunkAdapter(chunk)
{
    public uint TotalSize => this.chunk.N1;
    public uint NumChildren => this.chunk.N2;
    public uint SongID => this.chunk.N3;
    public uint FileType => this.chunk.N6;
    public uint FileSize => this.chunk.N8;
    public TimeSpan Duration => TimeSpan.FromMilliseconds(chunk.N9);

    public DateTime Creation => new DateTime(1904, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddSeconds(this.chunk.N7);
}