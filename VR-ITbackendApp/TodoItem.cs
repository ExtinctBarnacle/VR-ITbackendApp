namespace VR_ITbackendApp
{
    public class TodoItem
    {
        public int Id { get; set; } //        (primary key);
        public string? Title { get; set; } //(обязательно, максимальная длина: 100);
        public bool IsCompleted { get; set; } // default value = false
        public DateTime CreatedAt { get; set; }
    }
}
