namespace moviereview.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<MovieGenres> MovieGenres { get; set; }
    }
}