namespace stefc.itunes;

public class MhypProxy : ChunkProxy
{
    public MhypProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint NumChildren => this.chunk.N3;

    public DateTime Creation
    {
        get
        {
            var x = new DateTime(1904, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            return x.AddSeconds(this.chunk.N5);
        }
    }
}