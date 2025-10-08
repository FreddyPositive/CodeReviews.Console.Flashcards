using System.Configuration;
using FlashCardLearning.Model.entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FlashCardLearning.DataAccess
{
    internal class StudyDataAccess
    {

        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

        internal void AddSession(StudySession studySession)
        {
            string query = "INSERT INTO LearningLog (flash_cards_reviewed, stack_id, duration_minutes, points) " +
                            "VALUES (@flashCardslist, @stackId, @durationPerSession, @totalPoints)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@flashCardslist", SqlDbType.VarChar).Value = studySession.FlashCardsLearned;
                        command.Parameters.Add("@stackId", SqlDbType.VarChar).Value = studySession.StackId;
                        command.Parameters.Add("@durationPerSession", SqlDbType.VarChar).Value = studySession.SessionDuration;
                        command.Parameters.Add("@totalPoints", SqlDbType.VarChar).Value = studySession.Points;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }

            }
        }

        internal List<StackReport> SelectReport(int year)
        {
            string query = @"
                            SELECT 
                                s.stack_name,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 1  THEN l.stack_id END) AS January,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 2  THEN l.stack_id END) AS February,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 3  THEN l.stack_id END) AS March,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 4  THEN l.stack_id END) AS April,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 5  THEN l.stack_id END) AS May,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 6  THEN l.stack_id END) AS June,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 7  THEN l.stack_id END) AS July,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 8  THEN l.stack_id END) AS August,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 9  THEN l.stack_id END) AS September,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 10 THEN l.stack_id END) AS October,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 11 THEN l.stack_id END) AS November,
                                COUNT(CASE WHEN DATEPART(month, l.session_date) = 12 THEN l.stack_id END) AS December
                            FROM LearningLog l
                            INNER JOIN Stacks s ON s.id = l.stack_id
                            WHERE YEAR(l.session_date) = @Year
                            GROUP BY s.stack_name
                            ORDER BY s.stack_name;";

            List<StackReport> reports = new List<StackReport>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Year", SqlDbType.Int).Value = year;  // use Int not VarChar

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reports.Add(new StackReport
                                {
                                    StackName = reader.GetString(reader.GetOrdinal("stack_name")),
                                    January = reader.GetInt32(reader.GetOrdinal("January")),
                                    February = reader.GetInt32(reader.GetOrdinal("February")),
                                    March = reader.GetInt32(reader.GetOrdinal("March")),
                                    April = reader.GetInt32(reader.GetOrdinal("April")),
                                    May = reader.GetInt32(reader.GetOrdinal("May")),
                                    June = reader.GetInt32(reader.GetOrdinal("June")),
                                    July = reader.GetInt32(reader.GetOrdinal("July")),
                                    August = reader.GetInt32(reader.GetOrdinal("August")),
                                    September = reader.GetInt32(reader.GetOrdinal("September")),
                                    October = reader.GetInt32(reader.GetOrdinal("October")),
                                    November = reader.GetInt32(reader.GetOrdinal("November")),
                                    December = reader.GetInt32(reader.GetOrdinal("December"))
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }
            }
            return reports;
        }

        internal List<StudySessionHistory> SelectSessionLog(StudySessionHistoryInput studySessionHistoryInput)
        {
            string query = @"
                            select 
                            stack_name,points,session_date from LearningLog l
                            INNER JOIN Stacks s ON s.id = l.stack_id
                            where session_date >= @startDate and session_date < @endDate
                            ORDER BY session_date ASC";

            List<StudySessionHistory> history = new List<StudySessionHistory>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@startDate", SqlDbType.Date).Value = studySessionHistoryInput.SessionStartDate;  // use Int not VarChar
                        command.Parameters.Add("@endDate", SqlDbType.Date).Value = studySessionHistoryInput.SessionEndDate;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                history.Add(new StudySessionHistory
                                {
                                    StackName = reader.GetString(reader.GetOrdinal("stack_name")),
                                    Points = reader.GetInt32(reader.GetOrdinal("points")),
                                    SessionDate = reader.GetDateTime(reader.GetOrdinal("session_date")),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }
            }
            return history;
        }
    }
}
