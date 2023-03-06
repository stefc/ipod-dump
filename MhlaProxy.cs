namespace stefc.itunes;

public class MhlaProxy : ChunkProxy
{
    public MhlaProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint NumChildren => this.chunk.N1;
}