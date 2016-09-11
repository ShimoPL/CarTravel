using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarTravel.Main.Classes.DataAccess.Model
{
    public class ReservationModel : reservations
    {
        public ReservationModel()
        {

            carsList = new List<int>();
            startDate = DateTime.Today;
            endDate = DateTime.Today.AddDays(7);
            createDate = DateTime.Now;

        }

        public IList<int> carsList { get; set; }
        public string statusCode { get; set; }
        public string clientName { get; set; }
    }
}
