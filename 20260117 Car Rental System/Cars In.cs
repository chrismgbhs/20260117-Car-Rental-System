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

        public static void ExportCarsInList()
        {
            File_Manager file_Manager = new File_Manager("cars_in.csv");
            List<string> lines = new List<string>();
            foreach (Car car in carsAvailable)
            {
                string line = $"{car.Name},{car.Brand},{car.Age},{car.LicensePlate}";
                lines.Add(line);
            }
            file_Manager.Write(lines, false);
        }

        public static void InitializeCarsInList()
        {
            List<string> carLines = new List<string>();
            Console.WriteLine("Adding cars to inventory from file...");
            File_Manager carFileManager = new File_Manager("cars_in.csv");

            if (!carFileManager.Read())
            {
                Console.WriteLine("Error reading car data. Exiting...");
            }

            else
            {
                carLines = carFileManager.getLines();

                foreach (string line in carLines)
                {
                    //Thread.Sleep(100);
                    string[] parts = line.Split(',');
                    if (parts.Length >= 4)
                    {
                        string modelName = parts[0].Trim();
                        string brand = parts[1].Trim();
                        string age = parts[2].Trim();
                        string plateNumber = parts[3].Trim();
                        Car car = new Car(modelName, brand, age, plateNumber);

                        if (!Rental_Manager.CheckDuplicates(plateNumber))
                        {
                            Cars_In.AddCar(car);
                            Console.WriteLine($"Added car: {modelName}, {brand}, {age}, {plateNumber}");
                        }

                        else
                        {
                            Console.WriteLine($"Duplicate car found with plate number: {plateNumber}. Skipping addition.");
                        }


                    }

                    else
                    {
                        Console.WriteLine($"Invalid car data line: {line}");
                    }
                }
            }

            Console.WriteLine("Car inventory loading complete. Enter any key to continue.");
            Console.ReadKey();
        }
    }
}
