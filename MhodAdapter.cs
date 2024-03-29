namespace stefc.itunes;

public enum TextType {
    SongTitle = 1, 
    Filename = 2, 
    Album = 3,
    Artist = 4,
    Genre = 5,
    FileType = 6, 
    Comment = 8,

    ArtistAlbum = 200,
    ArtistName = 201
}

public class MhodAdapter(ChunkRaw chunk) : ChunkAdapter(chunk)
{
    public uint TotalSize => this.chunk.N1;
    public TextType TextType => (TextType)this.chunk.N2;
    public bool IsEmpty => this.chunk.N5 != 1;

    public uint Length => this.chunk.N6;

    public string Text => IsEmpty ? string.Empty : this.chunk.DecodeString((int)Length);

}