using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260117_Car_Rental_System
{
    internal class Cars_Out
    {
        public static List<Borrowed_Car> carsRented = new List<Borrowed_Car>();

        public static void InitializeCarsOutList()
        {
            File_Manager file_Manager = new File_Manager("cars_out.csv");

            foreach (string line in file_Manager.getLines())
            {
                Car car = new Car(line.Split(',')[0].Trim(), line.Split(',')[1].Trim(), line.Split(',')[2].Trim(), line.Split(',')[3].Trim());
                Borrowed_Car borrowed_Car = new Borrowed_Car(car, line.Split(',')[4].Trim(), line.Split(',')[5].Trim(), line.Split(',')[6].Trim());
                carsRented.Add(borrowed_Car);
            }
        }

        public static void ExportCarsOutList()
        {
            File_Manager file_Manager = new File_Manager("cars_out.csv");
            
            List<string> lines = new List<string>();

            foreach (Borrowed_Car borrowed_Car in carsRented)
            {
                lines.Add($"{borrowed_Car.Car.Name},{borrowed_Car.Car.Brand},{borrowed_Car.Car.Age},{borrowed_Car.Car.LicensePlate},{borrowed_Car.BorrowerName},{borrowed_Car.StartDateTime},{borrowed_Car.EndDateTime}");
            }

            file_Manager.Write(lines, false);
        }
    }
}
