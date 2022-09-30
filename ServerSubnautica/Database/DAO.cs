using System;
using System.Data.SQLite;
using System.IO;


namespace ServerSubnautica.Database
{
    internal class Dao
    {
        private static SQLiteConnection connection;
        
        public static SQLiteConnection CreateConnection()
        {
            // Create a new database connection:
            connection = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True;");
            // Open the connection:
            try
            {
                connection.Open();

                if (!File.Exists("database.db"))
                {
                    Console.WriteLine("Creating database");
                    File.Create("database.db");
                    CreateTable(connection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return connection;
        }

        public static void SaveLocation(string playerId, string lastLocX, string lastLocY, string lastLocZ)
        {
            SQLiteCommand command = connection.CreateCommand();

            command.CommandText =
                $"INSERT INTO Players (PlayerId, LastPositionX, LastPositionY, LastPositionZ) VALUES ({playerId}, {lastLocX}, {lastLocY}, {lastLocZ})";
            command.ExecuteNonQuery();
        }

        public static string GetLocation()
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Players";

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int readerId = reader.GetInt16(0);
                float readerX = reader.GetFloat(1);
                float readerY = reader.GetFloat(2);
                float readerZ = reader.GetFloat(3);
                
                return $"{readerId};{readerX};{readerY};{readerZ}";
            }

            return null;
        }

        private static void CreateTable(SQLiteConnection conn)
        {
            SQLiteCommand sqliteCmd = conn.CreateCommand();
            string createSql = "CREATE TABLE Players (PlayerId INT, LastPositionX REAL, LastPositionY REAL, LastPositionZ REAL)";
            string createSql1 = "CREATE TABLE Inventory (PlayerId INT, ItemId INT, Quantity INT, Misc VARCHAR(20))";
            sqliteCmd.CommandText = createSql;
            sqliteCmd.ExecuteNonQuery();
            sqliteCmd.CommandText = createSql1;
            sqliteCmd.ExecuteNonQuery();
        }

        public static void ReadData(SQLiteConnection conn)
        {

            SQLiteCommand sqLiteCmd = conn.CreateCommand();
            sqLiteCmd.CommandText = "SELECT * FROM SampleTable WHERE 1";
            
            SQLiteDataReader sqLiteDataReader = sqLiteCmd.ExecuteReader();
            
            while (sqLiteDataReader.Read())
            {
                string myReader = sqLiteDataReader.GetString(0);
                Console.WriteLine(myReader);
            }
            conn.Close();
        }
    }
}