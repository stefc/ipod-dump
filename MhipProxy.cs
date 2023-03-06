namespace stefc.itunes;

public class MhipProxy : ChunkProxy
{

    public MhipProxy(ChunkRaw chunk) : base(chunk)
    {
    }

    public uint SongID => this.chunk.N5;

    public DateTime Creation
    {
        get
        {
            var x = new DateTime(1904, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            return x.AddSeconds(this.chunk.N6);
        }
    }
}