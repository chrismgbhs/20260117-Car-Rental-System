using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260117_Car_Rental_System
{
    internal class Cars_In
    {
        public static List<Car> carsAvailable = new List<Car>();

        public static void AddCar(Car car)
        {
            carsAvailable.Add(car);
        }
    }
}
