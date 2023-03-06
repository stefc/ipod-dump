namespace stefc.itunes;

public enum TextType {
    SongTitle = 1, 
    Filename = 2, 
    Album = 3,
    Artist = 4,
    Genre = 5,
    FileType = 6, 
    Comment = 8
}

public class MhodProxy : ChunkProxy
{

    public MhodProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint TotalSize => this.chunk.N1;
    public TextType TextType => (TextType)this.chunk.N2;
    public bool IsEmpty => this.chunk.N5 != 1;
    public uint Foo => this.chunk.N5;

    public uint Length => this.chunk.N6; 

    public string Text {
        get {
            return IsEmpty ? string.Empty : this.chunk.DecodeString((int)Length);
        }
    }
    
}