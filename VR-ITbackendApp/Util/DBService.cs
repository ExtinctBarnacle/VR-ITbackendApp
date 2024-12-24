using System.Data.SQLite;

namespace VR_ITbackendApp
{
    class DBService
    {
        // файл базы данных
        static string СonnectionString = "Data Source=toDoDatabase.db; Version=3;";

        // метод создаёт таблицу ToDoList (список дел), если в БД toDoDatabase.db такой таблицы нет
        public static void CreateChatTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(СonnectionString))
            {
                connection.Open();
                string createTableQuery = "CREATE TABLE IF NOT EXISTS ToDoList (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT, IsCompleted BOOLEAN, CreatedAt TEXT)";
                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        // метод сохраняет в таблицу ToDoList очередное дело, присланное клиентом
        public static void StoreDataToDB(TodoItem toDo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(СonnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO ToDoList (Title,IsCompleted, CreatedAt) VALUES (@Title, @CreatedAt, @IsCompleted)";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Title", toDo.Title);
                    command.Parameters.AddWithValue("@CreatedAt", toDo.CreatedAt);
                    command.Parameters.AddWithValue("@IsCompleted", toDo.IsCompleted);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        // метод читает из БД и возвращает список дел
        public static TodoItem[] LoadChatTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(СonnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM ToDoList";
                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        TodoItem[] toDo = new TodoItem[1];
                        while (reader.Read())
                        {
                            toDo[^1] = new TodoItem();
                            toDo[^1].Title = (string)reader[1];
                            toDo[^1].CreatedAt = (DateTime)reader[2];
                            toDo[^1].IsCompleted = (bool)reader[3];
                            Array.Resize(ref toDo, toDo.Length + 1);
                        }
                        Array.Resize(ref toDo, toDo.Length - 1);
                        connection.Close();
                        return toDo;
                    }
                }
            }
        }
    }
}