using CarTravel.Main.Classes.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CarTravel.Main.Classes.DataAccess
{
    class DataAccess
    {
        public List<ReservationStatusModel> statusList { get; private set; }
        public DataAccess()
        {
            using (var db = new CarTravelDb())
            {
                statusList = db.reservations_status.Select(s => new ReservationStatusModel
                {
                    Code = s.code,
                    DisplayAs = s.displayAs
                }).ToList();
            }
        }
        public List<ResrvationModel> getReservationsList(DateTime fromDate, DateTime toDate)
        {
            using (var db = new CarTravelDb())
            {
                var tmp = from r in db.reservations.ToList()
                                  join rc in db.reservations_cars.ToList() on r.reservationId equals rc.reservationId
                                  where r.createDate >= fromDate && r.createDate <= toDate
                                  group new { r, rc } by r into rgroupped
                                  select new
                                  {
                                      reservationId = rgroupped.Key.reservationId,
                                      startDate = rgroupped.Key.startDate,
                                      endDate = rgroupped.Key.endDate,
                                      statusCode = rgroupped.Key.status,
                                      client = db.users.Where(u => u.userId == rgroupped.Key.client).Select(u => u.firstName + " " + u.lastName).FirstOrDefault(),
                                      carsList = rgroupped.Select(p => p.rc.carId)
                                  };

                var reservation = tmp.ToList().Select(o => new ResrvationModel
                {
                    reservationId = o.reservationId,
                    startDate = o.startDate,
                    endDate = o.endDate,
                    statusCode = o.statusCode,
                    status = statusList.Where(s => s.Code == o.statusCode).Select(s => s.DisplayAs).FirstOrDefault(),
                    client = o.client,
                    carsList = o.carsList.ToList()
                }).ToList();

                return reservation;
            } 
        }
    }
}
