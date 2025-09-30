using FlashCardLearning.DataAccess;
using FlashCardLearning.Model.entities;

namespace FlashCardLearning.Controller
{
    internal class StudyController
    {
        StudyDataAccess studyDataAccess = new();
        internal void TrackSession(string flashCardslist, int stackId, int durationPerSession, int totalPoints)
        {
            StudySession studySession = new();
            studySession.FlashCardsLearned = flashCardslist;
            studySession.StackId = stackId;
            studySession.SessionDuration = durationPerSession;
            studySession.Points = totalPoints;

            studyDataAccess.AddSession(studySession);
        }


        internal List<StackReport> ViewReports(int year)
        {
            List<StackReport> report = studyDataAccess.SelectReport(year);
            return report;
        }

        internal List<StudySessionHistory> ViewSessionLog(string? startDate, string? endDate)
        {
            StudySessionHistoryInput studySessionHistoryInput = new();

            DateOnly startDateOnly = DateOnly.Parse(startDate);
            DateOnly endDateOnly = DateOnly.Parse(endDate);

            studySessionHistoryInput.SessionStartDate = startDateOnly;
            studySessionHistoryInput.SessionEndDate = endDateOnly;

            List<StudySessionHistory> studySessionHistories = studyDataAccess.SelectSessionLog(studySessionHistoryInput);
            return studySessionHistories;
        }
    }
}
