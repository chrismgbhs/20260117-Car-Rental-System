using NAudio.Wave;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace _20260117_Car_Rental_System
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Rental_Manager.StartRentalSystem();
            Rental_Manager.AddCarToInventory();
            Rental_Manager.MainMenu();

            // Further implementation goes here
        }
    }
}
