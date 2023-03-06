namespace stefc.itunes;

public class MhsdProxy : ChunkProxy
{
    public MhsdProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint ChildSize => this.chunk.N1;
    public uint ChildType => this.chunk.N2;
}