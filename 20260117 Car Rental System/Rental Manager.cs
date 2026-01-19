using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _20260117_Car_Rental_System
{
    internal class Rental_Manager
    {
        public static void StartRentalSystem()
        {
            Console.WriteLine("Welcome to the Car Rental System!");
        }

        public static void AddCarToInventory()
        {
            Console.WriteLine("Adding cars to inventory from file...");
            File_Manager carFileManager = new File_Manager("cars.csv");

            if (!carFileManager.Read())
            {
                Console.WriteLine("Error reading car data. Exiting...");
                return;
            }

            else
            {
                List<string> carLines = carFileManager.getLines();

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

                        if (!CheckDuplicates(plateNumber))
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

        public static bool CheckDuplicates(string plateNumber)
        {
            bool duplicateFound = false;

            foreach (Car car in Cars_In.carsAvailable)
            {
                if (car.LicensePlate == plateNumber)
                {
                    duplicateFound = true;
                }
            }

            foreach (Car car in Cars_Out.carsRented)
            {
                if (car.LicensePlate == plateNumber)
                {
                    duplicateFound = true;
                }
            }

            foreach (Car car in Cars_in_Maintenance.carsInMaintenance)
            {
                if (car.LicensePlate == plateNumber)
                {
                    duplicateFound = true;
                }
            }

            return duplicateFound;
        }

        public static void MainMenu()
        {
            int choice;

            Console.Clear();
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. View Available Cars");
            Console.WriteLine("2. Rent a Car");
            Console.WriteLine("3. View Rented Cars");
            Console.WriteLine("4. Return a Car");
            Console.WriteLine("5. Send Car to Maintenance");
            Console.WriteLine("6. View Cars in Maintenance");
            Console.WriteLine("7. View Mainthenance History");
            Console.WriteLine("8. View Mainthenance History");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Select an option (1-6): ");
                int.TryParse(Console.ReadLine(), out choice);

                if (choice > 0 && choice < 7)
                {
                    break;
                }

                else
                {
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                }
            }

            switch (choice)
            {
                case 1:
                    ViewAvailableCars();
                    ReturnToMainMenu();
                    break;
                case 2:
                    RentCar();
                    break;
                case 3:
                    ViewRentedCars();
                    ReturnToMainMenu();
                    break;
                case 4:
                    ReturnCar();
                    break;
                case 5:
                    SendCarToMaintenance();
                    break;
                case 6:
                    ViewCarsInMaintenance();
                    break;
                case 7:
                    ViewMaintenanceHistory();
                    break;
                case 8:
                    Console.WriteLine("Exiting the system. Goodbye!");
                    break;
            }
        }

        public static void ViewAvailableCars()
        {
            Console.Clear();
            Console.WriteLine("Available Cars:");

            for (int counter = 0; counter < Cars_In.carsAvailable.Count; counter++)
            {
                Console.WriteLine($"{counter+1}. {Cars_In.carsAvailable[counter].Name} - {Cars_In.carsAvailable[counter].Brand} - {Cars_In.carsAvailable[counter].Age} - {Cars_In.carsAvailable[counter].LicensePlate}");
            }
        }

        public static void ViewRentedCars()
        {
            Console.Clear();

            int counter = 0;

            Console.WriteLine("View Rented Cars:");

            foreach (Car car in Cars_Out.carsRented)
            {
                counter++;
                Console.WriteLine($"{counter}. {car.startMonth}/{car.startDay}/{car.startYear} to {car.endMonth}/{car.endDay}/{car.endYear}: {car.Name} - {car.Brand} - {car.Age} - {car.LicensePlate} | Borrower: {car.borrowerName}");
            }
        }

        public static void RentCar()
        {
            int carNumber;
            int rentalYear;
            int rentalMonth;
            int rentalDay;
            bool thirtyOneDays = false;

            Console.Clear();
            ViewAvailableCars();
            Console.WriteLine();

            while (true)
            {

                Console.Write("Enter the number of the car you want to rent: ");
                int.TryParse(Console.ReadLine(), out carNumber);

                if (carNumber > 0 && carNumber <= Cars_In.carsAvailable.Count)
                {
                     break;
                }

                else
                {
                     Console.WriteLine("Invalid car number.");
                }     
            }
            
            Console.Write("Enter your name: ");
            string borrowerName = Console.ReadLine();

            while (true)
            {
                
                Console.Write("Enter return year: ");
                int.TryParse(Console.ReadLine(), out rentalYear);

                if (rentalYear >= DateTime.Now.Year && rentalYear < DateTime.Now.Year+2)
                {
                    break;
                }

                else
                {
                    Console.WriteLine("Invalid year. Please enter a valid year within two years.");
                }
            }

            while (true)
            {
                Console.Write("Enter return month (1-12): ");
                int.TryParse(Console.ReadLine(), out rentalMonth);

                if (rentalYear == DateTime.Now.Year)
                {
                    if (rentalMonth >= DateTime.Now.Month && rentalMonth <= 12)
                    {
                        Check31Days(rentalMonth);
                        break;   
                    }

                    else
                    {
                        Console.WriteLine("Invalid rental month.");
                    }
                }

                else
                {
                    if (rentalMonth >= 1 && rentalMonth <= 12)
                    {
                        Check31Days(rentalMonth);
                        break;
                    }

                    else
                    {
                        Console.WriteLine("Invalid rental month input.");
                    }
                }
            }

            while (true)
            {
                int monthDays;

                if (thirtyOneDays)
                {
                    monthDays = 31;
                }

                else
                {
                    monthDays = 30;
                }

                Console.Write("Enter return day: ");
                int.TryParse(Console.ReadLine(), out rentalDay);

                if (rentalYear == DateTime.Now.Year)
                {
                    if (rentalMonth == DateTime.Now.Month)
                    {
                        if (rentalDay >= DateTime.Now.Day && rentalDay <= monthDays)
                        {
                            break;
                        }

                        else
                        {
                            Console.WriteLine("Invalid rental day.");
                        }
                    }

                    else
                    {
                        if (rentalDay >= 1 && rentalDay <= monthDays)
                        {
                            break;
                        }

                        else
                        {
                            Console.WriteLine("Invalid rental day.");
                        }
                    }
                }

                else
                {
                    if (rentalDay >= 1 && rentalDay <= monthDays)
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("Invalid rental day.");
                    }
                }
            }

            Cars_Out.carsRented.Add(Cars_In.carsAvailable[carNumber - 1]);
            Cars_In.carsAvailable[carNumber - 1].borrowerName = borrowerName;
            Cars_In.carsAvailable[carNumber - 1].startYear = DateTime.Today.Year;
            Cars_In.carsAvailable[carNumber - 1].startMonth = DateTime.Today.Month;
            Cars_In.carsAvailable[carNumber - 1].startDay = DateTime.Today.Day;
            Cars_In.carsAvailable[carNumber - 1].endYear = rentalYear;
            Cars_In.carsAvailable[carNumber - 1].endMonth = rentalMonth;
            Cars_In.carsAvailable[carNumber - 1].endDay = rentalDay;
            Console.WriteLine($"{Cars_In.carsAvailable[carNumber - 1].Name} has been rented on {Cars_In.carsAvailable[carNumber - 1].startYear}/{Cars_In.carsAvailable[carNumber - 1].startMonth}/{Cars_In.carsAvailable[carNumber - 1].startDay} until {Cars_In.carsAvailable[carNumber - 1].endYear}/{Cars_In.carsAvailable[carNumber - 1].endMonth}/{Cars_In.carsAvailable[carNumber - 1].endDay}.");

            List<string> content = new List<string>();
            content.Add(DateTime.Now.ToString());
            content.Add($"Model: {Cars_In.carsAvailable[carNumber - 1].Name} | Plate Number: {Cars_In.carsAvailable[carNumber - 1].LicensePlate}");
            content.Add($"Borrowed by {Cars_In.carsAvailable[carNumber - 1].borrowerName} from {Cars_In.carsAvailable[carNumber - 1].startYear}/{Cars_In.carsAvailable[carNumber - 1].startMonth}/{Cars_In.carsAvailable[carNumber - 1].startDay} until {Cars_In.carsAvailable[carNumber - 1].endYear}/{Cars_In.carsAvailable[carNumber - 1].endMonth}/{Cars_In.carsAvailable[carNumber - 1].endDay}");

            Cars_In.carsAvailable.RemoveAt(carNumber - 1);
            File_Manager file_manager = new File_Manager("receipt.csv");
            file_manager.Write(content, false);
            Console.WriteLine("Please enter any key to go back to main menu. Receipt has been printed.");
            Console.ReadKey();
            MainMenu();
        }

        public static void ReturnCar()
        {
            ViewRentedCars();

        }

        public static void SendCarToMaintenance()
        {
            // Implementation for sending a car to maintenance
        }

        public static void ViewCarsInMaintenance()
        {
            Console.Clear();
            Console.WriteLine("Cars in Maintenance:");
            foreach (Car car in Cars_in_Maintenance.carsInMaintenance)
            {
                Console.WriteLine($"{car.startMonth}/{car.startDay}/{car.startYear} to {car.endMonth}/{car.endDay}/{car.endYear}: {car.Name} - {car.Brand} - {car.Age} - {car.LicensePlate} | Maintenance staff: {car.maintenanceWorker}");
            }
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
            MainMenu();
        }

        public static void ExitSystem()
        {
            Console.WriteLine("Exiting the system. Goodbye!");
        }

        public static void ReturnToMainMenu()
        {
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
            MainMenu();
        }

        public static bool Check31Days(int rentalMonth)
        {
            bool thirtyOneDays = false;

            switch (rentalMonth)
            {
                case 1:
                    thirtyOneDays = true;
                    break;
                case 2:
                    thirtyOneDays = false;
                    break;
                case 3:
                    thirtyOneDays = true;
                    break;
                case 4:
                    thirtyOneDays = false;
                    break;
                case 5:
                    thirtyOneDays = true;
                    break;
                case 6:
                    thirtyOneDays = false;
                    break;
                case 7:
                    thirtyOneDays = true;
                    break;
                case 8:
                    thirtyOneDays = true;
                    break;
                case 9:
                    thirtyOneDays = false;
                    break;
                case 10:
                    thirtyOneDays = true;
                    break;
                case 11:
                    thirtyOneDays = false;
                    break;
                case 12:
                    thirtyOneDays = true;
                    break;
            }

            return thirtyOneDays;
        }

        public static void SendReceipt()
        {

        }

        public static void ViewMaintenanceHistory()
        {

        }
    }
}
