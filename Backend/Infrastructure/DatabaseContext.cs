
using Application.DTOs;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Project>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Pattern>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Post>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Like>()
            .HasKey(p => new { p.UserId, p.LikedUserId });
            
        
        modelBuilder.Entity<User>().Ignore(t => t.Patterns);
        modelBuilder.Entity<Pattern>().Ignore(t => t.User);
        modelBuilder.Entity<User>().Ignore(t => t.Projects);
        modelBuilder.Entity<Project>().Ignore(t => t.Posts);
        modelBuilder.Entity<Like>().Ignore(l => l.User);



        //Foregin key to author ID
        modelBuilder.Entity<Pattern>()
            .HasOne(pattern => pattern.User)
            .WithMany(user => user.Patterns)
            .HasForeignKey(pattern => pattern.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Project>()
            .HasOne(project => project.User)
            .WithMany(user => user.Projects)
            .HasForeignKey(project => project.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Project>()
            .HasOne(project => project.Pattern)
            .WithMany(p => p.Projects)
            .HasForeignKey(p => p.PatternId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Post>()
            .HasOne(post => post.Project)
            .WithMany(project => project.Posts)
            .HasForeignKey(post => post.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Like>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Like>()
                .HasOne(p => p.LikedUser)
                .WithMany()
                .HasForeignKey(p => p.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);

    }

    
    public DbSet<User> UserTable { get; set; }
    public DbSet<Project> ProjectTable { get; set; }
    public DbSet<Post> PostTable { get; set; }
    public DbSet<Pattern> PatternTable { get; set; }
    
    public DbSet<Like> LikeTable { set; get; }
}