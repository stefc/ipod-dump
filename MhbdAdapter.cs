namespace stefc.itunes;

public class MhbdAdapter(ChunkRaw chunk) : ChunkAdapter(chunk)
{
    public uint VersionMajor => this.chunk.N2;
    public uint VersionMinor => this.chunk.N3;
    public uint VersionPatch => this.chunk.N4;
    public uint FileSize => this.chunk.N1;
}