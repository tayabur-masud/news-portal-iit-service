using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using NewsPortalIIT.Domain.Models;

namespace NewsPortalIIT.Persistence;

/// <summary>
/// Represents the Entity Framework database context for the application, configured for MongoDB.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the context.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the Users collection.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the News collection.
    /// </summary>
    public DbSet<News> News { get; set; }

    /// <summary>
    /// Gets or sets the Comments collection.
    /// </summary>
    public DbSet<Comment> Comments { get; set; }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This is typically handled by dependency injection in Program.cs
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToCollection("users");
        modelBuilder.Entity<News>().ToCollection("news");
        modelBuilder.Entity<Comment>().ToCollection("comments");
    }
}
