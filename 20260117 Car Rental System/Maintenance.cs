using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260117_Car_Rental_System
{
    internal class Maintenance
    {
        public Car Car;
        public string MaintenanceDetails;
        public string MaintenanceWorker;
        public string StartDate;

        public Maintenance(Car car, string maintenanceDetails, string maintenanceWorker, string startDate)
        {
            Car = car;
            MaintenanceDetails = maintenanceDetails;
            MaintenanceWorker = maintenanceWorker;
            StartDate = startDate;
        }

    }
}
