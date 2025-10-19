namespace FlashCardLearning.Model.Entities;

internal class FlashCardDetails
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int StackId { get; set; }
}
