using AspTemplate.Core.Model.Auth;
using AspTemplate.Core.Model.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Type = AspTemplate.Core.Model.Main.Type;

namespace AspTemplate.Data;

/// <summary>
/// Контекст базы данных приложения
/// </summary>
public class ApplicationContext : DbContext
{
    /// <inheritdoc />
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    public ApplicationContext()
    {
    }

    #region Auth

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    #endregion

    #region Main

    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Type> Types { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}