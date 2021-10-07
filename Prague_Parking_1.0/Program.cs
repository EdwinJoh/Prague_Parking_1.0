using System;
using System.Linq;


namespace PragueParking
{
    class Program
    {
        public static string[] ParkingList = new string[100];


        static void Main(string[] args)
        {
            int menu;
            do
            {
                menu = MainMenu();

            } while (menu != 9);
        }
        public static int MainMenu() // Menu selection
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Prague Parking\n" +
                 "Enter choise below\n" +
                "[1] Add vehicle\n" +
                "[2] See parked vehicles\n" +
                "[3] Move Vehicle\n" +
                "[4] Remove vehicle\n" +
                "[5] Search for vehicle\n" +
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
                    case 9:
                        Console.WriteLine("Program quitting...");

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
                Console.WriteLine("Wrong input. Try again with an integer {0}", e.Message);
                MainMenu();
            }
            return 9;
        }
        public static void VehicleType() // Vehicle selection. Car or Mc
        {
            Console.Clear();
            Console.WriteLine("\t*** Park Vehicle ***");
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
                Console.WriteLine("Wrong input. Try an number from 1 - 3. press any key to go back...");
                Console.ReadKey();
                Console.Clear();
                VehicleType();
            }
        }
        public static void ParkCar() // Parking a Car
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
                Console.WriteLine("Vehicle {0} is already parked here", carReg);
                standardText();
            }
            if (carReg.Length >= 4 && carReg.Length <= 10)
            {
                for (int i = 0; i < ParkingList.Length; i++)
                {
                    if (ParkingList[i] == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        spotRecipt = "CAR#" + carReg;
                        SpotAllocation(empty, spotRecipt);
                        Console.WriteLine($"Parking vehicle {carReg} at parking space {empty}\nParking started at {now}\n");
                        standardText();

                    }
                    else if (ParkingList[i] != null)
                    {
                        continue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The parkinglot is full Please come back later.");
                        standardText();
                    }
                }
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The Entered license plate number is to short or to long.\nPress any key to continue...");
                standardText();
            }
        }
        public static void McPark() // Parking a MC
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle is already parked here.\nPress any key to continue...");
                standardText();
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
                            standardText();
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
                                    recipt = $"Parking vehicle {mcReg} at parking space {i + 1}\nParking started at {now}";
                                    Console.WriteLine("{0}\n", recipt);
                                    standardText();
                                }
                            }
                            if (ParkingList[i] == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;

                                spotRecipt = "MC#" + mcReg;
                                SpotAllocation(empty, spotRecipt);
                                recipt = $"Parking vehicle {mcReg} at parking space {empty}\nParking started at {now}";
                                Console.WriteLine("{0} \n", recipt);
                                standardText();
                            }
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("The registration number is to long or to short. Max length is 10 letters and a min of 4 letters.");
                    standardText();

                }
            }
            MainMenu();
        }
        public static void SeeParkedVehicles() // List of all the vehicle that are parked
        {
            Console.Clear();
            Console.WriteLine("Prague Parking");
            const int cols = 6;
            int n = 1;
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
            standardText();
        }
        public static void Move() // Move selection Car or MC
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
                    standardText();
                    break;
            }
        }
        public static void RemoveVehicle() // Remove a vehicle
        {
            Console.Clear();
            Console.WriteLine("\t*** Removing a vehicle ***\n");
            Console.Write("Enter license plate number:");
            string userReg = Console.ReadLine().ToUpper();
            DateTime now = DateTime.Now;
            Console.Clear();
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
                    standardText();
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
                            Console.WriteLine("Removing vehicle {0}. Thanks for using us and welcome back!\nParking ended at {1}", userReg, now);
                            standardText();
                        }
                        else if (parkingSpot[1] == "MC#" + userReg)
                        {
                            parkingSpot[1] = null;
                            ParkingList[i] = parkingSpot[0];
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Clear();
                            Console.WriteLine("Removing Car {0}. Thanks for using us and welcome back!\nParking ended at {1}", userReg, now);
                            standardText();
                        }
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Vehicle {0} is not parked here. Please try again", userReg);
            standardText();
        }
        public static void Search()// Search for an vehicle that are parked
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
                Console.WriteLine($"Your vehicle {userReg} is parked at parking space:{index + 1}\n");
                standardText();
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Vehicle {userReg} is not parked here.\nPress any hey to continue..");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        public static int EmptySpace() // looks for empty space 
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
                    Console.WriteLine("Unfortunately there is no parkingspots available.\n");
                    standardText();
                }
            }
            emptySpot = counter;
            return emptySpot;
        }
        public static void SpotAllocation(int spot, string vehicle) // allocate an parkingspace for the vehicles
        {
            spot = spot - 1;
            ParkingList[spot] = vehicle;
        }
        public static void MoveCar() // Moving Car
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
                    Console.WriteLine("we only have parking spot up to 100\n");
                    standardText();
                }
                if (ParkingList[newSpot - 1] == null)
                {
                    ParkingList[newSpot - 1] = "CAR#" + userReg;
                    ParkingList[index] = null;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Moveing vehicle {0} to new parking spot {1}", userReg, newSpot);
                    standardText();
                }
                else if (ParkingList[newSpot - 1] != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is not enought room. This spot is allocated by a vehicle \n");
                    standardText();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not find a vehicle with licens plate: {0}\n", userReg);
                standardText();
            }
        }
        public static void MoveMC() // Moving MC
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
                    Console.WriteLine("we only have parking spot up to 100\n");
                    standardText();
                }
                else if (ParkingList[index] == ParkingList[newSpot - 1])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your vehicle is already parked here\n");
                    standardText();
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
                    Console.WriteLine("there is already 2 mc parked at this spot, please try again with another spot.\n");
                    standardText();
                }
                else if (ParkingList[newSpot - 1].Contains("CAR#"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is not enought room. There is an Car parked here.\n");
                    standardText();
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
                Console.WriteLine("Moving vehicle {0} to new spot {1}\n", userReg, newSpot);
                standardText();
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Could not find a vehicle: {0}\n", userReg);
            standardText();
        }
        public static bool SearchReg(string userReg) // Looking for licensPlate number
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
        public static int FindIndex(string userReg) // Finding index of an parked vehicle
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
        public static bool SearchVehicle(string userInput) // Searching for a vehicle for the search metod
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
        static void standardText()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
    }
}









