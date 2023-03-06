namespace stefc.itunes;

public class MhltProxy : ChunkProxy
{
    public MhltProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint NumChildren => this.chunk.N1;
}