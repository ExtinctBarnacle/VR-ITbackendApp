using System.Data.SQLite;

namespace VR_ITbackendApp
{
    class DBService
    {
        // файл базы данных
        static string СonnectionString = "Data Source=toDoDatabase.db; Version=3;";

        // метод создаёт таблицу ToDoList (список дел), если в БД toDoDatabase.db такой таблицы нет
        public static void CreateToDoTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(СonnectionString))
            {
                connection.Open();
                string createTableQuery = "CREATE TABLE IF NOT EXISTS ToDoList (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT(100), IsCompleted BOOLEAN DEFAULT(FALSE), CreatedAt datetime)";
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
                string insertQuery = "INSERT INTO ToDoList (Title,IsCompleted, CreatedAt) VALUES (@Title, @IsCompleted, @CreatedAt)";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Title", toDo.Title);
                    command.Parameters.AddWithValue("@IsCompleted", toDo.IsCompleted);
                    command.Parameters.AddWithValue("@CreatedAt", toDo.CreatedAt);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        // метод читает из БД и возвращает список дел
        public static TodoItem[] LoadToDoTable()
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
                            toDo[^1] = new TodoItem("");
                            toDo[^1].Title = (string) reader[1];
                            toDo[^1].IsCompleted = (bool) reader[2];
                            toDo[^1].CreatedAt = (DateTime) reader[3];
                            Array.Resize(ref toDo, toDo.Length + 1);
                        }
                        Array.Resize(ref toDo, toDo.Length - 1);
                        connection.Close();
                        return toDo;
                    }
                }
            }
        }

        // метод находит в БД дело по номеру
        public static TodoItem LoadToDoById(int Id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(СonnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM ToDoList where Id = @Id";
                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        TodoItem toDo = new TodoItem("");
                        if (reader.HasRows && reader.Read())
                        {
                            toDo.Id = (int)(long) reader[0];
                            if (toDo.Id != Id)
                            {
                                return null;
                            }
                            toDo.Title = (string)reader[1];
                            toDo.IsCompleted = (bool)reader[2];
                            toDo.CreatedAt = (DateTime)reader[3];
                        } else return null;
                        connection.Close();
                        return toDo;
                    }
                }
            }
        }

        // метод удаляет в таблице ToDoList дело с номером, указанным клиентом
        public static bool RemoveToDoById(int Id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(СonnectionString))
            {
                connection.Open();
                string selectQuery = "DELETE * FROM ToDoList where Id = @Id";
                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return false;
                    }
                    connection.Close();
                    return true;
                }
            }
        }

        // метод обновляет в таблице ToDoList дело с номером, указанным клиентом
        public static bool UpdateDataToDB(TodoItem toDo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(СonnectionString))
            {
                connection.Open();
                string insertQuery = "UPDATE ToDoList SET Title = @Title, IsCompleted = @IsCompleted, CreatedAt = @CreatedAt WHERE Id = @Id";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", toDo.Id);
                    command.Parameters.AddWithValue("@Title", toDo.Title);
                    command.Parameters.AddWithValue("@IsCompleted", toDo.IsCompleted);
                    command.Parameters.AddWithValue("@CreatedAt", toDo.CreatedAt);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return false;
                    }
                }
                connection.Close();
                return true;
            }
        }
    }
}