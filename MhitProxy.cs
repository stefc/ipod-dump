namespace stefc.itunes;

public class MhitProxy : ChunkProxy
{

    public MhitProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint TotalSize => this.chunk.N1;
    public uint NumChildren => this.chunk.N2;
    public uint SongID => this.chunk.N3;
    public uint FileType => this.chunk.N6;
    public uint FileSize => this.chunk.N8;
    public TimeSpan Duration => TimeSpan.FromMilliseconds(chunk.N9);

    public DateTime Creation
    {
        get
        {
            var x = new DateTime(1904, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            return x.AddSeconds(this.chunk.N7);
        }
    }
}