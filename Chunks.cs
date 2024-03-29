using System.Text;

namespace stefc.itunes;

public enum ChunkType
{
    mhbd,   // header
    mhsd,   // section

    mhlt,   // tracks
    mhlp,  // playlist
    mhla,    // artist  

    mhit,   // title item  

    mhip,   // playlist item
    mhia,   // artist item

    mhyp,   // playlist info 

    mhod,   // text info
    a28a    // end marker
}

/*
mhbd = 1 // header
mhsd = 6 // sections 

mhlt = 1  // tracks 
mhit = 107  // track 

mhla = 1   // artists   
mhia = 2   // artist

mhlp = 3  // playlists 
mhyp = 4   // 
mhip = 214 // playlist item

mhod = 1090 // text

*/

static public class Parser
{

    public static ChunkType ChunkId(byte[] chunk) => Encoding.ASCII.GetString(chunk, 0, 4).ToEnum<ChunkType>();

    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}

public class ChunkRaw
{

    private const int PROLOG_LENGTH = 12;
    
    private readonly long pos;
    private readonly byte[] data;

    private readonly Lazy<ChunkType> id;

    public ChunkRaw(Stream stream)
    {
        this.pos = stream.Position;
        this.data = new byte[PROLOG_LENGTH];
        this.id = new Lazy<ChunkType>(() => Parser.ChunkId(this.data), true);
        var read = stream.Read(this.data, 0, PROLOG_LENGTH);
        var len = (int)((Id != ChunkType.a28a) ?
                (Id == ChunkType.mhod ? this.N1 : this.ChunkSize) - PROLOG_LENGTH :
                (stream.Length - (this.pos + PROLOG_LENGTH)));
        Array.Resize(ref this.data, len + PROLOG_LENGTH);
        read = stream.Read(data, PROLOG_LENGTH, len);
    }


    public ChunkType Id => this.id.Value;
    public long Position => this.pos;

    public long Length => (this.Id == ChunkType.mhod ? this.N1 : this.ChunkSize);

    public long Sibling => this.pos + this.N1;
    
    public Span<byte> Data => this.data;

    public uint ChunkSize => BitConverter.ToUInt32(this.data, 4);
    public uint N1 => BitConverter.ToUInt32(this.data, 8);
    public uint N2 => BitConverter.ToUInt32(this.data, 12);
    public uint N3 => BitConverter.ToUInt32(this.data, 16);
    public uint N4 => BitConverter.ToUInt32(this.data, 20);
    public uint N5 => BitConverter.ToUInt32(this.data, 24);
    public uint N6 => BitConverter.ToUInt32(this.data, 28);
    public uint N7 => BitConverter.ToUInt32(this.data, 32);
    public uint N8 => BitConverter.ToUInt32(this.data, 36);
    public uint N9 => BitConverter.ToUInt32(this.data, 40);
    public uint N10 => BitConverter.ToUInt32(this.data, 44);

    public string DecodeString(int len) => System.Text.Encoding.Unicode.GetString(this.data, 40, len);
}


