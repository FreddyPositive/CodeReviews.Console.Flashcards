using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardLearning.View
{
    internal class MainMenu
    {
        internal void UserMainMenue()
        {
            StackMenu stackMenu = new();
            StudyMenu studyMenu = new();

            Console.Clear();
            Console.WriteLine("-----------Welcome to Flash Card Learning App-----------");
            Console.WriteLine("Enter 1 to Manage Stacks and Flash Cards");
            Console.WriteLine("Enter 2 to Start a Learning Session");
            Console.WriteLine("Enter 3 View Study Session Reports");
            Console.WriteLine("Enter 0 to Quit");
            Console.WriteLine();
            var userInput = Console.ReadLine();
            bool runApp = true;

            string[] allowedUserInput = { "1", "2", "3", "0" };

            while (!allowedUserInput.Contains(userInput))
            {
                Console.WriteLine("Please Enter a valid menu number");
                userInput = Console.ReadLine();
            }

            while (runApp)
            {
                switch (userInput)
                {
                    case "1":
                        stackMenu.ManageStacks();
                        break;
                    case "2":
                        studyMenu.StudySession();
                        break;
                    case "3":
                        studyMenu.ViewReports();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                }
            }

        }
    }
}
