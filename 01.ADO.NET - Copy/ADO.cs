using System;
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

                //string createDatabase = "CREATE DATABASE MinionsDB";
                //ExecutionNonQuery(conncetion, createDatabase);
                //string[] createTable = CreateTableMethod();
                //foreach (var currentLine in createTable)
                //{
                //    ExecutionNonQuery(conncetion, currentLine);
                //}
                //string[] insertDataStatments = InsertValuesToMinionsDBTables();

                //foreach (var currentInsert in insertDataStatments)
                //{
                //    ExecutionNonQuery(conncetion, currentInsert);
                //    ;
                //}

                //string quary = @"SELECT [Name],COUNT(mv.MinionId) FROM Villains  AS v JOIN MinionsVillains AS mv ON mv.VillainId=v.Id GROUP BY v.Id, v.[Name]";
                //using (SqlCommand command = new SqlCommand(quary, conncetion))
                //{
                //    using (var reader = command.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {

                //            string name = (string)reader[0];
                //            int count = (int)reader[1];
                //            Console.WriteLine(name + " - " + count);

                //            ;
                //        }

                //    }
                //}
                //int inputId = int.Parse(Console.ReadLine());
                //string quaryId = $@"SELECT Name FROM Villains WHERE Id = @Id";
                //using (SqlCommand command = new SqlCommand(quaryId, conncetion))
                //{
                //    StringBuilder sb = new StringBuilder();
                //    command.Parameters.AddWithValue("@Id", inputId);
                //    var result = command.ExecuteScalar();
                //   string resultString = SelectMethod(result,inputId,conncetion,sb);
                //    Console.WriteLine(resultString);
                //}


                //SELECT Id FROM Villains WHERE Name = @Name
                //SELECT Id FROM Minions WHERE Name = @Name
                //INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(@villainId, @minionId)
                //INSERT INTO Villains(Name, EvilnessFactorId)  VALUES(@villainName, 4)
                //INSERT INTO Minions(Name, Age, TownId) VALUES(@nam, @age, @townId)
                //INSERT INTO Towns(Name) VALUES(@townName)
                //SELECT Id FROM Towns WHERE Name = @townName
                StringBuilder sb = new StringBuilder();
                Console.Write("Minion: ");
                string[] minions = Console.ReadLine().Split(" ");
                Console.Write("Villain: ");
                string villain = Console.ReadLine();
                string villainName = villain;
                string minionName = minions[0];
                int age = int.Parse(minions[1]);
                string townName = minions[2];
                var iDVillain = new object();
                var iDMinions = new object();
                var iDTown = new object();

                string townId = @"SELECT Id FROM Towns WHERE Name = @townName";
                using (SqlCommand command = new SqlCommand(townId, conncetion))
                {
                    command.Parameters.AddWithValue("@townName", townName);
                    iDTown = command.ExecuteScalar();
                    if (iDTown == null)
                    {

                        string townQuarry = @"INSERT INTO Towns(Name) VALUES(@townName)";
                        using (var townCommand = new SqlCommand(townQuarry, conncetion))
                        {
                            townCommand.Parameters.AddWithValue("@townName", townName);
                            townCommand.ExecuteNonQuery();
                            sb.AppendLine($"Town {townName} was added to the database.");
                        }

                    }

                }
                string villainNameId = @"SELECT Id FROM Villains  WHERE Name = @Name";
                using (SqlCommand command = new SqlCommand(villainNameId, conncetion))
                {
                    command.Parameters.AddWithValue("@Name", villainName);
                    iDVillain = command.ExecuteScalar();
                    if (iDVillain == null)
                    {
                        string villainQuarry = @"INSERT INTO Villains(Name, EvilnessFactorId)  VALUES(@villainName, 4)";
                        using (var villainCommand = new SqlCommand(villainQuarry, conncetion))
                        {
                            villainCommand.Parameters.AddWithValue("@villainName", villainName);
                            villainCommand.ExecuteNonQuery();
                            sb.AppendLine($"Villain {villainName} was added to the database.");
                        }
                    }
                }

                string minionNameId = @"SELECT Id FROM Minions WHERE Name = @Name";
                using (SqlCommand command = new SqlCommand(minionNameId, conncetion))
                {
                    command.Parameters.AddWithValue("@Name", minionName);
                    iDMinions = command.ExecuteScalar();
                    if (iDMinions == null)
                    {
                        string minionsQuarry = @"INSERT INTO Minions(Name, Age, TownId) VALUES(@name, @age, @townId)";
                        using (var minionsCommand = new SqlCommand(minionsQuarry, conncetion))
                        {

                            minionsCommand.Parameters.AddWithValue("@name", minionName);
                            minionsCommand.Parameters.AddWithValue("@age", age);
                            minionsCommand.Parameters.AddWithValue("@townId", iDTown);
                            minionsCommand.ExecuteNonQuery();

                            sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");
                        }
                    }
                }
                if(iDMinions!=null && iDVillain !=null)
                {
                    string quarry = @"INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(@villainId, @minionId)";
                    using (var command = new SqlCommand(quarry, conncetion))
                    {
                        command.Parameters.AddWithValue("@villainId", iDVillain);
                        command.Parameters.AddWithValue("@minionId", iDMinions);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine(sb.ToString());
            
            }
        }

        private static string SelectMethod(object result, int inputId, SqlConnection conncetion, StringBuilder sb)
        {
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
            return sb.ToString().TrimEnd();
        }
        private static string InsertValues(string quarry)
        {
            string result = quarry;

            return result;
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
