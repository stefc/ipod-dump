namespace stefc.itunes;

public class MhiaProxy : ChunkProxy
{

    public MhiaProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint TextCount => this.chunk.N2;
    public uint ArtistID => this.chunk.N3;

    
}