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
            Cars_In.InitializeCarsInList();
            Cars_Out.InitializeCarsOutList();
            Cars_in_Maintenance.InitializeMaintenancesList();
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

            foreach (Borrowed_Car borrowed_car in Cars_Out.carsRented)
            {
                if (borrowed_car.Car.LicensePlate == plateNumber)
                {
                    duplicateFound = true;
                }
            }

            foreach (Maintenance maintenance in Cars_in_Maintenance.carsInMaintenance)
            {
                if (maintenance.Car.LicensePlate == plateNumber)
                {
                    duplicateFound = true;
                }
            }

            return duplicateFound;
        }

        public static void MainMenu()
        {
            int choice = 0;
            while (choice != 10)
            {
                Console.Clear();
                Console.WriteLine("DO NOT CLOSE THE CONSOLE, INSTEAD JUST PLEASE SAVE AND EXIT.");
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. View Available Cars");
                Console.WriteLine("2. Rent a Car");
                Console.WriteLine("3. View Rented Cars");
                Console.WriteLine("4. Return a Car");
                Console.WriteLine("5. Send Car to Maintenance or Return Car from Maintenance");
                Console.WriteLine("6. View Cars in Maintenance");
                Console.WriteLine("7. View Mainthenance History");
                Console.WriteLine("8. Add a Car");
                Console.WriteLine("9. Add Multiple Cars via CSV file");
                Console.WriteLine("10. Exit and Save");
                Console.WriteLine();

                while (true)
                {
                    Console.Write("Select an option (1-10): ");
                    int.TryParse(Console.ReadLine(), out choice);

                    if (choice > 0 && choice < 11)
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
                        ReturnToMainMenu();
                        break;
                    case 5:
                        SendCarToMaintenanceOrReturnCarFromMaintenance();
                        ReturnToMainMenu();
                        break;
                    case 6:
                        ViewCarsInMaintenance();
                        ReturnToMainMenu();
                        break;
                    case 7:
                        ViewMaintenanceHistory();
                        ReturnToMainMenu();
                        break;
                    case 8:
                        AddCar();
                        ReturnToMainMenu();
                        break;
                    case 9:
                        AddCars();
                        ReturnToMainMenu();
                        break;
                    case 10:
                        ExitSystem();
                        break;
                }
            }
            
        }

        public static void AddCars()
        {
            Console.Clear();
            Console.Write("Enter the path of the CSV file to import cars from: ");
            string filePath = Console.ReadLine();
            File_Manager file_Manager = new File_Manager(filePath);
            List<string> lines = file_Manager.getLines();
            foreach (string line in lines)
            {
                string[] carDetails = line.Split(',');
                if (carDetails.Length == 4)
                {
                    string name = carDetails[0].Trim();
                    string brand = carDetails[1].Trim();
                    string age = carDetails[2].Trim();
                    string licensePlate = carDetails[3].Trim();
                    if (!CheckDuplicates(licensePlate))
                    {
                        Car car = new Car(name, brand, age, licensePlate);
                        Cars_In.carsAvailable.Add(car);
                        Console.WriteLine($"{car.Name} has been added to the inventory successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Duplicate license plate found for {licensePlate}. Car not added.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid line format. Each line must contain exactly 4 values: Name, Brand, Age, License Plate.");
                }
            }
            Console.WriteLine("Car import process completed.");
        }

        public static void AddCar()
        {
            Console.Clear();
            string name;
            string brand;
            string age;
            string licensePlate;
            Console.Write("Enter car name/model: ");
            name = Console.ReadLine();
            Console.Write("Enter car brand: ");
            brand =  Console.ReadLine();
            Console.Write("Enter car age: ");
            age = Console.ReadLine();
            while (true)
            {
                Console.Write("Enter car license plate number: ");
                licensePlate = Console.ReadLine();
                if (!CheckDuplicates(licensePlate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Duplicate license plate found. Please enter a unique license plate number.");
                }
            }
            Car car = new Car(name, brand, age, licensePlate);
            Cars_In.carsAvailable.Add(car);
            Console.WriteLine($"{car.Name} has been added to the inventory successfully.");
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

            foreach (Borrowed_Car borrowed_car in Cars_Out.carsRented)
            {
                counter++;
                Console.WriteLine($"{counter}. {borrowed_car.StartDateTime} to {borrowed_car.EndDateTime}: {borrowed_car.Car.Name} - {borrowed_car.Car.Brand} - {borrowed_car.Car.Age} - {borrowed_car.Car.LicensePlate} | Borrower: {borrowed_car.BorrowerName}");
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

            string startDateTime = $"{DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Year}";
            string endDateTime = $"{rentalMonth}/{rentalDay}/{rentalYear}";
            Borrowed_Car borrowed_car = new Borrowed_Car(Cars_In.carsAvailable[carNumber-1],borrowerName,startDateTime,endDateTime);
            Cars_Out.carsRented.Add(borrowed_car);

            Console.WriteLine($"{borrowed_car.Car.Name} has been rented on {borrowed_car.StartDateTime} until {borrowed_car.EndDateTime}.");

            List<string> content = new List<string>();
            content.Add(DateTime.Now.ToString());

            content.Add($"Model: {borrowed_car.Car.Name} | Plate Number: {borrowed_car.Car.LicensePlate}");
            content.Add($"Borrowed by {borrowed_car.BorrowerName} from {borrowed_car.StartDateTime} until {borrowed_car.EndDateTime}");

            Cars_In.carsAvailable.RemoveAt(carNumber - 1);

            File_Manager file_manager = new File_Manager("receipt.csv");
            file_manager.Write(content, false);
            Console.WriteLine("Please enter any key to go back to main menu. Receipt has been printed.");
            Console.ReadKey();
        }

        public static void ReturnCar()
        {
            int carNumber;
            ViewRentedCars();
            while(true)
            {
                Console.Write("Enter the number of the car you want to return: ");
                int.TryParse(Console.ReadLine(), out carNumber);

                if (carNumber > 0 && carNumber <= Cars_Out.carsRented.Count)
                {
                    break;
                }

                else
                {
                    Console.WriteLine("Invalid car number.");
                }
            }

            Cars_In.carsAvailable.Add(Cars_Out.carsRented[carNumber - 1].Car);
            Cars_Out.carsRented.RemoveAt(carNumber - 1);
            Console.WriteLine("Car has been returned successfully.");
        }

        public static void SendCarToMaintenanceOrReturnCarFromMaintenance()
        {
            Console.Clear();
            int choice = 0;

            while (choice < 1 || choice > 2)
            {
                Console.Write("Enter 1 to send a car to maintenance or 2 to return a car from maintenance: ");
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        ViewAvailableCars();
                        int carNumber;

                        while (true)
                        {
                            Console.Write("Enter the car number to send to maintenance: ");
                            if (int.TryParse(Console.ReadLine(), out carNumber))
                            {
                                break;
                            }

                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid car number.");
                            }    
                        }

                        Console.Write("Enter maintenance details: ");
                        string maintenanceDetails = Console.ReadLine();

                        Console.Write("Enter maintenance worker name: ");
                        string maintenanceWorker = Console.ReadLine();

                        Cars_in_Maintenance.carsInMaintenance.Add(new Maintenance(Cars_In.carsAvailable[carNumber - 1], maintenanceDetails, maintenanceWorker, DateTime.Now.ToString()));
                        Cars_In.carsAvailable.RemoveAt(carNumber - 1);

                        break;
                    case 2:
                        ViewCarsInMaintenance();
                        int returnCarNumber;

                        while (true)
                        {
                            Console.Write("Enter the car number to return from maintenance: ");
                            if (int.TryParse(Console.ReadLine(), out returnCarNumber))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid car number.");
                            }
                        }

                        Cars_In.carsAvailable.Add(Cars_in_Maintenance.carsInMaintenance[returnCarNumber - 1].Car);
                        File_Manager file_Manager = new File_Manager($"{Cars_in_Maintenance.carsInMaintenance[returnCarNumber - 1].Car.LicensePlate}");

                        List<string> content = new List<string>();
                        content.Add($"{Cars_in_Maintenance.carsInMaintenance[returnCarNumber - 1].StartDate} | Maintenance details: {Cars_in_Maintenance.carsInMaintenance[returnCarNumber - 1].MaintenanceDetails} | Maintenance worker: {Cars_in_Maintenance.carsInMaintenance[returnCarNumber - 1].MaintenanceWorker} | Completed: {DateTime.Now}");

                        file_Manager.Write(content);
                        Console.WriteLine("Car has been returned from maintenance successfully.");
                        File_Manager file_manager = new File_Manager($"{Cars_in_Maintenance.carsInMaintenance[returnCarNumber - 1].Car.LicensePlate}.csv");
                        file_manager.Write(content);
                        Cars_in_Maintenance.carsInMaintenance.RemoveAt(returnCarNumber - 1);

                        break;
                    
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                        break;
                }
            }
        }

        public static void ViewCarsInMaintenance()
        {
            Console.Clear();
            Console.WriteLine("Cars in Maintenance:");
            for (int counter = 0; counter < Cars_in_Maintenance.carsInMaintenance.Count; counter++)
            {
                Maintenance maintenance = Cars_in_Maintenance.carsInMaintenance[counter];
                Console.WriteLine($"{counter + 1}. {maintenance.Car.Name} - {maintenance.Car.Brand} - {maintenance.Car.Age} - {maintenance.Car.LicensePlate} | Maintenance staff: {maintenance.MaintenanceWorker} | Maintenance details: {maintenance.MaintenanceDetails}");
            }
        }

        public static void ExitSystem()
        {
            Console.WriteLine("Exiting the system. Goodbye!");
            Cars_Out.ExportCarsOutList();
            Cars_in_Maintenance.ExportMaintenancesList();
            Cars_In.ExportCarsInList();
        }

        public static void ReturnToMainMenu()
        {
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
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
            Console.Clear();
            Console.Write("Enter the plate number of the car to view maintenance history: ");
            string plateNumber = Console.ReadLine();

            File_Manager file_Manager = new File_Manager($"{plateNumber}.csv");
            List<string> lines = file_Manager.getLines();
            Console.WriteLine($"Maintenance History for Car with Plate Number {plateNumber}: ");
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
