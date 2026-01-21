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
        public int StartYear;
        public int StartMonth;
        public int StartDay;
        public int EndYear;
        public int EndMonth;
        public int EndDay;

        public Maintenance(Car car, string maintenanceDetails, string maintenanceWorker, int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
        {
            Car = car;
            MaintenanceDetails = maintenanceDetails;
            MaintenanceWorker = maintenanceWorker;
            StartYear = startYear;
            StartMonth = startMonth;
            StartDay = startDay;
            EndYear = endYear;
            EndMonth = endMonth;
            EndDay = endDay;
        }

    }
}
