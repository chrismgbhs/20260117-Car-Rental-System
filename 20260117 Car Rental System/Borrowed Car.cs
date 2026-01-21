using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260117_Car_Rental_System
{
    internal class Borrowed_Car
    {
        public Car Car;
        public string BorrowerName;
        public int StartYear;
        public int StartMonth;
        public int StartDay;
        public int EndYear;
        public int EndMonth;
        public int EndDay;

        public Borrowed_Car(Car car, string borrowerName, int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
        {
            Car = car;
            BorrowerName = borrowerName;
            StartYear = startYear;
            StartMonth = startMonth;
            StartDay = startDay;
            EndYear = endYear;
            EndMonth = endMonth;
            EndDay = endDay;
        }
    }
}