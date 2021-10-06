
using System;
using System.Linq;


namespace PragueParking
{
    class Program
    {
        public static string[] ParkingList = new string[100];
        public static string[] TicketList = new string[100];
        static string[] List = new string[200];


        static void Main(string[] args)
        {
            MainMenu();
        }
        public static void MainMenu()
        {
            try
            {

                TimeSpan time = DateTime.Now.TimeOfDay;

                if (time > new TimeSpan(23, 59, 00) || time < new TimeSpan(0, 00, 00))
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
                Console.WriteLine("\n" +
                 "Enter choise below\n" +
                "[1] Add vehicle\n" +
                "[2] See parked vehicles\n" +
                "[3] Move Vehicle\n" +
                "[4] Remove vehicle\n" +
                "[5] Search for vehicle\n" +
                "[6] Tickets\n" +
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
                Console.WriteLine("Wrong input, try again with an integer {0}\n{1}", e.Message, e.StackTrace);
                MainMenu();
            }
        }
        public static void VehicleType()
        {
            Console.Clear();
            Console.WriteLine("*** Park Vehicle ***");
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
            carReg = carReg.Replace(" ", "");
            Console.Clear();
            if (SearchReg(carReg))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle {0} is already parked here.\nPress any key to continue...", carReg);
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }
            if (carReg.Length >= 4 && carReg.Length <= 10)
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
                for (int i = 0; i < TicketList.Length; i++)
                {
                    if (TicketList[i] == null)
                    {
                        TicketList[i] = carReg + " " + now;
                        break;
                    }
                }
                MainMenu();

            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The Entered license plate number is to short or to long.\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }

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
            mcReg = mcReg.Replace(" ", "");
            Console.Clear();
            if (SearchReg(mcReg))
            {
                Console.WriteLine("Vehicle is already parked here.\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
                MainMenu();

            }
            else
            {
                if (mcReg.Length >= 4 && mcReg.Length <= 10)
                {
                    for (int j = 0; j < ParkingList.Length; j++)
                    {
                        if (ParkingList[j] != null && ParkingList[j].Contains("MC#" + mcReg))
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
                                    string seperator = "/MC#";
                                    temp = string.Join(seperator, ParkingList[i], mcReg);
                                    ParkingList[i] = temp;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    recipt = $"Parking vehicle {mcReg} at parking space {i + 1 }\nParking started at {now}";
                                    Console.WriteLine("{0}\nPress any key to continue...", recipt);
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
                        for (int i = 0; i < TicketList.Length; i++)
                        {
                            if (TicketList[i] == null)
                            {
                                TicketList[i] = mcReg + " " + now;
                                break;
                            }
                        }
                        MainMenu();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("The registration number is to long or to short. Max length is 10 letters and a min of 4 letters.\nPress any key to try again...");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    MainMenu();
                }
            }
            for (int j = 0; j < TicketList.Length; j++)
            {
                if (TicketList[j] == null)
                {
                    TicketList[j] = mcReg + " " + now;
                    break;
                }
            }
        }
        public static void SeeParkedVehicles()
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
            Console.Write("\nPress any key to retrun to the menu...");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        public static void Move()
        {
            Console.Clear();
            Console.WriteLine("\t*** Moving a vehicle ***\n");
            Console.WriteLine("What kind of vehicle would you like to move?\n" +
                "[1] Car\n" +
                "[2] MC\n" +
                "[3] Main menu");
            int userInput = int.Parse(Console.ReadLine());
            Console.Clear();
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
                    Console.WriteLine("choose an number from 1 to 3\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
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
            Console.Clear();

            for (int i = 0; i < ParkingList.Length; i++)
            {
                int ticketIndex = FindTicket(userReg);
                string ticketDate = TicketList[ticketIndex].Replace(userReg, "");
                DateTime checkInDate = DateTime.Parse(ticketDate);
                TimeSpan interval = new TimeSpan();
                interval = now - checkInDate;
                string output = null;

                if (ParkingList[i] == null)
                {
                    continue;
                }
                else if (ParkingList[i] == "CAR#" + userReg || ParkingList[i] == "MC#" + userReg)
                {
                    TicketList[ticketIndex] = null;
                    ParkingList[i] = null;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Removing vehicle {0}. Thanks for using us and welcome back!\nParking ended at {1}", userReg, now);
                    output = "Vehicle has been parked for: " + interval.ToString(@"dd\.hh\:mm\:ss");
                    Console.WriteLine(output);
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
                            TicketList[ticketIndex] = null;
                            parkingSpot[0] = null;
                            ParkingList[i] = parkingSpot[1];
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Clear();
                            Console.WriteLine("Removing vehicle {0}. Thanks for using us and welcome back!\nParking ended at {1}", userReg, now);
                            output = "Vehicle has been parked for: " + interval.ToString(@"dd\.hh\:mm\:ss");
                            Console.WriteLine(output);
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();
                            break;
                        }
                        else if (parkingSpot[1] == "MC#" + userReg)
                        {
                            TicketList[ticketIndex] = null;
                            parkingSpot[1] = null;
                            ParkingList[i] = parkingSpot[0];
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Clear();
                            Console.WriteLine("Removing vehicle {0}. Thanks for using us and welcome back!\nParking ended at {1}", userReg, now);
                            output = "Vehicle has been parked for: " + interval.ToString(@"dd\.hh\:mm\:ss");
                            Console.WriteLine(output);
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            MainMenu();
                            break;
                        }
                    }
                }
            }
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
            Console.Write("Enter licens plate number:");
            string userReg = Console.ReadLine().ToUpper();
            Console.Clear();
            if (SearchVehicle(userReg))
            {
                int index = FindIndex(userReg);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Your vehicle {userReg} is parked at parking space:{index + 1}\nPress any key to continue...");
                Console.ReadLine();
                MainMenu();
            }


            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Vehicle {userReg} is not parked here.\nPress any hey to continue..");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        public static int EmptySpace()
        {
            Console.Clear();
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
                    Console.WriteLine("Unfortunately there is no parkingspots available.\n press any key to continue...");
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
        }
        public static void MoveCar()
        {
            Console.Clear();
            Console.WriteLine("\t*** Moving a Car ***\n");
            Console.Write("Enter license plate plate number:");
            string userReg = Console.ReadLine().ToUpper();
            Console.Clear();
            if (SearchReg(userReg))
            {
                int index = FindIndex(userReg);
                Console.WriteLine("Enter the spot you want to change to:");
                int newSpot = int.Parse(Console.ReadLine());
                if (newSpot > 100)
                {
                    Console.WriteLine("we only have parking spot up to 100");
                    Console.Read();
                }
                if (ParkingList[newSpot - 1] == null)
                {
                    ParkingList[newSpot - 1] = "CAR#" + userReg;
                    ParkingList[index] = null;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Moveing vehicle {0} to new parking spot {1}", userReg, newSpot);
                    Console.ReadKey();
                    MainMenu();
                }

                else if (ParkingList[newSpot - 1] != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is not enought room. This spot is allocated by a vehicle \nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not find a vehicle with licens plate: {0}\nPress any key to continue...", userReg);
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
            Console.Clear();
            if (SearchReg(userReg))
            {

                int index = FindIndex(userReg);
                Console.Write("Enter the spot you want to change to:");
                int newSpot = int.Parse(Console.ReadLine());
                if (newSpot > 100)
                {
                    Console.WriteLine("we only have parking spot up to 100\nPress any key to continue...");
                    Console.Read();
                    Console.Clear();
                    MainMenu();
                }
                if (ParkingList[index] == ParkingList[newSpot - 1])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your vehicle is already parked here\nPress any key to continue..");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
                }
                else if (ParkingList[newSpot - 1] == null)
                {
                    if (ParkingList[index].Contains("/"))
                    {
                        string[] vehicles = ParkingList[index].Split("/");
                        foreach (var vehicle in vehicles)
                        {
                            if (vehicles[0] == "MC#" + userReg)
                            {
                                ParkingList[newSpot - 1] = "MC#" + userReg;
                                ParkingList[index] = vehicles[1];
                            }
                            else if (vehicles[1] == "MC#" + userReg)
                            {
                                ParkingList[newSpot - 1] = "MC#" + userReg;
                                ParkingList[index] = vehicles[0];
                            }
                        }
                    }
                    else
                    {
                        ParkingList[newSpot - 1] = "MC#" + userReg;
                        ParkingList[index] = null;
                    }

                }
                else if (ParkingList[newSpot - 1].Contains("/"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("there is already 2 mc parked at this spot, please try again with another spot.\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
                }
                else if (ParkingList[newSpot - 1].Contains("CAR#"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is not enought room. There is an Car parked here.\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
                }


                else if (ParkingList[newSpot - 1].Contains("MC#") && ParkingList[index] == "MC#" + userReg)
                {
                    string temp;
                    string seperatorerator = "/MC#";
                    temp = string.Join(seperatorerator, ParkingList[newSpot - 1], userReg);
                    ParkingList[newSpot - 1] = temp;
                    ParkingList[index] = null;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Moving vehicle {0} to new spot {1}\nPress any key to continue...", userReg, newSpot);
                Console.ReadKey();
                Console.Clear();

            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Could not find a vehicle with licens plate: {0}\nPress any key to continue...", userReg);
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        public static bool SearchReg(string userReg)
        {
            userReg.ToUpper();
            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (ParkingList[i] != null && ParkingList[i].Contains(userReg))
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
            foreach (var ticket in TicketList)
            {
                if (ticket == null)
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("{0},{1}", count, ticket);
                    count++;
                }
            }
            Console.ReadKey();
            MainMenu();
        }
        public static int FindTicket(string userReg)
        {
            for (int i = 0; i < TicketList.Length; i++)
            {
                if (TicketList[i] != null && TicketList[i].Contains(userReg))
                {
                    int index = i;
                    return index;
                }
            }
            return 0;
        }
        public static bool SearchVehicle(string userInput)
        {
            for (int i = 0; i < ParkingList.Length; i++)
            {
                if (ParkingList[i] != null && ParkingList[i].Contains(userInput))
                {
                    string[] vehicles = ParkingList[i].Split("/");

                    if (vehicles[0] == "MC#" + userInput || vehicles[0] == "CAR#" + userInput)
                    {
                        return true;
                    }
                    else if (vehicles[1] == "MC#" + userInput || vehicles[0] == "CAR#" + userInput)
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return false;
        }
        public static void MenuDesign()
        {
            Console.WriteLine("\n" +
 " 00000011                                                   000000ba                    11       oo                   \n" +
 " 00    `01                                                  00    `01                   00                            \n" +
 " 00110011' 00d0001. .100001. .100001. 11    11 .100001.     00110011' .100001. 00d0001. 00  .11  11 00d0001. .100001. \n" +
 " 00        00'  `00 00'  `00 00'  `00 00    00 00ooood8     00        00'  `00 00'  `00 00008    00 00'  `00 00'  `00 \n" +
 " 00        00       00.  .00 00.  .00 00.  .00 00.  ...     00        00.  .00 00       00  `01. 00 00    00 00.  .00 \n" +
 " 11        11       `00008P8 `0000P00 `00008P' `00008P'     11        `00008P8 11       11   `00 11 11    11 `0000P00 \n" +
 "oooooooooooooooooooooooooooooo~~~~.00~oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo~~~~.00~\n" +
 "                              d0000P                                                                          d0000P  \n");
        }

    }
}










