using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260117_Car_Rental_System
{
    internal class Car
    {
        public string Name;
        public string Brand;
        public string Age;
        public string LicensePlate;
        public string borrowerName;
        public bool maintenanceStatus;
        public string maintenanceDetails;
        public string maintenanceWorker;
        public int startYear;
        public int startMonth;
        public int startDay;
        public int endYear;
        public int endMonth;
        public int endDay;
        public List<Maintenance> MaintenanceHistory;

        public Car(string name, string brand, string age, string licensePlate)
        {
            Name = name;
            Brand = brand;
            Age = age;
            LicensePlate = licensePlate;
            MaintenanceHistory = new List<Maintenance>();

        }
    }
}
