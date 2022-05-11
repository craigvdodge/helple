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

        public async Task<bool> LoadFromLocalStorage()
        {
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

            // More settings here

            return true;
        }
        
        public async Task<bool> SaveToLocalStorage()
        {
            string sortOrderAsString = SortOrder.ToString();
            await _ilss.SetItemAsync<string>("sort", sortOrderAsString);

            // More settings here

            return true;
        }

        private ILocalStorageService _ilss;
    }

    public enum SortOrder
    {
        Alphabetical = 0,
        Score
    }
}