using Epa.Engine.Models.Entity_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Epa.Engine.DB
{
    public class EpaDbContext : DbContext
    {
        public EpaDbContext(DbContextOptions<EpaDbContext> options) : base(options)
        {
            try
            {
                var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                
                if(dbCreator != null)
                {
                    if (!dbCreator.CanConnect()) dbCreator.Create();
                    if (!dbCreator.HasTables()) dbCreator.CreateTables();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DbSet<WordList> WordLists { get; set; }
        public DbSet<Word> WordPool { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordList>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Name).HasMaxLength(50);
                entity.Property(x => x.CreationDate).HasDefaultValue(DateTime.Now);
                entity.HasMany(x => x.Words).WithOne(x => x.WordList)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Value).HasMaxLength(50);
                entity.HasOne(x => x.WordList).WithMany(x => x.Words)
                    .HasForeignKey(x => x.WordList_Id)
                    .HasConstraintName("FK__WordList__CustomWord")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            this.ApplyWordListSeed(modelBuilder);
            this.ApplyWordPoolSeed(modelBuilder);

        }
        private void ApplyWordListSeed(ModelBuilder builder)
        {
            var wordList = new WordList()
            {
                Id = 1,
                Name = "Fruits"
            };

            builder.Entity<WordList>().HasData(wordList);
        }

        private void ApplyWordPoolSeed(ModelBuilder builder)
        {
            var wordPool = new List<Word>()
            {
                new Word()
                {
                    Id = 1,
                    Value = "Orange",
                    WordList_Id = 1
                },
                new Word()
                {
                    Id = 2,
                    Value = "Apple",
                    WordList_Id = 1
                },
                new Word()
                {
                    Id = 3,
                    Value = "Cherry",
                    WordList_Id = 1
                },
                new Word()
                {
                    Id = 4,
                    Value = "Strawberry",
                    WordList_Id = 1
                },
                new Word()
                {
                    Id = 5,
                    Value = "Peach",
                    WordList_Id = 1
                }
            };

            builder.Entity<Word>().HasData(wordPool);
        }
    }
}
