namespace HelpleDataFile
{
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
            // This is currently written for backwards compatiblity and will be changed in the future
            entries.OrderBy(s => s.Word);

            if (File.Exists(Filename))
            {
                File.Delete(Filename);
            }

            using (StreamWriter sw = File.CreateText(Filename))
            {
                foreach(Entry e in entries)
                {
                    sw.Write(e.Word + ',');
                }
            }
        }

        public async Task<bool> Read(string Filename)
        {
            // Subject to change without notice
            if (HttpClient == null)
            {
                _extendedInfo = "Http client info not set";
                return false;
            }

            try
            {
                var readTask = HttpClient.GetStringAsync(Filename);
                await readTask;
                string[] wordsTemp = readTask.Result.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in wordsTemp)
                {
                    entries.Add(new Entry(word));
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