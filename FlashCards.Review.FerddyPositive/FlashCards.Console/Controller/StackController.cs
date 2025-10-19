using FlashCardLearning.DataAccess;
using FlashCardLearning.Model.Entities;
using FlashCardLearning.FlashCards.Console.Model.DTO;

namespace FlashCardLearning.Controller;

internal class StackController
{
    StackDataAccess stackDataAccess = new();

    internal void CreateStack(string stackName)
    {
        Stacks stackEntity = new();
        stackEntity.StackName = stackName;

        stackDataAccess.Addstack(stackEntity);
    }

    internal List<StacksDTO> ViewStack()
    {
        List<StacksDTO> stacks = stackDataAccess.Selectstack<StacksDTO>(false);
        return stacks;
    }

    internal bool CheckStackAvailable(int stackId)
    {
        bool StackAvailable = stackDataAccess.CheckStackAvailable(stackId);
        return StackAvailable;
    }

    internal void EditStack(int stackId, string stackName)
    {
        StacksDTO stackDetails = new();

        stackDetails.DisplayId = stackId;
        stackDetails.StackName = stackName;

        stackDataAccess.UpdateStack(stackDetails);
    }

    internal void RemoveStack(int stackId)
    {
        int stackIdToUpdate =  stackDataAccess.DeleteStack(stackId);

        stackDataAccess.DeleteFlashCards(stackIdToUpdate);

        stackDataAccess.DeleteSudySession(stackIdToUpdate);
    }

    internal bool CheckStackNameAvailable(string? stackName)
    {
        bool StackAvailable = stackDataAccess.CheckStackNameAvailable(stackName);
        return StackAvailable;
    }
}
