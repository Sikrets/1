using System.Collections.Generic;

namespace GameStudioApp
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public Country Country { get; set; } = null!;
        public List<Studio> Studios { get; set; } = new List<Studio>();
    }
}