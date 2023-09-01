public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortSummary { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int Budget { get; set; }
    public int Rating { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<MovieActors> MovieActors { get; set; }
    public ICollection<MovieGenres> MovieGenres { get; set; }
}