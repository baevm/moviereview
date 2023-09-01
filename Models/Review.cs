public class Review
{
    public int Id { get; set; }
    public int Score { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public User User { get; set; }
    public Movie Movie { get; set; }
}