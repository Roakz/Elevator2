using System;
using System.Collections.Generic;
using System.Configuration;   
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ElevatorChallenge
{
    class Program
    {
        class MenuItem
        {
            public int Index { get; }
            public string Name { get; }

            public MenuItem(int index, string name)
            {
                Index = index;
                Name = name;
            }

            public override string ToString()
            {
                return
                    $"Index = {Index}" +
                    $"\nName = {Name}";
            }

            public void PerformAction(int menuItemIndex, Menu mainMenu, JobAllocator jobAllocator, Lift[] liftArray) {
                switch (Name)
                {
                    case "Instructions":
                        PrintInstructions(mainMenu, jobAllocator);
                        break;
                    case "Exit":
                        break;

                    default:
                        List<string> travellerData = PrintUserCreationMenu();
                        jobAllocator.jobList.Add(new Job(travellerData[0], Convert.ToInt32(travellerData[1]), Convert.ToInt32(travellerData[2])));
                        jobAllocator.Computate(liftArray);
                        mainMenu.PrintMenu(jobAllocator);
                        break;
                }
            }

            private void PrintInstructions(Menu mainMenu, JobAllocator jobAllocator)
            {
                Console.Clear();
                Console.WriteLine("Welcome to elevator MADNESS!!\n" +
                    "Select up or down from the menu. Enter the requested details and enjoy your ride.\n" +
                    "you will recieve updates as we go and a file output at the end of the game. \n" +
                    "Add as many trips as you like. There are 3 lifts and each lift can hold 6 people.\n" +
                    "See if you can fill them up!\n" +
                    "Press any key to exit.\n\n\n\n"); // File output location?
                Console.ReadLine();
                Console.Clear();
                mainMenu.PrintMenu(jobAllocator);
            }

            private List<string> PrintUserCreationMenu()
            {
                List<string> answers = new List<string>();
                Console.WriteLine("Travellers Name?");
                answers.Add(Console.ReadLine());
                Console.WriteLine("Current Floor?");
                answers.Add(Console.ReadLine());
                Console.WriteLine("Desired Floor?");
                answers.Add(Console.ReadLine());
                return answers;
            }
        }

        class JobAllocator
        {
            public Lift[] AllocatorLiftArray;
            public List<Job> jobList { get; }

            public JobAllocator(Lift[] liftArray)
            {
                jobList = new List<Job>();
                AllocatorLiftArray = liftArray;
                //Temporary Testing Job
                //jobList.Add(new Job("Rory", 1, 3));
            }

            public void Computate(Lift[] liftArray) {

                //Select the newly appened job
                Job newJob = jobList[(jobList.Count - 1)];
                //Where is the desired pickup?
                int newJobPickupFloor = newJob.Location;

                //BELLLOW REQUIRED HANDLING IF THERE ARE NO LIFTS RETURNED

                //query to return lifts give or take 2 floors from jobs pickup location.
                IEnumerable<Lift> LiftInZoneQuery = from x in liftArray
                            where Enumerable.Range(newJob.Location - 2, newJob.Location + 2).Contains(x.CurrentLocation)
                            select x;
                //Is the passenger travelling up.
                bool goingUp = newJob.Desiredlocation > newJob.Location;
                IEnumerable<Lift> liftSelectionQuery;
                //query to select lifts from above or below them and within the zone depending on if they are travelling up on down.
                if(goingUp == true)
                {
                    liftSelectionQuery = from lift in LiftInZoneQuery
                                         where lift.CurrentLocation < newJob.Location
                                         select lift;
                }else
                {
                    liftSelectionQuery = from lift in LiftInZoneQuery
                                         where lift.CurrentLocation > newJob.Location
                                         select lift;
                }
                //Execute liftSelection query. If there are available lifts then pick whichever one will make it the quickest. Otherwise
                //If no lifts are in the zone are there any stationary lifts? if yes are they closer than a moving lift? if yes deploy stationary lift.
                // If no and moving lift is heading in the right direction. Then is it going to be quicker with stop times. if yes add to list if no deploy stationary
            }
         }

        class Menu
        {
            private List<MenuItem> menuItemList;
            public Lift[] liftArrayRef;
            public Menu(List<MenuItem> menuItems, Lift[] liftArray)
            {
                menuItemList = menuItems;
                liftArrayRef = liftArray;
            }
            public void PrintMenu(JobAllocator jobAllocator)
            {
                Console.WriteLine("Please make a selection");
                foreach (MenuItem menuItem in menuItemList)
                {
                   Console.WriteLine($"{menuItem.Index} {menuItem.Name}");                    
                }
                ProcessUserInput(jobAllocator);
            }

            public void ProcessUserInput(JobAllocator jobAllocator)
            {
                int userSelection;
                try
                {
                    userSelection = Convert.ToInt32(Console.ReadLine());
                } catch
                {
                    Console.Clear();
                    Console.WriteLine("Please select 1-4 only.");
                    PrintMenu(jobAllocator);
                    return;
                }
                bool valid = Enumerable.Range(1, menuItemList.Count).Contains(userSelection);
                if (valid == true) {
                    menuItemList[userSelection - 1].PerformAction(userSelection, this, jobAllocator, liftArrayRef);
                    if (userSelection == 4) {
                        Environment.Exit(0);
                            } else { 
                        PrintMenu(jobAllocator);
                    }
                } else
                {
                    Console.Clear();
                    Console.WriteLine("Please select 1-4 only.");
                    PrintMenu(jobAllocator);
                }                    
            }
         }

        class Job
        {
            public string Name { get; }
            public int Location { get; set; }
            public int Desiredlocation { get; }

            Stopwatch stopWatch = new Stopwatch();


            public Job(string name, int initialLocation, int desiredLocation)
            {
                Name = name;
                Location = initialLocation;
                Desiredlocation = desiredLocation;
                stopWatch.Start();
            }
        }

        class Building
        {
            public int MetresBetweenFloors { get; }
            public int Levels { get; }

            public Building()
            {
                MetresBetweenFloors = 6;
                Levels = 6;
            }
        }

        class Lift
        {
            public int MetresPerSecond { get; }
            public int secondsElapsedDoorOpeningClosing { get; }
            public List<int> priorities = new List<int>();
            public int LiftNumber { get; }
            public int CurrentLocation { get; set; }
            public string TravelStatus { get; set; }

            public Lift(int liftNumber)
            {
                LiftNumber = liftNumber;
                secondsElapsedDoorOpeningClosing = 6;
                MetresPerSecond = 1;
                CurrentLocation = 1;
                //Test code
                TravelStatus = "UP";
            }

            public int returnLocation()
            {
                // Sone code to return where the lift currently is
                return 0;
            }
        }

        static void Main(string[] args)
        {
            string[] menuItemInputs = { "Up", "Down", "Instructions", "Exit" };
            List<MenuItem> menuItemsList = new List<MenuItem> { };

            for (int i = 0; i < menuItemInputs.Length; i++)
            {
                menuItemsList.Add(new MenuItem(i + 1, menuItemInputs[i]));
            }
            Building building = new Building();
            Lift[] liftArray = new Lift[3];
            liftArray[0] = new Lift(1);
            liftArray[1] = new Lift(2);
            liftArray[2] = new Lift(3);
            JobAllocator jobAllocator = new JobAllocator(liftArray);
            Menu mainMenu = new Menu(menuItemsList, liftArray);
            mainMenu.PrintMenu(jobAllocator);           
        }
    }
}
