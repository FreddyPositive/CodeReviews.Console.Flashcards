using FlashCardLearning.FlashCards.Console.Model.DTO;
using FlashCardLearning.FlashCards.Console.ObjectsMapper;
using FlashCardLearning.Model.Entities;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FlashCardLearning.DataAccess;

internal class FlashCardDataAccess
{
    StackDataAccess stackDataAccess = new();

    string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

    internal void AddFlashCard(FlashCardDetails flashCardEntity)
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
                    command.Parameters.Add("@stackId", SqlDbType.Int).Value = flashCardEntity.StackId;
                    command.ExecuteNonQuery();

                    Console.WriteLine();
                    Console.WriteLine($"Flash Card has been created succesfully");
                    Console.WriteLine();
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
    }

    internal List<T> SelectFlashCard<T>(int StackId, bool isEntityResult)
    {
        string query = "SELECT id, question, answer FROM FlashCards WHERE stack_id = @StackId";

        List<FlashCardDetails> flashCards = new List<FlashCardDetails>();

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
                            flashCards.Add(new FlashCardDetails
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
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
        if (isEntityResult)
        {
            return flashCards as List<T>;
        }
        else
        {
            var flashCardList = EntityToDTOMaper.flashCardList(flashCards);
            return flashCardList as List<T>;
        }

    }

    internal bool CheckFlashCardAvailable(int flashCardId, int stackId)
    {
        string query = "SELECT id FROM FlashCards WHERE id=@flashCardId";

        int flashCardEntityToCheck = GetFlashCardIdEntity(flashCardId, stackId);

        bool flashCardAvailable = true;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@flashCardId", SqlDbType.Int).Value = flashCardEntityToCheck;
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

    internal void UpdateFlashCard(FlashCardDetails flashCardEntity)
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
            
        int flashCardEntityToUpdate = GetFlashCardIdEntity(flashCardEntity.Id, flashCardEntity.StackId);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@flashCardId", SqlDbType.Int).Value = flashCardEntityToUpdate;
                    command.Parameters.Add("@question", SqlDbType.VarChar).Value = flashCardEntity.Question;
                    command.Parameters.Add("@answer", SqlDbType.VarChar).Value = flashCardEntity.Answer;
                    command.ExecuteNonQuery();

                    Console.WriteLine();
                    Console.WriteLine($"Flash Card updated successfully");
                    Console.WriteLine();
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
    }
    internal void DeleteFlashCard(FlashCardDetails flashCardEntity)
    {
        string query = "DELETE FROM FlashCards WHERE id = @flashCardId";

        int flashCardEntityToDelete = GetFlashCardIdEntity(flashCardEntity.Id, flashCardEntity.StackId);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@flashCardId", SqlDbType.Int).Value = flashCardEntityToDelete;
                    command.ExecuteNonQuery();

                    Console.WriteLine();
                    Console.WriteLine($"Flash Card deleted successfully");
                    Console.WriteLine();
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
    }
    public int GetFlashCardIdEntity(int flashCardId, int stackId)
    {
        FlashCardDetails flashCardToUpdate = new();
        try
        {
            int stackIdToCheck = stackDataAccess.GetStackIdEntity(stackId);

            var flashCardList = SelectFlashCard<FlashCardDetails>(stackIdToCheck, true);

            int flashCardIndex = flashCardId - 1;

            flashCardToUpdate = flashCardList[flashCardIndex];

            if (flashCardIndex < 0 || flashCardIndex >= flashCardList.Count)
            {
                return 0;
            }
            return flashCardToUpdate.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
    }
}

