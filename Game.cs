namespace GameStudioApp
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public decimal Price { get; set; }
        public int StudioId { get; set; }
        public Studio Studio { get; set; } = null!;
    }
}