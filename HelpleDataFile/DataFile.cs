namespace HelpleDataFile
{
    using System.Text.Json;
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
            using (StreamWriter sw = File.CreateText(Filename))
            {
                sw.Write(JsonSerializer.Serialize(entries, options));
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
                var readTask = HttpClient.GetStringAsync(Filename);
                await readTask;
#pragma warning disable CS8601 // Possible null reference assignment.
                entries = JsonSerializer.Deserialize<List<Entry>>(readTask.Result);
#pragma warning restore CS8601 // Possible null reference assignment.

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

        public void AddWord(string word)
        {
            entries.Add(new Entry(word.ToUpper()));
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
        }

        public Entry(string word)
        {
            Word = word;
        }

        public string Word { get; set; }

    }
}