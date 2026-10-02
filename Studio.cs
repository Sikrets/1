using System.Collections.Generic;

namespace GameStudioApp
{
    public class Studio
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CityId { get; set; }
        public City City { get; set; } = null!;
        public int FoundationYear { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();
    }
}