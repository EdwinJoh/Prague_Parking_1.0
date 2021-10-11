using System;
using System.Linq;


namespace PragueParking
{
    class Program
    {
        public static string[] ParkingList = new string[100];


        static void Main(string[] args)
        {
            int menuInput;
            do
            {
                menuInput = MainMenu();

            } while (menuInput != 6);
        }
        public static int MainMenu()
        {
            Console.Clear();
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
                "[6] Quit Program\n");

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
                        Console.WriteLine("Program quitting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("The number is not in the menu. Enter an number from 1-6");
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
            return 6;

        }
        static void VehicleType()
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid number. Try again.");
                StandardText();
            }
        }
        static void AddCar()
        {
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
                            Console.WriteLine("Parked vehicle {0} at parking spot {1}", userInput, i + 1);
                            StandardText();
                        }
                        else if (ParkingList[i] != null)
                        {
                            continue;
                        }
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The parking lot is full");
                    StandardText();

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input, either the registration number is fo short or to long\n" +
                    "minimum characters 4 and max characters is 10.");
                    StandardText();
                }
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle is already parked here");
                StandardText();
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
        }
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
            StandardText();
        }
        static void StandardText()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
            MainMenu();

        }
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
        }
        static void RemoveVehicle()
        {
            Console.Clear();
            Console.WriteLine("PRague parking\tRemoving vehicle");
            Console.Write("Enter Registration number:");
            string userInput = Console.ReadLine().ToUpper();
            int index = FindIndex(userInput, out index);
            if (SearchRegCar(userInput))
            {

                if (ParkingList[index] == "CAR#" + userInput)
                {
                    ParkingList[index] = null;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Removing {0} Thanks for parking here.", userInput);
                    StandardText();
                }
            }
            if (SearchRegMC(userInput))
            {
                if (ParkingList[index] == "MC#" + userInput || ParkingList[index].Contains("/"))
                {
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

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Removing {0} Thanks for parking here.", userInput);
                StandardText();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle {0} is not parked here try again.", userInput);
                StandardText();
            }
        }
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
                Console.WriteLine("Car {0} is parked at parking space:{1}", userInput, index + 1);
            }
            else if (SearchRegMC(userInput))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                int index = FindIndex(userInput, out index);
                Console.WriteLine("MC {0} is parked at parking space:{1}", userInput, index + 1);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle:{0} is not parked here", userInput);
            }
            StandardText();
        }
        static void AddMc()
        {
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
                            ParkingList[i] = "MC#" + userInput;
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Parked vehicle {0} at parking spot:{1}", userInput, i + 1);
                            StandardText();
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
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Parked vehicle {0} at parking spot:{1}", userInput, i + 1);
                                StandardText();
                            }
                        }
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The parking lot is full");
                    StandardText();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input, either the registration number is fo short or to long\n" +
                    "minimum characters 4 and max characters is 10.");
                    StandardText();
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vehicle is already parked here");
                StandardText();
            }
        }
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
        }
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
                StandardText();
            }
        }
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
                    StandardText();
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Moving vehicle {0} to new spot {1}", userInput, newSpot);
                StandardText();
                MainMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The vehicle is either a MC or not parked here.");
                StandardText();
            }

        }
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
                    StandardText();
                }
                else
                {
                    if (ParkingList[newSpot - 1].Contains("/"))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This spot is allocated by two MC already. please try again.");
                        StandardText();
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
                StandardText();
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The vehicle is either a Car or not parked here.", userInput);
                StandardText();

            }
        }
    }
}


