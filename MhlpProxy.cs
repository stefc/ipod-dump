namespace stefc.itunes;

public class MhlpProxy : ChunkProxy
{
    public MhlpProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint NumChildren => this.chunk.N1;
}