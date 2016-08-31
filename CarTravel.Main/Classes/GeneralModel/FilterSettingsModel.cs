using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarTravel.Main.Classes.GeneralModel
{
    public class FilterSettingsModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public FilterSettingsModel()
        {
            DateFrom = DateTime.Today.AddDays(-7);
            DateTo = DateTime.Today;
        }
    }
}
