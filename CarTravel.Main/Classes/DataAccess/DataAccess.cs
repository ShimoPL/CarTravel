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
        public List<users> clientList { get; private set; }
        public List<cars> carsList { get; private set; }
        public DataAccess()
        {
        }

        public List<ReservationStatusModel> getStatusList()
        {
            using (var db = new CarTravelDb())
            {
                statusList = db.reservations_status.Select(s => new ReservationStatusModel
                {
                    Code = s.code,
                    DisplayAs = s.displayAs
                }).ToList();
            }
            return statusList;
        }

        public List<users> getClientList(int takeLast = 0, string filterQuery = null)
        {
            if (filterQuery != null) filterQuery = filterQuery.ToLower();
            using (var db = new CarTravelDb())
            {
                var clients = db.users.Where(u => (
                      u.role.ToLower() == "c"
                     && (filterQuery != null ? (u.firstName.ToLower().Contains(filterQuery) || u.lastName.ToLower().Contains(filterQuery) || u.email.ToLower().Contains(filterQuery)) : true)
                 ));
                if (takeLast < 1)
                    clientList = clients.ToList();
                else
                    clientList = clients.Reverse().Take(takeLast).ToList();
            }
            return clientList;
        }

        public List<cars> getCarsList()
        {
            using (var db = new CarTravelDb())
            {
                carsList = db.cars.ToList();
            }
            return carsList;
        }
        public List<ReservationModel> getReservationsList(DateTime fromDate, DateTime toDate)
        {
            if (statusList == null || statusList.Count == 0) getStatusList();

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
                              createDate = rgroupped.Key.createDate,
                              statusCode = rgroupped.Key.status,
                              clientName = db.users.Where(u => u.userId == rgroupped.Key.client).Select(u => u.firstName + " " + u.lastName).FirstOrDefault(),
                              carsList = rgroupped.Select(p => p.rc.carId)
                          };

                var reservation = tmp.ToList().Select(o => new ReservationModel
                {
                    reservationId = o.reservationId,
                    startDate = o.startDate,
                    endDate = o.endDate,
                    createDate = o.createDate,
                    statusCode = o.statusCode,
                    status = statusList.Where(s => s.Code == o.statusCode).Select(s => s.DisplayAs).FirstOrDefault(),
                    clientName = o.clientName,
                    carsList = o.carsList.ToList()
                }).ToList();

                return reservation;
            }
        }

        public ReservationModel getReservation(long reservationId)
        {
            using (var db = new CarTravelDb())
            {
                var reservation = from r in db.reservations.ToList()
                                  join rc in db.reservations_cars.ToList() on r.reservationId equals rc.reservationId
                                  where r.reservationId == reservationId
                                  group new { r, rc } by r into rgroupped
                                  select new ReservationModel
                                  {
                                      reservationId = rgroupped.Key.reservationId,
                                      startDate = rgroupped.Key.startDate,
                                      endDate = rgroupped.Key.endDate,
                                      statusCode = rgroupped.Key.status,
                                      client = rgroupped.Key.client,
                                      carsList = rgroupped.Select(p => p.rc.carId).ToList(),
                                      comment = rgroupped.Key.comment,
                                      modifiedBy = rgroupped.Key.modifiedBy,
                                      modifiedOn = rgroupped.Key.modifiedOn,
                                      createDate = rgroupped.Key.createDate
                                  };
                return reservation.FirstOrDefault();
            }
        }
    }
}
