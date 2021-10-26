using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace NETADO
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection(Configuration.CONNECTION_STRING);

           await sqlConnection.OpenAsync();

            await using (sqlConnection)
            {
                await ViliansWIthMoreThan3MinionsAsync(sqlConnection);
            }

            await using(sqlConnection)
            {
                await MinionNamesOnCurrentVillainAsync(sqlConnection);
            }

        }

        private static async Task MinionNamesOnCurrentVillainAsync(SqlConnection sqlConnection)
        {

            SqlCommand idVillaincommand = new SqlCommand(Quarries.GET_ID_FROM_VILAINS, sqlConnection);
            //Prevent SQL injection
            SqlParameter idParameter = new SqlParameter("@Id",SqlDbType.Int) ;
            idParameter.Value = idVillaincommand;
            idVillaincommand.Parameters.Add(idParameter);

            object villaNameObj = await idVillaincommand.ExecuteScalarAsync();

        }

        static private async Task ViliansWIthMoreThan3MinionsAsync(SqlConnection sqlconnetction)
        {
            SqlCommand command = new SqlCommand(Quarries.VILIANS_WITH_MORE_THAN_3_MINIONS,sqlconnetction);
            SqlDataReader reader = await command.ExecuteReaderAsync();

            await using (reader)
            {
                while (await reader.ReadAsync())
                {

                    string name = reader.GetString(0);
                    int count = reader.GetInt32(1);
                    Console.WriteLine($"{name} - {count}");
                }
            }
        }
    }
}
