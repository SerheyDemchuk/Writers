
namespace Writers.ViewModels
{
    public class PersonsListInfo
    {
        public string SearchString { get; set; }
        public int TotalItemsFound { get; set; }
        public int ItemsOnPage { get; set; }
        public string CurrentSortOrder { get; set; }

        //public int[] ItemsOnPageVariants { get; set; }

    }
}