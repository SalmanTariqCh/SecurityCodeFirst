using SecurityDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SecurityDB.ModelView
{
    public class ShiftModelView
    {
        public Shift Shifts { get; set; }
        public IEnumerable<Site> Sites {get;set;}
        public User Users { get; set; }
    }
}