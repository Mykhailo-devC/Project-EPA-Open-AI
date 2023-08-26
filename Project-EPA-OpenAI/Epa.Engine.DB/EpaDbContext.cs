using EPA.Engine.Models.Entity_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EPA.Engine.DB
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

        public virtual DbSet<WordList> WordLists { get; set; }
        public virtual DbSet<Word> WordPool { get; set; }
        public virtual DbSet<WordListWord> WordListWords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordList>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Name).HasMaxLength(50);
                entity.Property(x => x.CreationDate).HasDefaultValue(DateTime.Now);
                entity.HasMany(x => x.Words).WithMany(x => x.WordLists)
                    .UsingEntity<WordListWord>();
                entity.HasMany(x => x.WordList_Word).WithOne(x => x.WordList)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Value).HasMaxLength(50);
                entity.HasIndex(x => x.Value).IsUnique();
                entity.HasMany(x => x.WordLists).WithMany(x => x.Words)
                    .UsingEntity<WordListWord>();
                entity.HasMany(x => x.WordList_Word).WithOne(x => x.Word)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WordListWord>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.HasOne(x => x.WordList)
                      .WithMany(x => x.WordList_Word)
                      .HasForeignKey(x => x.WordList_Id)
                      .HasConstraintName("FK__WordListWord__WordList_Id_Key")
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Word)
                      .WithMany(x => x.WordList_Word)
                      .HasForeignKey(x => x.Word_Id)
                      .HasConstraintName("FK__WordListWord__Word_Id_Key")
                      .OnDelete(DeleteBehavior.Cascade);
            });
            this.ApplyWordListSeed(modelBuilder);
            this.ApplyWordPoolSeed(modelBuilder);
            this.ApplyWordListWordSeed(modelBuilder);

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

        private void ApplyWordListWordSeed(ModelBuilder builder)
        {
            var wordMatching = new List<WordListWord>()
            {
                new WordListWord()
                {
                    Id = 1,
                    WordList_Id = 1,
                    Word_Id = 1
                },
                new WordListWord()
                {
                    Id = 2,
                    WordList_Id = 1,
                    Word_Id = 2
                },
                new WordListWord()
                {
                    Id = 3,
                    WordList_Id = 1,
                    Word_Id = 3
                },
                new WordListWord()
                {
                    Id = 4,
                    WordList_Id = 1,
                    Word_Id = 4
                },
                new WordListWord()
                {
                    Id = 5,
                    WordList_Id = 1,
                    Word_Id = 5
                }
            };

            builder.Entity<WordListWord>().HasData(wordMatching);
        }

        private void ApplyWordPoolSeed(ModelBuilder builder)
        {
            var wordPool = new List<Word>()
            {
                new Word()
                {
                    Id = 1,
                    Value = "Orange",
                },
                new Word()
                {
                    Id = 2,
                    Value = "Apple",
                },
                new Word()
                {
                    Id = 3,
                    Value = "Cherry"
                },
                new Word()
                {
                    Id = 4,
                    Value = "Strawberry"
                },
                new Word()
                {
                    Id = 5,
                    Value = "Peach"
                }
            };

            builder.Entity<Word>().HasData(wordPool);
        }
    }
}
