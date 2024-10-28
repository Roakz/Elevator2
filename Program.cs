using System;
using System.Collections.Generic;
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

            private void PerformAction() { }


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
                foreach (MenuItem menuItem in menuItemList)
                {
                    Console.WriteLine($"{menuItem.Index} {menuItem.Name}");
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
            Console.Read();
        }
    }
}
