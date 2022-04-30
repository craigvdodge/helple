namespace HelpleDataFile
{
    using System.Text.Json;
    using System.IO.Compression;
    public class DataFile
    {
        public DataFile()
        {
            entries = new List<Entry>();
            HttpClient = null;
            _extendedInfo = string.Empty;
        }

        public DataFile(HttpClient httpClient)
        {
            entries = new List<Entry>();
            HttpClient = httpClient;
            _extendedInfo = string.Empty;
        }

        public void Write(string Filename)
        {
            entries.OrderBy(s => s.Word);

            if (File.Exists(Filename))
            {
                File.Delete(Filename);
            }
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            using (FileStream fs = File.Create(Filename))
            {
                using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress))
                {
                    string data = JsonSerializer.Serialize(entries, options);
                    byte[] dataAsBytes = System.Text.Encoding.UTF8.GetBytes(data);
                    gz.Write(dataAsBytes);
                }
            }
        }

        public async Task<bool> Read(string Filename)
        {
            if (HttpClient == null)
            {
                _extendedInfo = "Http client info not set";
                return false;
            }

            try
            {
                var readTask = HttpClient.GetStreamAsync(Filename);
                await readTask;              
                using (GZipStream gzip = new GZipStream(readTask.Result, CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(gzip))
                    {
                        string data = reader.ReadToEnd();
#pragma warning disable CS8601 // Possible null reference assignment.
                        entries = JsonSerializer.Deserialize<List<Entry>>(data);
#pragma warning restore CS8601 // Possible null reference assignment.
                    }
                }

                _extendedInfo = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                _extendedInfo = e.Message;
                return false;
            }
        }

        public HttpClient? HttpClient { get; set; }

        public string ExtendedInfo { get { return _extendedInfo; } }
        private string _extendedInfo;

        public void AddWord(string word, int score=0)
        {
            entries.Add(new Entry(word.ToUpper(), score));
        }

        private List<Entry> entries;
        public List<Entry> Entries { get { return entries; } }
    }

    /// <summary>
    /// Words, and any associated data (i.e. ranking)
    /// </summary>
    public class Entry
    {
        public Entry()
        {
            Word = string.Empty;
            Score = 0;
        }

        public Entry(string word, int score=0)
        {
            Word = word;
            Score = score;
        }

        public string Word { get; set; }
        public int Score { get; set; }

    }
}