using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleTables;
using FlashCardLearning.Controller;
using FlashCardLearning.Model.entities;
using Microsoft.IdentityModel.Tokens;

namespace FlashCardLearning.View
{
    internal class StackMenu
    {
        StackController stackController = new();
        FlashCardMenu flashCardMenu = new();
        MainMenu mainMenu = new();

        internal void ManageStacks()
        {
            Console.Clear();
            Console.WriteLine("-----------Manage Stack Menu-----------");
            Console.WriteLine();
            Console.WriteLine("Enter 1 to Create a Stack");
            Console.WriteLine("Enter 2 to Edit a Stack");
            Console.WriteLine("Enter 3 to Delete a Stack");
            Console.WriteLine("Enter 4 to View Available Stacks");
            Console.WriteLine("Enter 5 to View Flash Cards Inside the Stack");
            Console.WriteLine("Enter 6 to Return to Main Menu");

            var userInput = Console.ReadLine();

            string[] allowedUserInput = { "1", "2", "3", "4", "5", "6" };

            while (!allowedUserInput.Contains(userInput))
            {
                Console.WriteLine("Please Enter a valid menu number");
                userInput = Console.ReadLine();
            }
            bool fromMenu = true;

            while (true)
            {
                switch (userInput)
                {
                    case "1":
                        CreateStack();
                        break;
                    case "2":
                        EditStack();
                        break;
                    case "3":
                        RemoveStack();
                        break;
                    case "4":
                        ViewStack(fromMenu);
                        break;
                    case "5":
                        flashCardMenu.SelectStack();
                        break;
                    case "6":
                        mainMenu.UserMainMenue();
                        break;
                }
            }
        }

        private void CreateStack()
        {
            Console.Clear();
            Console.WriteLine("Please Enter the Stack Name, To Return to Stack Menu Enter 0");

            string stackName = Console.ReadLine();

            while (string.IsNullOrEmpty(stackName) || stackName.Length < 2 || !Regex.IsMatch(stackName, @"^[a-zA-Z #\-/]+$"))
            {
                if (stackName == "0")
                {
                    ManageStacks();
                    return;
                }

                Console.WriteLine("Please Enter a Proper Stack Name");
                stackName = Console.ReadLine();
            }
            stackController.CreateStack(stackName);
        }

        private void EditStack()
        {
            Console.Clear();

            //display available stacks
            ViewStack(false);
            Console.WriteLine("Please Enter a valid Stack Id, To Return to Stack Menu Enter 0");
            int StackId = ValidateStackId();

            while (!stackController.CheckStackAvailable(StackId))
            {
                Console.WriteLine();
                Console.WriteLine("Stack Id was not available ");
                Console.WriteLine();
                StackId = ValidateStackId();
            }

            //obtain stack name
            Console.WriteLine();
            Console.WriteLine("Please Enter a Stack Name to Update,  To Return to Stack Menu Enter 0");
            string stackName = Console.ReadLine();

            while (string.IsNullOrEmpty(stackName) || stackName.Length < 2 || !Regex.IsMatch(stackName, @"^(?!\d+$)[a-zA-Z0-9 #\-/]+$"))
            {
                if (stackName == "0")
                {
                    ManageStacks();
                    return;
                }

                Console.WriteLine("Please Enter a Proper Stack Name to Update");
                stackName = Console.ReadLine();

            }

            stackController.EditStack(StackId, stackName);
        }

        public int ValidateStackId()
        {

            string userInput = Console.ReadLine();

            int StackId = 0;

            bool isInteger = int.TryParse(userInput, out StackId);


            while (StackId < 0 || !isInteger)
            {
                Console.WriteLine("Please Enter a valid Stack Id, To Return to Stack Menu Enter 0");
                userInput = Console.ReadLine();

                isInteger = int.TryParse(userInput, out StackId);
            }

            if (StackId == 0)
            {
                ManageStacks();
                return 0;
            }

            return StackId;
        }

        public void ViewStack(bool fromMenu)
        {

            Console.WriteLine("Below are the available stacks");

            List<Stacks> stackList = stackController.ViewStack();

            var stackTable = new ConsoleTable("Id", "Stack Name");

            foreach (Stacks stack in stackList)
            {
                stackTable.AddRow(stack.Id, stack.StackName);
            }

            stackTable.Write();

            Console.WriteLine();

            //obtain ID
            if (fromMenu)
            {
                Console.WriteLine("To Return to Stack Menu Enter 0");
                string unserInput = Console.ReadLine();

                if (unserInput == "0")
                {
                    ManageStacks();
                    return;
                }
            }
        }

        private void RemoveStack()
        {
            Console.Clear();
            //display available stacks
            ViewStack(false);

            //obtain ID
            Console.WriteLine("Please note that by deleting a stack the related flash cards will also get deleted!!!");
            Console.WriteLine();
            Console.WriteLine("Please Enter a valid Stack Id, To Return to Stack Menu Enter 0");
            int StackId = ValidateStackId();


            while (!stackController.CheckStackAvailable(StackId))
            {
                StackId = ValidateStackId();
            }

            stackController.RemoveStack(StackId);

        }

    }
}
