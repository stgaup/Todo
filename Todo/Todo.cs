namespace Todo
{
    [Serializable]
    internal class Todo
    {
        public Todo(int pri, string text)
        {
            Priority = pri;
            Text = text;
        }

        public int Priority { get; set; } = 0;

        public string Text { get; set; } = string.Empty;
    }
}
