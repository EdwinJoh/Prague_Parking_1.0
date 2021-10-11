using System;
using System.Linq;


namespace PragueParking
{
    class Program
    {
        // Creating tree diffrent array/ list that can contain strings
        public static string[] ParkingList = new string[100];
        public static string[] TicketList = new string[200];
        public static string[] List = new string[200];


        static void Main(string[] args)
        {
            //Program will run untill the user press number seven
            int menuInput;
            do
            {
                menuInput = MainMenu();

            } while (menuInput != 7);
        }
        public static int MainMenu()                                    // Display the menu for the user and make it interactive using swich metod
        {
            Console.Clear();
            try
            {
                // The loop below will remove all the objects in the arratý when time hits 23.59. and add them to the List array
                TimeSpan time = DateTime.Now.TimeOfDay;
                if (time > new TimeSpan(23, 59, 00) && time <= new TimeSpan(0, 00, 00))
                {
                    TicketList.CopyTo(List, 0);
                    ParkingList = null;
                    Console.WriteLine("The time is now 23:59. All the vehicles that still are parked will be moved to diffrent parking lot.\nThe fine for this will be 600 SEK.");
                    Console.WriteLine("Vehicles parked:");
                    foreach (var vehicle in List)
                    {
                        if (vehicle != null)
                        {
                            Console.WriteLine(vehicle);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                MenuDesign();
                Console.WriteLine("Prague Parking\n" +
                 "Enter choise below\n" +
                "[1] Add vehicle\n" +
                "[2] See parked vehicles\n" +
                "[3] Move Vehicle\n" +
                "[4] Remove vehicle\n" +
                "[5] Search for vehicle\n" +
                "[6] Tickets\n" +
                "[7] Quit Program\n");

                int menuInput = int.Parse(Console.ReadLine());
                switch (menuInput)
                {
                    case 1:
                        VehicleType();
                        break;
                    case 2:
                        SeeVehicles();
                        break;
                    case 3:
                        VehicleTypeMove();
                        break;
                    case 4:
                        RemoveVehicle();
                        break;
                    case 5:
                        SearchVehicle();
                        break;
                    case 6:
                        Tickets();
                        break;
                    case 7:
                        Console.WriteLine("Program quitting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("The number is not in the menu. Enter an number from 1-7");
                        MainMenu();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("Wrong input. Try again with an integer {0}", e.Message);
                StandardReturnText();
            }
            return 7;
        }
        static void VehicleType()                                       // Letting the user choose if they want to add Car or MC
        {
            Console.Clear();
            Console.WriteLine("Choose a vehicle type below:\n" +
                "[1] Car\n" +
                "[2] MC\n" +
                "[3] Main menu");
            int vehicleInput = int.Parse(Console.ReadLine());

            if (vehicleInput == 1)
            {
                AddCar();
            }
            else if (vehicleInput == 2)
            {
                AddMc();
            }
            else if (vehicleInput == 3)
            {
                MainMenu();
            }
            else
            {
                Console.WriteLine("You can only choose 1 or 3 try again.");
                StandardReturnText();
            }
        }
        static void AddCar()                                            // Method for adding an Car
        {
            DateTime now = DateTime.Now;
            Console.Clear();
            Console.Write("Enter registration number:");
            string userInput = Console.ReadLine().ToUpper();
            userInput.Replace(" ", "");
            if (!SearchRegCar(userInput) && !SearchRegMC(userInput))
            {
                if (userInput.Length >= 4 && userInput.Length <= 10)
                {
                    for (int i = 0; i < ParkingList.Length; i++)
                    {
                        if (ParkingList[i] == null)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            ParkingList[i] = "CAR#" + userInput;
                            TicketList[i] = userInput + " " + now;
                            Console.WriteLine("Parked vehicle {0} at parking spot {1}\nParking started:{2}\n", userInput, i + 1, now);
                            StandardReturnText();
                        }
                        else if (ParkingList[i] != null)
                        {
                            continue;
                        }
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The parking lot is full");
                    StandardReturnText();
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input, either the registration number is fo short or to long\n" +
                      "minimum characters 4 and max characters is 10.");
                    StandardReturnText();
                }

            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle is already parked here");
                StandardReturnText();
            }
            MainMenu();

        }
        static bool SearchRegCar(string userInput)
        {

            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (ParkingList[i] == null)
                {
                    continue;
                }
                if (ParkingList[i] == "CAR#" + userInput)
                {
                    return true;
                }

            }

            return false;
        }                  // Searching for an string in our ParkingList array to see if it exist or not
        static void SeeVehicles()
        {
            Console.Clear();
            Console.WriteLine("Prague Parking");
            const int cols = 6;
            int n = 1;
            for (int i = 0; i < ParkingList.Length; i++)
            {
                Console.ResetColor();
                if (n >= cols && n % cols == 0)
                {
                    Console.WriteLine();
                    n = 1;
                }
                if (ParkingList[i] == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(i + 1 + ": Empty \t");
                    n++;

                }
                else if (ParkingList[i].Contains("CAR#"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(i + 1 + ": " + ParkingList[i] + "\t");
                    n++;
                }
                else if (ParkingList[i].Contains("MC#") && ParkingList[i].Length <= 10)
                {
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(i + 1 + ": " + ParkingList[i] + "\t");
                        n++;
                    }
                }
                else if (ParkingList[i].Contains("/"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(i + 1 + ": " + ParkingList[i] + "\t");
                    n++;
                }
            }
            StandardReturnText();
        }                                   // Display all the array index 
        static void StandardReturnText()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            MainMenu();

        }                            // Standard text used for not write the same text over and over again
        static int FindIndex(string userInput, out int index)
        {
            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (ParkingList[i] == null)
                {
                    continue;
                }
                else if (ParkingList[i] == "CAR#" + userInput || ParkingList[i] == "MC#" + userInput || ParkingList[i].Contains("MC#" + userInput))
                {
                    index = i;
                    return index;
                }
            }
            index = 0;
            return index;
        }       // Method for finding an index in out parkingList array
        static void RemoveVehicle()
        {
            Console.Clear();
            Console.WriteLine("Prague parking\tRemoving vehicle");
            Console.Write("Enter Registration number:");
            string userInput = Console.ReadLine().ToUpper();
            int index = FindIndex(userInput, out index);

            DateTime now = DateTime.Now;
            int ticketIndex = FindTicket(userInput);
            TimeSpan interval = new TimeSpan();

            if (SearchRegCar(userInput))
            {
                string ticketDate = TicketList[ticketIndex].Replace(userInput, "");
                DateTime checkInDate = DateTime.Parse(ticketDate);
                interval = now - checkInDate;

                if (ParkingList[index] == "CAR#" + userInput)
                {
                    Console.Clear();
                    ParkingList[index] = null;
                    TicketList[ticketIndex] = null;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Removing {0} Thanks for parking here.", userInput);
                    Console.WriteLine("Vehicle:{0} has been parked for:{1} ", userInput, interval.ToString(@"dd\.hh\:mm\:ss"));
                    StandardReturnText();
                }
            }
            if (SearchRegMC(userInput))
            {
                string ticketDate = TicketList[ticketIndex].Replace(userInput, "");
                DateTime checkInDate = DateTime.Parse(ticketDate);
                interval = now - checkInDate;

                if (ParkingList[index] == "MC#" + userInput || ParkingList[index].Contains("/"))
                {
                    TicketList[ticketIndex] = null;
                    if (ParkingList[index].Contains("/"))
                    {
                        string[] mcSplit = ParkingList[index].Split("/");
                        if (mcSplit[0] == "MC#" + userInput)
                        {
                            ParkingList[index] = mcSplit[1];
                        }
                        else
                        {
                            ParkingList[index] = mcSplit[0];
                        }
                    }
                    else
                    {
                        ParkingList[index] = null;
                    }
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Removing {0} Thanks for parking here.", userInput);
                Console.WriteLine("Vehicle:{0} has been parked for:{1} ", userInput, interval.ToString(@"dd\.hh\:mm\:ss"));
                StandardReturnText();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle:{0} is not parked here try again.", userInput);
                StandardReturnText();
            }
        }                                 // Method for removing a vehicle
        static void SearchVehicle()
        {
            Console.Clear();
            Console.WriteLine("Enter registration number:");
            string userInput = Console.ReadLine().ToUpper();
            if (SearchRegCar(userInput))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                int index = FindIndex(userInput, out index);
                Console.WriteLine("Vehicle {0} is parked at parking space:{1}", userInput, index + 1);
            }
            else if (SearchRegMC(userInput))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                int index = FindIndex(userInput, out index);
                Console.WriteLine("Vehicle {0} is parked at parking space:{1}", userInput, index + 1);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle:{0} is not parked here", userInput);
            }
            StandardReturnText();
        }                                 // Method for searching an vehicle in out parking list array
        static void AddMc()
        {
            Console.Clear();
            DateTime now = DateTime.Now;
            Console.Write("Enter registration number:");
            string userInput = Console.ReadLine().ToUpper();
            userInput.Replace(" ", "");
            if (!SearchRegCar(userInput) && !SearchRegMC(userInput))
            {
                if (userInput.Length >= 4 && userInput.Length <= 10)
                {
                    for (int i = 0; i < ParkingList.Length; i++)
                    {
                        if (ParkingList[i] == null)
                        {
                            ParkingList[i] = "MC#" + userInput;
                            TicketList[i] = userInput + " " + now;
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Parked vehicle {0} at parking spot:{1}\nParking started:{2}\n", userInput, i + 1, now);
                            StandardReturnText();
                        }
                        else if (ParkingList[i] != null)
                        {
                            if (ParkingList[i].Contains("/") || ParkingList[i].Contains("CAR"))
                            {
                                continue;
                            }

                            else if (ParkingList[i].Contains("MC#"))
                            {
                                string temp;
                                string seperator = "/MC#";
                                temp = string.Join(seperator, ParkingList[i], userInput);
                                ParkingList[i] = temp;
                                TicketList[i + 1] = userInput + " " + now;
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Parked vehicle {0} at parking spot:{1}\nParking started:{2}\n", userInput, i + 1, now);
                                StandardReturnText();
                            }
                        }
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The parking lot is full");
                    StandardReturnText();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input, either the registration number is fo short or to long\n" +
                    "minimum characters 4 and max characters is 10.");
                    StandardReturnText();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle is already parked here");
                StandardReturnText();
            }
        }                                         // Method for adding an MC
        static bool SearchRegMC(string userInput)
        {
            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (ParkingList[i] == null)
                {
                    continue;
                }
                else if (ParkingList[i] == "MC#" + userInput || ParkingList[i].Contains("MC#" + userInput))
                {
                    return true;
                }
            }
            return false;
        }                   // Searching for an string in our parking list array to see if it exist or not
        static void VehicleTypeMove()
        {
            Console.Clear();
            Console.WriteLine("Enter what you would like to move:\n" +
                "[1] Car\n" +
                "[2] MC\n" +
                "[3] Main menu");
            int userInput = int.Parse(Console.ReadLine());

            if (userInput == 1)
            {
                MoveCar();

            }
            else if (userInput == 2)
            {
                MoveMC();
            }
            else if (userInput == 3)
            {
                MainMenu();
            }
            else
            {
                Console.WriteLine("Wrong input. Number is not valid. try again");
                StandardReturnText();
            }
        }                               // Letting the user choose which type of vehicle they want to move
        static void MoveCar()
        {
            Console.Clear();
            Console.Write("Enter Registration number:");
            string userInput = Console.ReadLine().ToUpper();
            int index = FindIndex(userInput, out index);

            if (SearchRegCar(userInput))
            {
                Console.WriteLine("Enter new spot:");
                int newSpot = int.Parse(Console.ReadLine());
                if (ParkingList[newSpot - 1] == null)
                {
                    ParkingList[newSpot - 1] = ParkingList[index];
                    ParkingList[index] = null;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("This spot is allocated.");
                    StandardReturnText();
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Moving vehicle {0} to new spot {1}", userInput, newSpot);
                StandardReturnText();
                MainMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The vehicle is either a MC or not parked here.");
                StandardReturnText();
            }
        }                                      // Method for moving an Car to another index in our array
        static void MoveMC()
        {
            Console.Clear();
            Console.WriteLine("Enter Your registration number:");
            string userInput = Console.ReadLine().ToUpper();
            int index = FindIndex(userInput, out index);

            if (SearchRegMC(userInput))
            {

                Console.WriteLine("Enter new parking spot:");
                int newSpot = int.Parse(Console.ReadLine());
                if (ParkingList[newSpot - 1] == null)
                {
                    if (ParkingList[index] == "MC#" + userInput)
                    {
                        ParkingList[newSpot - 1] = ParkingList[index];
                        ParkingList[index] = null;
                    }
                    else if (ParkingList[index].Contains("/"))
                    {
                        string[] mcSplit = ParkingList[index].Split("/");
                        if (mcSplit[0] == "MC#" + userInput)
                        {
                            ParkingList[newSpot - 1] = mcSplit[0];
                            ParkingList[index] = mcSplit[1];
                        }
                        else if (mcSplit[1] == "MC#" + userInput)
                        {
                            ParkingList[newSpot - 1] = mcSplit[1];
                            ParkingList[index] = mcSplit[0];
                        }
                    }
                }
                else if (ParkingList[newSpot - 1].Contains("CAR#"))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is not enought space.There is an car parked here.");
                    StandardReturnText();
                }
                else
                {
                    if (ParkingList[newSpot - 1].Contains("/"))
                    {
                        Console.Clear();
                        Console.WriteLine("This spot is allocated by two MC already. please try again.");
                        StandardReturnText();
                    }
                    if (ParkingList[newSpot - 1].Contains("MC#"))
                    {
                        if (ParkingList[index] == "MC#" + userInput)
                        {
                            string seperator = "/MC#";
                            string temp = string.Join(seperator, ParkingList[newSpot - 1], userInput);
                            ParkingList[newSpot - 1] = temp;
                            ParkingList[index] = null;

                        }
                        else if (ParkingList[index].Contains("/"))
                        {
                            string[] mcSplit = ParkingList[index].Split("/");
                            if (mcSplit[0] == "MC#" + userInput)
                            {
                                string temp = string.Join("/MC#", ParkingList[newSpot - 1], userInput);
                                ParkingList[newSpot - 1] = temp;
                                ParkingList[index] = mcSplit[1];
                            }
                            else if (mcSplit[1] == "MC#" + userInput)
                            {
                                string temp = string.Join("/MC#", ParkingList[newSpot - 1], userInput);
                                ParkingList[newSpot - 1] = temp;
                                ParkingList[index] = mcSplit[0];
                            }
                        }
                    }

                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Moving vehicle {0} to new spot {1}", userInput, newSpot);
                StandardReturnText();
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle {0} is not parked here", userInput);
                StandardReturnText();

            }
        }                                       // Method for moving an Mc to another index in out array
        static void Tickets()
        {
            int count = 1;
            foreach (var Ticket in TicketList)
            {
                if (Ticket == null)
                {
                    continue;

                }
                else
                {
                    Console.WriteLine("{0}:{1}", count, Ticket);
                    count++;
                }
            }
            StandardReturnText();
        }                                      // Method for lopping true our Ticker array, contains License plate and time when parked
        static int FindTicket(string userInput)
        {
            for (int i = 0; i < TicketList.Length; i++)
            {
                if (TicketList[i] != null && TicketList[i].Contains(userInput))
                {
                    int index = i;
                    return index;
                }
            }
            return 0;
        }                   // Method used for finding the index in our Ticket array
        public static void MenuDesign()                             // Prague parking design for the menu.
    }
}


