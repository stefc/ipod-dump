namespace stefc.itunes;

public class ChunkProxy
{
    protected readonly ChunkRaw chunk;

    public ChunkProxy(ChunkRaw chunk) => this.chunk = chunk;

    public uint ChunkSize => this.chunk.ChunkSize;
    public long Position => this.chunk.Position;
    public ChunkType Id => this.chunk.Id;

    public int Level
    {
        get
        {
            var level = 0;
            var node = this.chunk;
            while (node.Parent != node) {
                level++;
                node = node.Parent;
            }
            return level;
        }
    }

}