namespace stefc.itunes;

public static class ITunesWriter {

    public static void  WriteToFile(string fileName, IDictionary<long, ChunkProxy> chunks) {

        var fs = File.OpenWrite(fileName);

        foreach (var chunk in chunks.Values.OrderBy(c => c.Position))
        {
            fs.Write(chunk.Data);
        }
        fs.Close();
    }

}