namespace FlashCardLearning.Model.Entities;

internal class StudySession
{
    public string FlashCardsLearned { get; set; }
    public int StackId { get; set; }
    public int SessionDuration { get; set; }
    public int Points { get; set; }
}

internal class StudySessionHistory
{
    public string StackName { get; set; }
    public DateTime SessionDate { get; set; }
    public int Points { get; set; }
}

