using ConsoleTables;
using FlashCardLearning.Controller;
using FlashCardLearning.Model.entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlashCardLearning.View
{
    internal class StudyMenu
    {
        MainMenu mainMenu = new();
        StackMenu stackMenu = new();
        FlashCardMenu flashCardMenu = new();

        FlashCardController flashCardController = new();
        StackController stackController = new();
        StudyController studyController = new();

        int StackId = 0;
        int totalPoints = 0;
        int totalReviewdPoints = 0;
        DateTime sessionStartTime;
        DateTime sessionEndTime;
        int durationPerSession = 0;
        string FlashCardslist = "";

        internal void StudySession()
        {

            Console.Clear();
            Console.WriteLine("-----------Learning Menu-----------");
            Console.WriteLine();
            Console.WriteLine("Enter 1 to Open Avaliable Stacks");
            Console.WriteLine("Enter 2 to Open Learning Session History");
            Console.WriteLine("Enter 0 to Return To Main Menue");

            string userInput = Console.ReadLine();

            string[] allowedUserInput = { "1", "2", "0" };

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
                        SelectStack();
                        break;
                    case "2":
                        ViewSessionLog();
                        break;
                    case "0":
                        mainMenu.UserMainMenue();
                        break;
                }
            }
        }

        private void ViewSessionLog()
        {
            Console.WriteLine("------------Session Log -----------");
            Console.WriteLine();

            string pattern = @"^\d{4}-\d{2}-\d{2}$";

            Console.WriteLine("Please enter the Start Date(yyyy-mm-dd), Enter 0 to Return to Main Menu ");
            string startDate = Console.ReadLine();
            Console.WriteLine();

            if (startDate == "0")
            {
                StudySession();
                return;
            }
            while (!System.Text.RegularExpressions.Regex.IsMatch(startDate, pattern) ||
                    !DateTime.TryParseExact(startDate, "yyyy-mm-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime validDate))
            {
                if (startDate == "0")
                {
                    StudySession();
                    return;
                }
                Console.WriteLine("Please enter the Start Date(yyyy-mm-dd) in proper format");
                startDate = Console.ReadLine();
            }


            Console.WriteLine("Please enter the End Date(yyyy-mm-dd), Enter 0 to Return to Main Menu ");
            string endDate = Console.ReadLine();
            Console.WriteLine();
            if (endDate == "0")
            {
                StudySession();
                return;
            }
            while (!System.Text.RegularExpressions.Regex.IsMatch(endDate, pattern) ||
                 !DateTime.TryParseExact(startDate, "yyyy-mm-dd",
                 System.Globalization.CultureInfo.InvariantCulture,
                 System.Globalization.DateTimeStyles.None,
                 out DateTime validDate))
            {
                if (endDate == "0")
                {
                    StudySession();
                    return;
                }
                Console.WriteLine("Please enter the End Date(yyyy-mm-dd) in proper format");
                endDate = Console.ReadLine();


            }


            List<StudySessionHistory> historyList = studyController.ViewSessionLog(startDate, endDate);

            var stackTable = new ConsoleTable("Id", "Stack Name", "Points", "Session Date");

            int rowCount = 1;

            foreach (StudySessionHistory history in historyList)
            {
                stackTable.AddRow(rowCount, history.StackName, history.Points, history.SessionDate);
                rowCount += 1;
            }
            stackTable.Write();

            Console.WriteLine();

            Console.WriteLine("Enter 0 to return to Previous menu");
            string userInput = Console.ReadLine();

            if (userInput == "0")
            {
                StudySession();
                return;
            }
        }

        internal void ViewReports()
        {

            Console.WriteLine("Please Enter a year in YYYY format to generate report, enter 0 to return to Previous menu");
            Console.WriteLine();

            string year = Console.ReadLine();

            int YearInput = 0;

            while (string.IsNullOrEmpty(year) || year.Length != 4 || !int.TryParse(year, out YearInput))
            {
                if (year == "0")
                {
                    StudySession();
                    return;
                }

                Console.WriteLine("Please Enter year in YYYY format");
                year = Console.ReadLine();
            }

            if (YearInput == 0) mainMenu.UserMainMenue();

            List<StackReport> studySessions = studyController.ViewReports(YearInput);

            var ReportTable = new ConsoleTable(
                              "Stack Name",
                              "January",
                              "February",
                              "March",
                              "April",
                              "May",
                              "June",
                              "July",
                              "August",
                              "September",
                              "October",
                              "November",
                              "December"
                          );

            // Add rows for each stack
            foreach (StackReport studySession in studySessions)
            {
                ReportTable.AddRow(
                    studySession.StackName,
                    studySession.January,
                    studySession.February,
                    studySession.March,
                    studySession.April,
                    studySession.May,
                    studySession.June,
                    studySession.July,
                    studySession.August,
                    studySession.September,
                    studySession.October,
                    studySession.November,
                    studySession.December
                );
            }

            ReportTable.Write();

            Console.WriteLine();
            Console.WriteLine("Enter 0 to return to Previous menu");
            Console.WriteLine();

            string exit = Console.ReadLine();

            if (exit == "0") mainMenu.UserMainMenue();
        }

        private void SelectStack()
        {
            Console.Clear();

            ViewStack();

            Console.WriteLine("Please Enter the Stack Id to View the Flash Cards and Manage It , enter 0 to return to Previous menu");
            Console.WriteLine();

            StackId = stackMenu.ValidateStackId();

            if (StackId != 0) ViewFlashCards(StackId);
            else mainMenu.UserMainMenue();
        }

        public void ViewStack()
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

        private void ViewFlashCards(int StackId)
        {
            Console.WriteLine("Below are the Available Flash Cards");
            Console.WriteLine();

            List<FlashCards> flashCards = flashCardController.ViewFlashCards(StackId);

            var flashCardTable = new ConsoleTable("Id", "Question");

            foreach (FlashCards flashCard in flashCards)
            {
                flashCardTable.AddRow(flashCard.Id, flashCard.Question);
            }

            flashCardTable.Write();

            AnswerFlashCard(flashCards);
        }

        private void AnswerFlashCard(List<FlashCards> FlashCards)
        {
            sessionStartTime = DateTime.Now;

            Console.WriteLine("Please enter the id of the flash card you choose to answer. enter 0 to return to Previous menu");

            int flashCardId = flashCardMenu.ValidateFlashCardId();

            if (flashCardId == 0)
            {
                Console.Clear();

                Console.WriteLine();

                sessionEndTime = DateTime.Now;

                GetDurationPerSession();

                studyController.TrackSession(FlashCardslist, StackId, durationPerSession, totalPoints);

                SelectStack();

                totalPoints = 0;

            }

            Console.WriteLine("Please enter the answer");

            string answer = Console.ReadLine();
            string correctAnswer = "";

            foreach (FlashCards flashCard in FlashCards)
            {

                if (flashCard.Id == flashCardId)
                {
                    totalReviewdPoints += 20;
                    correctAnswer = flashCard.Answer;
                }
            }

            if (answer == correctAnswer)
            {
                Console.Clear();

                Console.WriteLine("The answer you entered was correct. 20 points added !!");
                Console.WriteLine();

                totalPoints += 20;

                FlashCardslist += $"{flashCardId},";

                Console.WriteLine($"You Scored {totalPoints} out of {totalReviewdPoints} points");
                Console.WriteLine();

                ViewFlashCards(StackId);

            }
            else
            {
                Console.Clear();

                Console.WriteLine("The answer you entered was Incorrect!!. Below is the correct answer");
                Console.WriteLine();

                Console.WriteLine($"Answer : {correctAnswer}");
                Console.WriteLine();

                ViewFlashCards(StackId);
            }
        }

        private void GetDurationPerSession()
        {
            TimeSpan duration = sessionEndTime.Subtract(sessionStartTime);

            durationPerSession = duration.Minutes;
        }
    }
}
