namespace stefc.itunes;

public class MhbdProxy : ChunkProxy
{
    public MhbdProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint VersionMajor => this.chunk.N2;
    public uint VersionMinor => this.chunk.N3;
    public uint VersionPatch => this.chunk.N4;
}