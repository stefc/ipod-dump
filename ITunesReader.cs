using System.Collections.Immutable;

namespace stefc.itunes;

public static class ITunesReader {
    
    public static IDictionary<long, ChunkAdapter> ReadFromFile(string fileName) {

        var fs = File.OpenRead(fileName);

        var chunks = ImmutableDictionary<long, ChunkAdapter>.Empty;

        bool eof;
        do
        {
            var current = new ChunkRaw(fs);
            chunks = chunks.Add(current.Position, ChunkFactory.Create(current));
            eof = current.Id == ChunkType.a28a;
        } while (!eof);

        return chunks;
    }
}