using FlashCardLearning.DataAccess;
using FlashCardLearning.FlashCards.Console.Model.DTO;
using FlashCardLearning.Model.Entities;

namespace FlashCardLearning.Controller;

internal class FlashCardController
{
    FlashCardDataAccess flashCardDataAccess = new();
    StackDataAccess stackDataAccess = new();

    internal void CreateFlashCard(string question, string answer, int stackId)
    {
        int stackIdEntityToInsert = stackDataAccess.GetStackIdEntity(stackId);

        FlashCardDetails flashCardEntity = new();
        flashCardEntity.Question = question;
        flashCardEntity.Answer = answer;
        flashCardEntity.StackId = stackIdEntityToInsert;

        flashCardDataAccess.AddFlashCard(flashCardEntity);
    }

    internal bool CheckFlashCardAvailable(int flashCardId, int stackId)
    {
        bool flashCardAvailable = flashCardDataAccess.CheckFlashCardAvailable(flashCardId, stackId);
        return flashCardAvailable;
    }

    internal void EditFlashCard(int flashCardId, string question, string answer, int stackId)
    {
        FlashCardDetails flashCardEntity = new();
        flashCardEntity.Id = flashCardId;
        flashCardEntity.Question = question;
        flashCardEntity.Answer = answer;
        flashCardEntity.StackId = stackId;

        flashCardDataAccess.UpdateFlashCard(flashCardEntity);
    }

    internal void RemoveFlashCard(int flashCardId, int stackId)
    {
        FlashCardDetails flashCardEntity = new();
        flashCardEntity.Id = flashCardId;
        flashCardEntity.StackId = stackId;
        flashCardDataAccess.DeleteFlashCard(flashCardEntity);
    }

    internal List<FlashCardsDTO> ViewFlashCards(int StackId)
    {
        int stackIdEntity = stackDataAccess.GetStackIdEntity(StackId);
        List<FlashCardsDTO> flashCards = flashCardDataAccess.SelectFlashCard<FlashCardsDTO>(stackIdEntity, false);
        return flashCards;
    }
}
