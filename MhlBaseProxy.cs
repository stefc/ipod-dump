namespace stefc.itunes;

public class MhlBaseProxy : ChunkProxy
{
    public MhlBaseProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint NumChildren => this.chunk.N1;
}