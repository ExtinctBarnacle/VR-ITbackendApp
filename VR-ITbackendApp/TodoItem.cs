namespace VR_ITbackendApp
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public TodoItem(string Title)
        {
            this.Title = Title;
        }
    }
}
