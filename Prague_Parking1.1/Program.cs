using System;
using System.Linq;


namespace PragueParking
{
    class Program
    {
        public static string[] ParkingList = new string[100];
        public static string[] Tickets = new string[200];

        static void Main(string[] args)
        {
            MainMenu();
        }
        public static void MainMenu()
        {
            try
            {
                TimeSpan start = TimeSpan.Parse("23:58"); 
                TimeSpan end = TimeSpan.Parse("23:59");   
                TimeSpan now = DateTime.Now.TimeOfDay;
                if (start <= end)
                {
                    if (now >= start && now <= end)
                    {
                        ParkingList = null;
                        Console.WriteLine("The time is now 00.00, all the vehicles that are parked here now is been moved to a diffrent parkinglot\n"+
                        "Parked vehicles is fined xx SEK.");
                    }
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Prgue Parking\n" +
                 "Enter choise below\n" +
                "[1] Add vehicle\n" +
                "[2] See parked vehicles\n" +
                "[3] Move Vehicle\n" +
                "[4] Remove vehicle\n" +
                "[5] Search for vehicle\n" +
                "[6] TIckets\n" +
                "[9] Quit Program\n");

                int menuInput = int.Parse(Console.ReadLine());
                switch (menuInput)
                {
                    case 1:
                        VehicleType();
                        break;
                    case 2:
                        SeeParkedVehicles();
                        break;
                    case 3:
                        Move();
                        break;
                    case 4:
                        RemoveVehicle();
                        break;
                    case 5:
                        Search();
                        break;
                    case 6:
                        Ticket();
                        break;
                    case 9:
                        Console.WriteLine("Program quitting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("The number is not in the menu. Enter an number from 1-9");
                        MainMenu();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("Wrong input, try again with an integer {0}", e.Message);
                MainMenu();
            }
        }
        public static void VehicleType()
        {
            Console.Clear();
            Console.WriteLine("*** Park vehicle ***");
            Console.WriteLine("Do you have a car or an MC:\n" +
                "[1] Car\n" +
                "[2] Mc\n" +
                "[3] Main Menu");
            int userInput = int.Parse(Console.ReadLine());

            if (userInput == 1)
            {
                ParkCar();
            }
            if (userInput == 2)
            {
                McPark();
            }
            else if (userInput == 3)
            {
                Console.Clear();
                MainMenu();

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong input, try an number from 1 - 3. press any key to go back...");
                Console.ReadKey();
                Console.Clear();
                VehicleType();
            }
        }
        public static void ParkCar()
        {
            DateTime now = DateTime.Now;
            int empty = EmptySpace();
            string spotRecipt = "";
            Console.Clear();
            Console.WriteLine("\t*** Park a Car ***\n");
            Console.Write("Enter your License plate number:");
            string carReg = Console.ReadLine().ToUpper();
            if (SearchReg(carReg))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Vehicle {carReg} is already parked here try again...\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }

            else if (carReg.Length >= 4 && carReg.Length <= 10)
            {
                for (int i = 0; i < ParkingList.Length; i++)
                {
                    if (ParkingList[i] == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Parking vehicle {carReg} at parking space {empty}\nParking started at {now}\n");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        spotRecipt = "CAR#" + carReg;
                        SpotAllocation(empty, spotRecipt);
                        Console.Clear();
                        break;
                    }
                    else if (ParkingList[i] != null)
                    {
                        continue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The parkinglot is full Please come back later.\nPress any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        MainMenu();
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The Entered license plate number is to short or to long.\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }
            for (int i = 0; i < Tickets.Length; i++)
            {
                if (Tickets[i] == null)
                {
                    Tickets[i] = carReg + " " + now;
                    break;
                }

            }
            MainMenu();
        }
        public static void McPark()
        {
            string mcReg = "";
            DateTime now = DateTime.Now;
            int empty = EmptySpace();
            string recipt = "";
            string spotRecipt = "";

            Console.Clear();
            Console.WriteLine("\t*** Park a MC ***\n");
            Console.Write("Enter your License plate number:");
            mcReg = Console.ReadLine().ToUpper();
            Console.Clear();
            for (int j = 0; j < ParkingList.Length; j++)
            {
                if (ParkingList[j] != null && ParkingList.Contains("MC#" + mcReg) || ParkingList.Contains("CAR#" + mcReg))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("This registration number is already parked");
                    Console.WriteLine("Press any key for go back to the vehicle menu:");
                    Console.ReadKey();
                    MainMenu();
                    break;
                }
            }
            if (mcReg.Length >= 4 && mcReg.Length <= 10)
            {
                for (int i = 0; i < ParkingList.Length; i++)
                {
                    if (ParkingList[i] != null)
                    {
                        if (ParkingList[i].Contains("/"))
                        {
                            continue;
                        }
                        if (ParkingList[i].Contains("MC#"))
                        {
                            string temp;
                            string sep = "/MC#";
                            temp = string.Join(sep, ParkingList[i], mcReg);
                            ParkingList[i] = temp;
                            Console.ForegroundColor = ConsoleColor.Green;
                            recipt = $"Parking vehicle {mcReg} at parking space {i + 1 }\nParking started at {now}";
                            Console.WriteLine("{0}\nPRess any key to continue...", recipt);
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                    }
                    if (ParkingList[i] == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        recipt = $"Parking vehicle {mcReg} at parking space {empty}\nParking started at {now}";
                        Console.WriteLine("{0} \nPress any key to continue...", recipt);
                        Console.ReadKey();
                        spotRecipt = "MC#" + mcReg;
                        SpotAllocation(empty, spotRecipt);
                        Console.Clear();
                        break;
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("The registration number is to long or to short. Max length is 10 letters and a min of 4 letters.\nPress any key to try again...");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.White;
                McPark();
            }
            for (int i = 0; i < Tickets.Length; i++)
            {
                if (Tickets[i] == null)
                {
                    Tickets[i] = mcReg + " " + now;
                    break;
                }
            }
            MainMenu();
        }
        public static void SeeParkedVehicles()
        {
            const int cols = 6;
            int n = 1;
            Console.Clear();
            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (n >= cols && n % cols == 0)
                {
                    Console.WriteLine();
                    n = 1;
                }
                if (ParkingList[i] == null)
                {
                    Console.Write(i + 1 + ": Empty \t");
                    n++;
                }
                else
                {
                    Console.Write(i + 1 + ": " + ParkingList[i] + "\t");
                    n++;
                }
            }
            Console.Write("\nPress any key to retrun to the menu...");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        public static void Move()
        {
            Console.Clear();
            Console.WriteLine("\t*** Moving a vehicle ***\n");
            Console.WriteLine("What would you like to move?\n" +
                "[1] Car\n" +
                "[2] MC\n" +
                "[3] Main menu");
            int userInput = int.Parse(Console.ReadLine());
            switch (userInput)
            {
                case 1:
                    MoveCar();
                    break;
                case 2:
                    MoveMC();
                    break;
                case 3:
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("choose an number from 1 to 3");
                    break;
            }
        }
        public static void RemoveVehicle()
        {
            Console.Clear();
            Console.WriteLine("\t*** Removing a vehicle ***\n");
            Console.Write("Enter license plate number:");
            string userReg = Console.ReadLine().ToUpper();
            DateTime now = DateTime.Now;

            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (ParkingList[i] == null)
                {
                    continue;
                }
                else if (ParkingList[i] == "CAR#" + userReg || ParkingList[i] == "MC#" + userReg)
                {
                    ParkingList[i] = null;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Removing vehicle {0}. Thanks for using us and welcome back!\nParking ended at {1}", userReg, now);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
                    break;
                }
                else if (ParkingList[i].Contains("/") && ParkingList[i].Contains("MC#" + userReg))
                {
                    string[] parkingSpot = ParkingList[i].Split("/");
                    foreach (var vehicle in parkingSpot)
                    {
                        if (parkingSpot[0] == "MC#" + userReg)
                        {
                            parkingSpot[0] = null;
                            ParkingList[i] = parkingSpot[1];
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Clear();
                            Console.WriteLine("Removing vehicle {0}. Thanks for using us and welcome back!\nParking ended at {1}", userReg, now);
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();
                            break;
                        }
                        else if (parkingSpot[1] == "MC#" + userReg)
                        {
                            parkingSpot[1] = null;
                            ParkingList[i] = parkingSpot[0];
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Clear();
                            Console.WriteLine("Removing Car {0}. Thanks for using us and welcome back!\nParking ended at {1}", userReg, now);
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();
                            break;
                        }
                    }
                }
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Vehicle {0} is not parked here. Please try again", userReg);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            MainMenu();
        }
        public static void Search()
        {
            Console.Clear();
            Console.WriteLine("\t*** Parking lot ***\n");
            Console.WriteLine("Enter licens plate number:");
            string userReg = Console.ReadLine().ToUpper();
            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (ParkingList[i] != null)
                {
                    if (ParkingList[i].Contains("MC#" + userReg) || ParkingList[i].Contains("CAR#" + userReg))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Your vehicle {userReg} is parked at parking space:{i + 1}\nPress any key to continue...");
                        Console.ReadLine();
                        MainMenu();
                        break;
                    }
                }
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Vehicle {userReg} is not parked here.\nPress any hey to continue..");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        public static int EmptySpace()
        {
            int counter = 0;
            int emptySpot = 0;
            foreach (string parkingSpot in ParkingList)
            {
                counter = counter + 1;
                if (parkingSpot == null)
                {
                    emptySpot = counter;
                    return emptySpot;
                }
                else if (counter == 100)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unfortunatly there is no parkingspots available.\n press any key to continue...");
                    Console.Read();
                    Console.Clear();
                    MainMenu();
                }
            }
            emptySpot = counter;
            return emptySpot;
        }
        public static void SpotAllocation(int spot, string vehicle)
        {
            spot = spot - 1;
            ParkingList[spot] = vehicle;
        }// lägg till datum och tid 
        public static void MoveCar()
        {
            Console.Clear();
            Console.WriteLine("\t*** Moving a Car ***\n");
            Console.Write("Enter license plate plate number:");
            string userReg = Console.ReadLine().ToUpper();

            if (SearchReg(userReg))
            {
                int index = FindIndex(userReg);
                if (ParkingList[index].Contains("CAR#"))
                {


                    for (int i = 0; i < ParkingList.Length; i++)
                    {
                        Console.WriteLine("Enter the spot you want to change to:");
                        int newSpot = int.Parse(Console.ReadLine());
                        if (ParkingList[newSpot - 1] == ParkingList[i])
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Your vehicle is already parked here.\nPress any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();
                        }
                        else if (ParkingList[newSpot - 1] == null)
                        {
                            ParkingList[newSpot - 1] = "CAR#" + userReg;

                            ParkingList[index] = null;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Clear();
                            Console.WriteLine($"Moved vehicle {userReg} too parking spot {newSpot}\nPress any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();
                            break;
                        }
                        else if (ParkingList[newSpot - 1].Contains("MC#") || ParkingList[newSpot - 1].Contains("CAR#"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This parking spot is allocated, try another parking space");
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("This license plate number is an MC. You are trying to move a car.\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle {0} is not parked here..\nPress any key to continue...", userReg);
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }

        }
        public static void MoveMC()
        {
            DateTime now = DateTime.Now;
            Console.Clear();
            Console.WriteLine("\t*** Moving MC ***\n");
            Console.Write("Enter license plate plate number:");
            string userReg = Console.ReadLine().ToUpper();

            if (SearchReg(userReg))
            {
                int index = FindIndex(userReg);
                if (ParkingList[index].Contains("MC#"))
                {
                    for (int i = 0; i < ParkingList.Length; i++)
                    {
                        Console.Write("Enter new parkingSpot:");
                        int newSpot = int.Parse(Console.ReadLine());
                        if (ParkingList[index].Contains("/"))
                        {
                            string[] vehicles = ParkingList[index].Split("/");
                            foreach (var vehicle in vehicles)
                            {
                                if (vehicles[0] == "MC#" + userReg)
                                {
                                    ParkingList[newSpot - 1] = vehicles[0];
                                    ParkingList[index] = vehicles[1];
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Moving vehicles {0} to parking spot {1}\nPress any key to continue...", userReg, newSpot);
                                    Console.ReadKey();
                                    Console.Clear();
                                    MainMenu();
                                    break;
                                }
                                if (vehicles[1] == "MC#" + userReg)
                                {
                                    ParkingList[newSpot - 1] = vehicles[1];
                                    ParkingList[index] = vehicles[0];
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Moving vehicles {0} to parking spot {1}\nPress any key to continue...", userReg, newSpot);
                                    Console.ReadKey();
                                    Console.Clear();
                                    MainMenu();
                                    break;

                                }
                            }
                        }
                        else if (ParkingList[newSpot - 1] == null)
                        {
                            ParkingList[newSpot - 1] = "MC#" + userReg;
                            ParkingList[index] = null;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Moving vehicles {0} to parking spot {1}\nPress any key to continue...", userReg, newSpot);
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();
                            break;
                        }
                        else if (ParkingList[newSpot - 1].Contains("CAR#"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This spot is allocated to a car, please choose another spot\nPress any key to continue back...");
                            Console.ReadKey();
                            Console.Clear();
                            MoveMC();
                        }
                        else if (ParkingList[newSpot - 1] == ParkingList[i])
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Your vehicle is already parked here.\nPress any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();

                        }
                        else if (ParkingList[newSpot - 1].Contains("MC#"))
                        {
                            string temp;
                            string seperator = "/MC#";
                            temp = string.Join(seperator, ParkingList[newSpot - 1], userReg);
                            ParkingList[newSpot - 1] = temp;
                            ParkingList[i] = null;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Clear();
                            Console.WriteLine($"Moved vehicle {userReg} too parking spot {newSpot}\nPress any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();
                            break;
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("This license plate number is for car. You are trying to move a Mc.\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
                }
            }
        }
        public static bool SearchReg(string regnum)
        {
            regnum.ToUpper();

            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (ParkingList[i] != null && ParkingList[i].Contains(regnum))
                {
                    return true;
                }
            }
            return false;
        }
        public static int FindIndex(string userReg)

        {
            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (ParkingList[i] != null && ParkingList[i].Contains(userReg))
                {
                    int index = i;
                    return index;
                }
            }
            return 0;
        }
        public static void Ticket()
        {
            int count = 1;
            foreach (var ticket in Tickets)
            {
                if (ticket == null)
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("{0}:{1}", count, ticket);
                    count++;
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
            MainMenu();




        }
    }
}








