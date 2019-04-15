using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Writers.Models;

namespace Writers.ViewModels
{
    public class PersonsDetailsViewModel
    {
        public PersonsDetailsInfo PersonsDetailsInfo { get; set; }
        public Person Person { get; set; }

    }
}