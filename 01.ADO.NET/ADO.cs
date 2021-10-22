﻿using System;
using System.Data.SqlClient;
using System.Text;

namespace AdoNEt
{
    public class Program
    {
        static void Main(string[] args)
        {
            string SqlConncetionString = @"Server=.;Database=MinionsDB;Integrated Security=true";

            using (SqlConnection conncetion = new SqlConnection(SqlConncetionString))
            {
                conncetion.Open();

                string createDatabase = "CREATE DATABASE MinionsDB";
                ExecutionNonQuery(conncetion, createDatabase);
                string[] createTable = CreateTableMethod();
                foreach (var currentLine in createTable)
                {
                    ExecutionNonQuery(conncetion, currentLine);
                }
                string[] insertDataStatments = InsertValuesToMinionsDBTables();

                foreach (var currentInsert in insertDataStatments)
                {
                    ExecutionNonQuery(conncetion, currentInsert);
                    ;
                }

                string quary = @"SELECT [Name],COUNT(mv.MinionId) FROM Villains  AS v JOIN MinionsVillains AS mv ON mv.VillainId=v.Id GROUP BY v.Id, v.[Name]";
                using (SqlCommand command = new SqlCommand(quary, conncetion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            string name = (string)reader[0];
                            int count = (int)reader[1];
                            Console.WriteLine(name + " - " + count);

                            ;
                        }

                    }
                }
                StringBuilder sb = new StringBuilder();
                int inputId = int.Parse(Console.ReadLine());
                string quaryId = $@"SELECT Name FROM Villains WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(quaryId, conncetion))
                {
                    command.Parameters.AddWithValue("@Id", inputId);
                    var result = command.ExecuteScalar();
                    if (result == null)
                    {
                        sb.AppendLine($"No villain with ID {inputId} exists in the database.");
                    }
                    else
                    {
                        sb.AppendLine($"Villain: {result}");
                        string quaryMinions = $@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,    m.Name,   m.Age  FROM MinionsVillains AS mv  JOIN Minions As m ON mv.MinionId = m.Id  WHERE mv.VillainId = @Id ORDER BY m.Name";
                        using (SqlCommand commandMinions = new SqlCommand(quaryMinions, conncetion))
                        {
                            commandMinions.Parameters.AddWithValue("@Id", inputId);
                            using (var reader = commandMinions.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                   
                                    sb.AppendLine("(no minions)");
                                }
                                else
                                {
                                    while (reader.Read())
                                    {

                                        var id = reader[0];
                                        var name = reader[1];
                                        var age = reader[2];
                                        sb.AppendLine($"{id}. {name} {age}");
                                    }
                                }
                            
                            }

                        }

                    }
                    Console.WriteLine(sb.ToString());
                }
            }
        }

        private static string[] InsertValuesToMinionsDBTables()
        {
            string[] result = new string[] {
               "INSERT INTO Countries([Name]) VALUES('Bulgaria'),('UK')",
                "INSERT INTO Towns([Name],CountryCode) VALUES('Provadia', 1),('London', 2)",
                "INSERT INTO MINIONS([Name],Age,TownId) VALUES('Ivan', 25, 1),('Krisuna', 23, 2)",
                "INSERT INTO EvilnessFactors([Name])VALUES('super good'),('good'),('bad'),('evil'),('super evil')",
                "INSERT INTO Villains([Name],EvilnessFactorId)VALUES('Gru', 5),('Margo', 1)",
                "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(1, 1),(2, 2)"
            };
            return result;
        }

        private static void ExecutionNonQuery(SqlConnection connection, string query)
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            };
        }

        private static string[] CreateTableMethod()
        {
            var result = new string[]
            {
                "CREATE TABLE Countries(Id INT PRIMARY KEY IDENTITY NOT NULL,[Name] NVARCHAR(50) NOT NULL)",
                "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY NOT NULL,[Name] NVARCHAR(50) NOT NULL,CountryCode INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL)",
                "CREATE TABLE MINIONS(Id INT PRIMARY KEY IDENTITY NOT NULL,[Name] NVARCHAR(50) NOT NULL,Age INT NOT NULL,TownId INT FOREIGN KEY REFERENCES Towns(Id) NOT NULL)",
                "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY NOT NULL,[Name] NVARCHAR(50) NOT NULL)",
                "CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY NOT NULL,[Name] NVARCHAR(50) NOT NULL,EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id) NOT NULL)",
                "CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id))"
            };
            return result;
        }
    }
}
