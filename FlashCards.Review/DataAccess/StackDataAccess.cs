using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using FlashCardLearning.Model.entities;


namespace FlashCardLearning.DataAccess
{
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
                        command.Parameters.Add("@stackname", SqlDbType.Int).Value = stackEntity.StackName;
                        command.ExecuteNonQuery();

                        Console.WriteLine();
                        Console.WriteLine($"Stack with name {stackEntity.StackName} has been created succesfully");
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }

            }
        }

        internal List<Stacks> Selectstack()
        {
            string query = "SELECT id, stack_name from stacks";

            List<Stacks> stacks = new List<Stacks>();

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
                                stacks.Add(new Stacks
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
            return stacks;
        }

        internal bool CheckStackAvailable(int stackId)
        {
            string query = "SELECT id FROM stacks WHERE id=@stackId";

            bool stackAvailable = true;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@stackId", SqlDbType.VarChar).Value = stackId;

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

        internal void UpdateStack(Stacks stacks)
        {
            string query = "UPDATE stacks SET stack_name = @stackname  WHERE id = @stackId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@stackId", SqlDbType.Int).Value = stacks.Id;
                        command.Parameters.Add("@stackname", SqlDbType.VarChar).Value = stacks.StackName;

                        command.ExecuteNonQuery();

                        Console.WriteLine();
                        Console.WriteLine($"Stack updated to {stacks.StackName} succesfully");
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }
            }
        }

        internal void DeleteStack(int stackId)
        {
            string query = "DELETE FROM stacks WHERE id = @stackId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@stackId", SqlDbType.Int).Value = stackId;
                        command.ExecuteNonQuery();

                        Console.WriteLine();
                        Console.WriteLine($"Stack deleted succesfully");
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                }
            }
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
    }
}
