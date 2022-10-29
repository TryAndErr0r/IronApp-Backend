using Microsoft.EntityFrameworkCore;

namespace IronApp.Model.QuizEntityModel
{
    public class QuizContext : DbContext
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Question> Questions { get; set; }

        public QuizContext(DbContextOptions<QuizContext> options)
        : base(options)
        { 
        }
    }
}
