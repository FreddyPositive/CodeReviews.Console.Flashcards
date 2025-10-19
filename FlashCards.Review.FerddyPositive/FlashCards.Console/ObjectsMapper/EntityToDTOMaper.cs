using FlashCardLearning.FlashCards.Console.Model.DTO;
using FlashCardLearning.Model.Entities;

namespace FlashCardLearning.FlashCards.Console.ObjectsMapper;

class EntityToDTOMaper
{
    internal static List<StacksDTO> stackList(List<Stacks> stackentity)
    {
        return stackentity
      .Select((pb, index) => new StacksDTO
      {
          DisplayId = index + 1,
          StackName = pb.StackName ?? string.Empty
      })
      .ToList();
    }
    internal static List<FlashCardsDTO> flashCardList(List<FlashCardDetails> flashCardentity)
    {
        return flashCardentity
      .Select((pb, index) => new FlashCardsDTO
      {
          DisplayId = index + 1,
          Answer = pb.Answer ?? string.Empty,
          Question = pb.Question ?? string.Empty,
          StackId = pb.StackId
      })
      .ToList();
    }
}
