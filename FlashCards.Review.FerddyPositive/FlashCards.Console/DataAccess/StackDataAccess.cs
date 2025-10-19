using FlashCardLearning.Model.Entities;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using FlashCardLearning.FlashCards.Console.Model.DTO;
using FlashCardLearning.FlashCards.Console.ObjectsMapper;

namespace FlashCardLearning.DataAccess;

internal class StackDataAccess
{
    string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

    internal void Addstack(Stacks stackEntity)
    {
        string query = "INSERT INTO Stacks (stack_name) VALUES (@stackname)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@stackname", SqlDbType.VarChar).Value = stackEntity.StackName;
                    command.ExecuteNonQuery();

                    Console.WriteLine();
                    Console.WriteLine($"Stack with name {stackEntity.StackName} has been created succesfully");
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

    internal List<T> Selectstack<T>(bool isEntityResult)
    {
        string query = "SELECT id, stack_name from stacks ORDER BY id ASC";

        List<Stacks> stackentity = new List<Stacks>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {   
            connection.Open();
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stackentity.Add(new Stacks
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                StackName = reader.GetString(reader.GetOrdinal("stack_name"))
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

        if(isEntityResult)
        {
            return stackentity as List<T>;
        }
        else
        {
            var stackList = EntityToDTOMaper.stackList(stackentity);

            return stackList as List<T>;
        }
            
    }

    internal bool CheckStackAvailable(int stackId)
    {
        string query = "SELECT id FROM stacks WHERE id=@stackId";

        int stackToVerify = GetStackIdEntity(stackId);
            
        bool stackAvailable = true;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@stackId", SqlDbType.Int).Value = stackToVerify;

                    object? result = command.ExecuteScalar();

                    if (result == null)
                    {
                        stackAvailable = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
        return stackAvailable;
    }

    internal void UpdateStack(StacksDTO stacks)
    {
        string query = "UPDATE stacks SET stack_name = @stackname  WHERE id = @stackId";

        int stackToUpdate = GetStackIdEntity(stacks.DisplayId);

        if (true)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@stackId", SqlDbType.Int).Value = stackToUpdate;
                        command.Parameters.Add("@stackname", SqlDbType.VarChar).Value = stacks.StackName;

                        command.ExecuteNonQuery();

                        Console.WriteLine();
                        Console.WriteLine($"Stack updated to {stacks.StackName} succesfully");
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
      
    }

    internal int DeleteStack(int stackId)
    {
        string query = "DELETE FROM stacks WHERE id = @stackId";

        int stackToDelete = GetStackIdEntity(stackId);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@stackId", SqlDbType.Int).Value = stackToDelete;
                    command.ExecuteNonQuery();

                    Console.WriteLine();
                    Console.WriteLine($"Stack deleted succesfully");
                    Console.WriteLine();
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
        return stackToDelete;
    }

    internal void DeleteFlashCards(int stackId)
    {

        string query = "DELETE FROM FlashCards WHERE stack_id = @stackId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@stackId", SqlDbType.Int).Value = stackId;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
    }
    internal void DeleteSudySession(int stackId)
    {

        string query = "DELETE FROM LearningLog WHERE stack_id = @stackId";
       
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@stackId", SqlDbType.Int).Value = stackId;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
    }

    internal bool CheckStackNameAvailable(string? stackName)
    {
        string query = "SELECT stack_name FROM stacks WHERE stack_name=@stackName";

        bool stackNameAvailable = true;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@stackName", SqlDbType.VarChar).Value = stackName;

                    object? result = command.ExecuteScalar();

                    if (result == null)
                    {
                        stackNameAvailable = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
        return stackNameAvailable;
    }

    public int GetStackIdEntity(int StackId)
     {
            Stacks stackToUpdate = null;
            try
            {
                var StackList = Selectstack<Stacks>(true);

                int stackIdIndex = StackId - 1;

                stackToUpdate = StackList[stackIdIndex];

                if (stackIdIndex < 0 || stackIdIndex >= StackList.Count)
                {
                    return 0;
                }
                return stackToUpdate.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
     }
}
