using VR_ITbackendApp.Models;

namespace VR_ITbackendApp.Util
{
    
    // интерфейс для объявления операций со списком дел
    public interface ITodoService
    {
        public abstract static void CreateToDoTable();

        public abstract static void StoreDataToDB(TodoItem toDo);

        public abstract static TodoItem[] LoadToDoTable();

        public abstract static TodoItem LoadToDoById(int Id);

        public abstract static bool RemoveToDoById(int Id);

        public abstract static bool UpdateDataToDB(TodoItem toDo);

    }
}
