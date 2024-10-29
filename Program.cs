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

            public void PerformAction(int menuItemIndex, Menu mainMenu) {
                if(Name == "Instructions")
                {
                    PrintInstructions(mainMenu);
                }
            }

            private void PrintInstructions(Menu mainMenu)
            {
                Console.WriteLine("Welcome to elevator MADNESS!!\n" +
                    "Select up or down from the menu. Enter the requested details and enjoy your ride.\n" +
                    "you will recieve updates as we go and a file output at the end of the game. \n" +
                    "Add as many trips as you like. There are 3 lifts and each lift can hold 6 people.\n" +
                    "See if you can fill them up!\n" +
                    "Press any key to exit."); // File output location?

                Console.Read();
                mainMenu.PrintMenu();
                
            }
        }

        class Menu
        {
            private List<MenuItem> menuItemList;
            public Menu(List<MenuItem> menuItems)
            {
                menuItemList = menuItems;
            }

            public void PrintMenu()
            {

                Console.WriteLine("Please make a selection");

                foreach (MenuItem menuItem in menuItemList)
                {
                   Console.WriteLine($"{menuItem.Index} {menuItem.Name}");                    
                }
                ProcessUserInput();
            }

            public void ProcessUserInput()
            {
                int userSelection;

                try
                {
                    userSelection = Convert.ToInt32(Console.ReadLine());
                } catch
                {
                    Console.Clear();
                    Console.WriteLine("Please select 1-4 only.");
                    PrintMenu();
                    return;
                }
                
                bool valid = Enumerable.Range(1, menuItemList.Count).Contains(userSelection);

                if (valid == true) {
                    menuItemList[userSelection - 1].PerformAction(userSelection, this);} else
                {
                    Console.Clear();
                    Console.WriteLine("Please select 1-4 only.");
                    PrintMenu();
                }                    
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

            Menu mainMenu = new Menu(menuItemsList);
            mainMenu.PrintMenu();
            Console.Clear();
            Console.Read();
           
        }
    }
}
