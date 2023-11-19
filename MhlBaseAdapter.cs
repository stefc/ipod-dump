namespace stefc.itunes;

public class MhlBaseProxy(ChunkRaw chunk) : ChunkAdapter(chunk)
{
    public uint NumChildren => this.chunk.N1;
}