using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCardLearning.DataAccess;
using FlashCardLearning.Model.entities;


namespace FlashCardLearning.Controller
{
    internal class StackController
    {
        StackDataAccess stackDataAccess = new();

        internal void CreateStack(string stackName)
        {
            Stacks stackEntity = new();
            stackEntity.StackName = stackName;

            stackDataAccess.Addstack(stackEntity);
        }

        internal List<Stacks> ViewStack()
        {
            List<Stacks> stacks = stackDataAccess.Selectstack();
            return stacks;
        }

        internal bool CheckStackAvailable(int stackId)
        {
            bool StackAvailable = stackDataAccess.CheckStackAvailable(stackId);
            return StackAvailable;
        }

        internal void EditStack(int stackId, string stackName)
        {
            Stacks stackEntity = new();

            stackEntity.Id = stackId;
            stackEntity.StackName = stackName;

            stackDataAccess.UpdateStack(stackEntity);
        }

        internal void RemoveStack(int stackId)
        {
            stackDataAccess.DeleteStack(stackId);

            stackDataAccess.DeleteFlashCards(stackId);

            stackDataAccess.DeleteSudySession(stackId);
        }


    }
}
