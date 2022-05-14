using Blazored.LocalStorage;

namespace HelpleSharedWeb
{
    public class SettingsModel
    {
        public SettingsModel(ILocalStorageService ILSS)
        {
            _ilss = ILSS;
        }

        public SortOrder SortOrder { get; set; }

        public int Columns { get; set; }

        public int Rows { get; set; }

        public async Task<bool> LoadFromLocalStorage()
        {
            // Sort order
            string? sortOrderAsString = await _ilss.GetItemAsync<string>("sort");
            if (sortOrderAsString == null)
            {
                SortOrder = SortOrder.Alphabetical;
            }
            else if (sortOrderAsString == "Score")
            {
                SortOrder = SortOrder.Score;
            }
            else
            {
                SortOrder = SortOrder.Alphabetical;
            }

            // Columns
            int? columnsFromStorage = await _ilss.GetItemAsync<int>("columns");
            if (columnsFromStorage == null)
            {
                Columns = 4;
            }
            else
            {
                Columns = columnsFromStorage.Value;
                if (Columns == 0)
                {
                    Columns = 4;
                }
            }

            // Rows
            int? rowsFromStorage = await _ilss.GetItemAsync<int>("rows");
            if (rowsFromStorage == null)
            {
                Rows = 10;
            }
            else
            {
                Rows = rowsFromStorage.Value;
                if (Rows == 0)
                {
                    Rows = 10;
                }
            }

            return true;
        }
        
        public async Task<bool> SaveToLocalStorage()
        {
            // Sort order
            string sortOrderAsString = SortOrder.ToString();
            await _ilss.SetItemAsync<string>("sort", sortOrderAsString);

            // Columns
            await _ilss.SetItemAsync<int>("columns", Columns);
            // Rows
            await _ilss.SetItemAsync<int>("rows", Rows);

            return true;
        }

        public async Task<bool> Clear()
        {
            await _ilss.ClearAsync();
            await LoadFromLocalStorage();
            return true;
        }

        private ILocalStorageService _ilss;
        public ILocalStorageService LocalStorage
        {
            get { return _ilss; }
            set { _ilss = value; }
        }
    }

    public enum SortOrder
    {
        Alphabetical = 0,
        Score
    }
}