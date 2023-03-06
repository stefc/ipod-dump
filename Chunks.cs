using System.Text;

namespace stefc.itunes;

public enum ChunkType
{
    mhbd,   // header
    mhsd,   // section
    
    mhlt,   // tracks
    mhit,   // reference to title ? 

    mhlp,  // playlist
    mhip,  // playlist
    mhyp,   // playlist info 
    
    mhia, 
    mhla,    // art ?  
    mhod,   // text info
    a28a    // end marker
}

/*
mhbd = 1
mhsd = 6    

mhlt = 1  // songs left 
mhit = 107  // equal to number of titles 

mhla = 1   // image  
mhia = 2   // ?? 

mhlp = 3  // playlists 
mhyp = 4  
mhip = 214

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

public class ChunkRaw {

    private readonly ChunkRaw parent; 

    private readonly long pos;
    private readonly byte[] prolog = new byte[12];
    private readonly byte[] data; 

    private readonly ChunkType id;

    private readonly uint chunkSize;
    private readonly uint n1;


    public ChunkRaw(Stream stream, ChunkRaw? parent = null)
    {
        this.parent = parent == null ? this : parent;
        this.pos = stream.Position;
        var read = stream.Read(prolog, 0, prolog.Length);
        this.id = Parser.ChunkId(prolog);
        this.chunkSize = BitConverter.ToUInt32(prolog, 4);
        this.n1 = BitConverter.ToUInt32(prolog, 8);
        if (id != ChunkType.a28a) {

            var len =(int) (id == ChunkType.mhod ? n1 : this.chunkSize) - prolog.Length;
            data = new byte[len];
            read = stream.Read(data, 0, len);
        } else {
            var len = (int) (stream.Length - (this.pos+prolog.Length));
            data = new byte[len];
            read = stream.Read(data, 0, len);
        }
    }


    public ChunkType Id => this.id;
    public long Position => this.pos;
    public uint ChunkSize => this.chunkSize;
    public uint N1 => this.n1;

    public long Sibling => this.pos + this.n1;

    public ChunkRaw Parent => this.parent;

    public Span<byte> Data => this.data;

    public uint N2 => BitConverter.ToUInt32(this.data, 0);
    public uint N3 => BitConverter.ToUInt32(this.data, 4);
    public uint N4 => BitConverter.ToUInt32(this.data, 8);
    public uint N5 => BitConverter.ToUInt32(this.data, 12);
    public uint N6 => BitConverter.ToUInt32(this.data, 16);
    public uint N7 => BitConverter.ToUInt32(this.data, 20);
    public uint N8 => BitConverter.ToUInt32(this.data, 24);
    public uint N9 => BitConverter.ToUInt32(this.data, 28);
    public uint N10 => BitConverter.ToUInt32(this.data, 32);

    public string DecodeString(int len) {
        return System.Text.Encoding.Unicode.GetString(this.data, 28, len);
    }
}


