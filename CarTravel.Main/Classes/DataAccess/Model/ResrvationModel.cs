using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarTravel.Main.Classes.DataAccess.Model
{
    public class ResrvationModel : reservations
    {
        public ResrvationModel()
        {
            carsList = new List<int>();
        }

        public IList<int> carsList { get; set; }
        public string statusCode { get; set; }
        public string client { get; set; }


    }
}
