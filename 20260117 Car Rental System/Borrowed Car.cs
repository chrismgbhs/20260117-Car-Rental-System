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
        public string StartDateTime;
        public string EndDateTime;

        public Borrowed_Car(Car car, string borrowerName, string startDateTime, string endDateTime)
        {
            Car = car;
            BorrowerName = borrowerName;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }
    }
}
