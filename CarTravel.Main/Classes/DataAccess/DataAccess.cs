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
                var reservation = db.reservations.Where(r => r.createDate >= fromDate && r.createDate <= toDate)
                    .Select(r => new
                    {
                        reservationId = r.reservationId,
                        startDate = r.startDate,
                        endDate = r.endDate,
                        statusCode = r.status,
                        client = db.users.Where(u => u.userId == r.client).Select(u => u.firstName + " " + u.lastName).FirstOrDefault(),
                        //carsList = db.reservations_cars.Select(c => c).Where(c => c.reservationId == r.reservationId).ToArray()
                    }
                ).ToList().Select(o => new ResrvationModel
                {
                    reservationId = o.reservationId,
                    startDate = o.startDate,
                    endDate = o.endDate,
                    statusCode = o.statusCode,
                    status = statusList.Where(s => s.Code == o.statusCode).Select(s => s.DisplayAs).FirstOrDefault(),
                    client = o.client,
                    //carsList = o.carsList.ToList()
                }).ToList();

                return reservation;
            } 
        }
    }
}
