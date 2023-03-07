namespace stefc.itunes;

public enum SectionType {
    Titles = 1, 
    Playlist = 2, 
    Folder = 3,
    Artist = 4,
    Books = 5,
    Etc = 9
}

public class MhsdProxy : ChunkProxy
{
    public MhsdProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint ChildSize => this.chunk.N1;
    public SectionType SectionType => (SectionType)this.chunk.N2;
}