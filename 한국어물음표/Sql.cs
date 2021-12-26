namespace KoreanQuestionMark.Sql
{
    using MySql;
    using MySql.Data;
    using MySql.Data.MySqlClient;

    class KQMSql
    {
        const string connectionString = "server=localhost;Database=KoreanQuestionMark;Uid=KoreanQuestionMark";
        const string table = "SearchedWords";

        public static void SearchWord(string word)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                if(wordExist(word))
                {
                    plusWord(word);
                }
                else
                {
                    addWord(word);
                }
            }
        }
        private static bool wordExist(string word)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string cmd = $"SELECT count(*) as count FROM SearchedWords WHERE Word LIKE '{word}';";
            MySqlCommand command = new MySqlCommand(cmd, connection);
            var reader = command.ExecuteReader();
            bool exists = false;
            while (reader.Read())
            {
                if ((long)reader["count"] != 0) exists = true;
                break;
            }
            reader.Close();
            connection.Close();
            return exists;
        }
        private static void addWord(string word)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string cmd = $"INSERT INTO {table} (Word, Number) VALUES ('{word}', '1');";
            
            MySqlCommand command = new MySqlCommand(cmd, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        private static void plusWord(string word)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string cmd = $"select * from {table} where Word = '{word}';";
            MySqlCommand command = new MySqlCommand(cmd, connection);
            
            long number = 0;
            using(var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    number = (long)reader["Number"];
                    break;
                }
            }

            cmd = $"UPDATE {table} SET Number = '{number + 1}' WHERE Word = '{word}';";
            command = new MySqlCommand(cmd, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}