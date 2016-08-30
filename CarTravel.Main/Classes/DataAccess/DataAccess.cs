using CarTravel.Main.Classes.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    .Select(r => new ResrvationModel
                    {
                        reservationId = r.reservationId,
                        startDate = r.startDate,
                        endDate = r.endDate,
                        statusCode = r.status,
                        status = db.reservations_status.Where(s => s.code == r.status).Select(s => s.displayAs).FirstOrDefault(),
                        client = db.users.Where(u => u.userId == r.client).Select(u => u.firstName + " " + u.lastName).FirstOrDefault(),
                        carsList = db.reservations_cars.Where(c => c.reservationId == r.reservationId).Select(c => c.carId).ToList()
                    }
                ).ToList();
                return reservation;
            }
        }
    }
}
