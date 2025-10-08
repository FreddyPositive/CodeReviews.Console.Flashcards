using FlashCardLearning.Model.entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Collections;

namespace FlashCardLearning.DataAccess
{
    internal class FlashCardDataAccess
    {

        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

        internal void AddFlashCard(FlashCards flashCardEntity)
        {
            string query = "INSERT INTO FlashCards (question, answer, stack_id) VALUES (@question, @answer, @stackId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@question", SqlDbType.VarChar).Value = flashCardEntity.Question;
                        command.Parameters.Add("@answer", SqlDbType.VarChar).Value = flashCardEntity.Answer;
                        command.Parameters.Add("@stackId", SqlDbType.VarChar).Value = flashCardEntity.StackId;
                        command.ExecuteNonQuery();

                        Console.WriteLine();
                        Console.WriteLine($"Flash Card has been created succesfully");
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }
            }
        }

        internal List<FlashCards> SelectFlashCard(int StackId)
        {

            string query = "SELECT id, question, answer FROM FlashCards WHERE stack_id = @StackId";

            List<FlashCards> flashCards = new List<FlashCards>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@StackId", SqlDbType.VarChar).Value = StackId;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                flashCards.Add(new FlashCards
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Question = reader.GetString(reader.GetOrdinal("question")),
                                    Answer = reader.GetString(reader.GetOrdinal("answer"))
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
            return flashCards;
        }
        internal bool CheckFlashCardAvailable(int flashCardId)
        {
            string query = "SELECT id FROM FlashCards WHERE id=@flashCardId";

            bool flashCardAvailable = true;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@flashCardId", SqlDbType.Int).Value = flashCardId;
                        object? result = command.ExecuteScalar();
                        if (result == null)
                        {
                            flashCardAvailable = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }
            }
            return flashCardAvailable;
        }

        internal void UpdateFlashCard(FlashCards flashCardEntity)
        {
            string question = "";
            string answer = "";

            if (flashCardEntity.Question != "" && flashCardEntity.Answer != "")
            {
                question = "question = @question,";
                answer = " answer = @answer";

            }
            else if (flashCardEntity.Answer != "")
            {
                answer = " answer = @answer";
            }
            else if (flashCardEntity.Question != "")
            {
                question = "question = @question";
            }


            string query = $"UPDATE FlashCards SET {question} {answer} WHERE id = @flashCardId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@flashCardId", SqlDbType.Int).Value = flashCardEntity.Id;
                        command.Parameters.Add("@question", SqlDbType.VarChar).Value = flashCardEntity.Question;
                        command.Parameters.Add("@answer", SqlDbType.VarChar).Value = flashCardEntity.Answer;
                        command.ExecuteNonQuery();

                        Console.WriteLine();
                        Console.WriteLine($"Flash Card updated successfully");
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }
            }
        }

        internal void DeleteFlashCard(int flashCardId)
        {
            string query = "DELETE FROM FlashCards WHERE id = @flashCardId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@flashCardId", SqlDbType.Int).Value = flashCardId;
                        command.ExecuteNonQuery();

                        Console.WriteLine();
                        Console.WriteLine($"Flash Card deleted successfully");
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }
            }
        }
    }

}

