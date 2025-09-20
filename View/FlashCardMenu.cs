using ConsoleTables;
using FlashCardLearning.Controller;
using FlashCardLearning.Model.entities;


namespace FlashCardLearning.View
{
    internal class FlashCardMenu
    {
        FlashCardController flashCardController = new();
        StackController stackController = new();

        int StackId;
        private void ManageFlashCard()
        {
            Console.Clear();

            ViewFlashCard();
            Console.WriteLine();
            Console.WriteLine("-----------Manage Flash Card Menu-----------");
            Console.WriteLine();
            Console.WriteLine("Enter 1 to create a FlashCard");
            Console.WriteLine("Enter 2 to Edit a FlashCard");
            Console.WriteLine("Enter 3 to Delete a FlashCard");
            Console.WriteLine("Enter 4 to Return to Stack Menu");
            Console.WriteLine();
            var userInput = Console.ReadLine();

            string[] allowedUserInput = { "1", "2", "3", "4" };

            while (!allowedUserInput.Contains(userInput))
            {
                Console.WriteLine("Please Enter a valid menu number");
                userInput = Console.ReadLine();
            }

            while (true)
            {
                switch (userInput)
                {
                    case "1":
                        CreateFlashCard();
                        break;
                    case "2":
                        EditFlashCard();
                        break;
                    case "3":
                        RemoveFlashCard();
                        break;
                    case "4":
                        SelectStack();
                        break;
                }
            }
        }

        internal void SelectStack()
        {
            StackMenu stackMenu = new();

            Console.Clear();

            ViewStack();

            Console.WriteLine("Please Enter the Stack Id to View the Flash Cards and Manage It , enter 0 to return to stack menu");
            StackId = stackMenu.ValidateStackId();

            Console.Clear();

            if (StackId != 0) ManageFlashCard();
            else stackMenu.ManageStacks();

        }

        private void ViewFlashCard()
        {
            Console.WriteLine("Below are the Available Flash Cards");
            Console.WriteLine();

            List<FlashCards> flashCards = flashCardController.ViewFlashCards(StackId);

            var flashCardTable = new ConsoleTable("Id", "Question", "Answer");

            foreach (FlashCards flashCard in flashCards)
            {
                flashCardTable.AddRow(flashCard.Id, flashCard.Question, flashCard.Answer);
            }
            flashCardTable.Write();

        }

        private void CreateFlashCard()
        {
            Console.Clear();
            Console.WriteLine("Please Enter the Question, Press 0 to Return to Main Menu ");

            string Question = Console.ReadLine();

            while (string.IsNullOrEmpty(Question) || Question.Length < 2)
            {
                if (Question == "0")
                {
                    ManageFlashCard();
                    return;
                }

                Console.WriteLine("Please Enter a Proper Question");
                Question = Console.ReadLine();
            }

            Console.WriteLine("Please Enter the Answer, Press 0 to Return to Main Menu ");

            string Answer = Console.ReadLine();

            while (string.IsNullOrEmpty(Answer) || Answer.Length < 2)
            {
                if (Answer == "0")
                {
                    ManageFlashCard();
                    return;
                }

                Console.WriteLine("Please Enter a Proper Answer");
                Answer = Console.ReadLine();
            }
            flashCardController.CreateFlashCard(Question, Answer, StackId);
        }

        private void EditFlashCard()
        {
            Console.Clear();

            ViewFlashCard();
            Console.WriteLine();

            int flashCardId = ValidateFlashCardId();
            Console.WriteLine();

            while (!flashCardController.CheckFlashCardAvailable(flashCardId))
            {
                flashCardId = ValidateFlashCardId();
            }

            Console.WriteLine("Please enter the Question to update, Press 0 to Return to Main Menu");
            string question = Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Please enter the Answer to update, Press 0 to Return to Main Menu");
            string answer = Console.ReadLine();

            while ((string.IsNullOrEmpty(question) || question.Length < 2) && (string.IsNullOrEmpty(answer) || answer.Length < 2))
            {
                if (question == "0")
                {
                    ManageFlashCard();
                    return;
                }
                Console.WriteLine("Please enter either Answer or Question to update");
                question = Console.ReadLine();
            }

            flashCardController.EditFlashCard(flashCardId, question, answer);
        }

        public int ValidateFlashCardId()
        {
            Console.WriteLine("Please enter the FlashCard Id, Press 0 to Return to Main Menu");
            string userInput = Console.ReadLine();

            int flashCardId = 0;

            bool isInteger = int.TryParse(userInput, out flashCardId);


            while (flashCardId < 0 || !isInteger)
            {
                Console.WriteLine("Please Enter a valid FlashCard Id, To Return to Stack Menu Enter 0");
                userInput = Console.ReadLine();

                isInteger = int.TryParse(userInput, out flashCardId);
            }

            if (flashCardId == 0)
            {
                ManageFlashCard();
                return 0;
            }

            return flashCardId;
        }

        private void RemoveFlashCard()
        {
            Console.Clear();

            ViewFlashCard();
            Console.WriteLine();

            int flashCardId = ValidateFlashCardId();

            Console.WriteLine();

            while (!flashCardController.CheckFlashCardAvailable(flashCardId))
            {
                flashCardId = ValidateFlashCardId();
            }

            flashCardController.RemoveFlashCard(flashCardId);
        }

        private void ViewStack()
        {
            Console.WriteLine("Below are the available stacks");
            Console.WriteLine();

            List<Stacks> stackList = stackController.ViewStack();

            var stackTable = new ConsoleTable("Id", "Stack Name");

            foreach (Stacks stack in stackList)
            {
                stackTable.AddRow(stack.Id, stack.StackName);
            }
            stackTable.Write();

            Console.WriteLine();
        }

    }
}
