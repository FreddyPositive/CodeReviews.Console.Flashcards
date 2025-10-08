using FlashCardLearning.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardLearning.Helper
{
    internal class Utilities
    {
        FlashCardMenu flashCardMenu = new();

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
                flashCardMenu.ManageFlashCard();
                return 0;
            }

            return flashCardId;
        }
    }
}
