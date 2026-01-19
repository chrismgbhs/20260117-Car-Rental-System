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
        public DateTime ScheduleStart;
        public DateTime ScheduleEnd;

        public Borrowed_Car(Car car, string borrowerName, DateTime scheduleStart, DateTime scheduleEnd)
        {
            Car = car;
            BorrowerName = borrowerName;
            ScheduleStart = scheduleStart;
            ScheduleEnd = scheduleEnd;
        }
    }
}
