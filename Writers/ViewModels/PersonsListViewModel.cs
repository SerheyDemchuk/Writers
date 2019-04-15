using System.Collections.Generic;
using Writers.Models;
using X.PagedList;

namespace Writers.ViewModels
{
    public class PersonsListViewModel
    {
        public PersonsListInfo PersonsListInfo { get; set; }
        public IPagedList<Person> PagedList { get; set; }
    }
}