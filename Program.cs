using System;
using System.Collections.Generic;
using System.Configuration;   
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            public void PerformAction(int menuItemIndex, Menu mainMenu, JobAllocator jobAllocator) {
                if(Name == "Instructions")
                {
                    PrintInstructions(mainMenu, jobAllocator);
                }
                if(Name == "Exit")
                {
                    return;
                }
                if(Name == "Up" || Name == "Down")
                {
                   List<string> travellerData = PrintUserCreationMenu();
                    jobAllocator.jobList.Add(new Job(travellerData[0], Convert.ToInt32(travellerData[1]), Convert.ToInt32(travellerData[2])));
                }
            }

            private void PrintInstructions(Menu mainMenu, JobAllocator jobAllocator)
            {
                Console.WriteLine("Welcome to elevator MADNESS!!\n" +
                    "Select up or down from the menu. Enter the requested details and enjoy your ride.\n" +
                    "you will recieve updates as we go and a file output at the end of the game. \n" +
                    "Add as many trips as you like. There are 3 lifts and each lift can hold 6 people.\n" +
                    "See if you can fill them up!\n" +
                    "Press any key to exit."); // File output location?

                Console.Read();
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
            public List<Job> jobList { get; }
         }


        class Menu
        {
            private List<MenuItem> menuItemList;
            public Menu(List<MenuItem> menuItems)
            {
                menuItemList = menuItems;
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
                    menuItemList[userSelection - 1].PerformAction(userSelection, this, jobAllocator);
                    if (userSelection == 4) {
                        return;
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
            string Name { get; }
            int Location { get; set; }
            int Desiredlocation { get; }
         
            public Job(string name, int initialLocation, int desiredLocation)
            {
                this.Name = name;
                this.Location = initialLocation;
                this.Desiredlocation = desiredLocation;
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

            JobAllocator jobAllocator = new JobAllocator();
            Menu mainMenu = new Menu(menuItemsList);
            mainMenu.PrintMenu(jobAllocator);           
        }
    }
}
