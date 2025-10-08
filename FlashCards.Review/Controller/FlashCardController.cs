using FlashCardLearning.DataAccess;
using FlashCardLearning.Model.entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardLearning.Controller
{
    class FlashCardController
    {
        FlashCardDataAccess flashCardDataAccess = new();

        internal void CreateFlashCard(string? question, string answer, int stackId)
        {
            FlashCards flashCardEntity = new();
            flashCardEntity.Question = question;
            flashCardEntity.Answer = answer;
            flashCardEntity.StackId = stackId;

            flashCardDataAccess.AddFlashCard(flashCardEntity);
        }

        internal bool CheckFlashCardAvailable(int flashCardId)
        {
            bool flashCardAvailable = flashCardDataAccess.CheckFlashCardAvailable(flashCardId);
            return flashCardAvailable;
        }

        internal void EditFlashCard(int flashCardId, string question, string answer)
        {
            FlashCards flashCardEntity = new();
            flashCardEntity.Id = flashCardId;
            flashCardEntity.Question = question;
            flashCardEntity.Answer = answer;

            flashCardDataAccess.UpdateFlashCard(flashCardEntity);
        }

        internal void RemoveFlashCard(int flashCardId)
        {
            flashCardDataAccess.DeleteFlashCard(flashCardId);
        }

        internal List<FlashCards> ViewFlashCards(int StackId)
        {
            List<FlashCards> flashCards = flashCardDataAccess.SelectFlashCard(StackId);
            return flashCards;
        }


    }
}
