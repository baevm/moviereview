using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }


    public DbSet<Movie> Movies { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<MovieActors> MovieActors { get; set; }
    public DbSet<MovieGenres> MovieGenres { get; set; }

    // Link all many-to-many relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieActors>()
            .HasKey(ma => new { ma.MovieId, ma.ActorId });

        modelBuilder.Entity<MovieActors>()
            .HasOne(ma => ma.Movie)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(ma => ma.MovieId);

        modelBuilder.Entity<MovieActors>()
            .HasOne(ma => ma.Actor)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(ma => ma.ActorId);


        modelBuilder.Entity<MovieGenres>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });

        modelBuilder.Entity<MovieGenres>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId);

        modelBuilder.Entity<MovieGenres>()
            .HasOne(mg => mg.Genre)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.GenreId);
    }
}