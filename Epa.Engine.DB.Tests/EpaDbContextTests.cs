using EPA.Engine.DB;
using EPA.Engine.Models.Entity_Models;
using Microsoft.EntityFrameworkCore;

namespace EPA.Engine.Tests
{
    public class EpaDbContextTests : IDisposable
    {
        DbContextOptions<EpaDbContext> options;
        public EpaDbContextTests()
        {
            string dbHost = "(localdb)\\MSSQLLocalDB";
            string dbName = "epa_db_for_testing";
            string dbPassword = "P@ssw0rd121#";

            var DefaultConnection = "Data Source={0};Initial Catalog={1};TrustServerCertificate=True";

            options = new DbContextOptionsBuilder<EpaDbContext>()
                        .UseSqlServer(string.Format(DefaultConnection, dbHost, dbName))
                        .Options;

            using (var context = new EpaDbContext(options))
            {
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public async void DatabaseExists()
        {
            using(var context = new EpaDbContext(options))
            {
                var exists = await context.Database.CanConnectAsync();
                Assert.True(exists);
            }
        }

        [Fact]
        public void TablesHasValues()
        {
            using (var context = new EpaDbContext(options))
            {
                Assert.True(context.WordLists.Any());
                Assert.True(context.WordPool.Any());
                Assert.True(context.WordListWords.Any());
            }
        }

        [Fact]
        public void CheckSeedDataOfWordListsTable()
        {
            var wordList = new WordList()
            {
                Id = 1,
                Name = "Fruits",
            };

            using (var context = new EpaDbContext(options))
            {
                var seedWordList = context.WordLists.FirstOrDefault();
                Assert.Equal(seedWordList.Id, wordList.Id);
                Assert.Equal(seedWordList.Name, wordList.Name);
                Assert.Null(seedWordList.Words);
                Assert.NotNull(seedWordList.CreationDate);
            }
        }

        [Theory]
        [InlineData(1, "Orange")]
        [InlineData(2, "Apple")]
        [InlineData(3, "Cherry")]
        [InlineData(4, "Strawberry")]
        [InlineData(5, "Peach")]
        public void CheckSeedDataOfWordPoolTable(int id, string value, int list_id = 1)
        {
            using (var context = new EpaDbContext(options))
            {
                var seedWordPool = context.WordPool.Include(x => x.WordList_Word).ToList();

                Assert.Contains(seedWordPool, x => x.Value == value &&
                                                    x.Id == id &&
                                                    x.WordLists == null &&
                                                    x.WordList_Word
                                                    .First(x => x.Id == id).WordList_Id == list_id);

                Assert.Equal(5, seedWordPool.Count);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void CheckSeedDataOfWordPoolWordTable(int id, int list_id = 1)
        {
            using (var context = new EpaDbContext(options))
            {
                var seedWordPool = context.WordListWords.ToList();

                Assert.Contains(seedWordPool, x =>  x.Id == id &&
                                                    x.Word_Id == id &&
                                                    x.WordList_Id == list_id);

                Assert.Equal(5, seedWordPool.Count);
            }
        }

        [Fact]
        public void GetWordListNameOfSpecifiedWord()
        {
            using (var context = new EpaDbContext(options))
            {
                var word = context.WordPool.Include(x => x.WordLists).FirstOrDefault();
                var wordList = context.WordLists.FirstOrDefault();

                Assert.Equal(wordList.Name, word.WordLists.First().Name);
            }
        }

        [Fact]
        public void GetWordValueFromWordList()
        {
            using (var context = new EpaDbContext(options))
            {
                var wordList = context.WordLists.Include(x => x.Words).FirstOrDefault();
                var words = context.WordPool.Where(x => x.WordList_Word.Any(x => x.WordList_Id == wordList.Id)).ToList();

                foreach (var word in words)
                {
                    Assert.Contains(wordList.Words, x => x.Value == word.Value);
                }
            }
        }

        [Theory]
        [InlineData("Fruit1", 1)]
        public void AddWordPoolItems(string value, int wordListId)
        {
            var newWord = new Word()
            {
                Value = value,
            };

            var wordMathcing = new WordListWord()
            {
                WordList_Id = wordListId
            };

            using (var context = new EpaDbContext(options))
            {
                context.WordPool.Add(newWord);
                context.SaveChanges();

                wordMathcing.Word_Id = newWord.Id;
                context.WordListWords.Add(wordMathcing);
                context.SaveChanges();
            }

            using (var context = new EpaDbContext(options))
            {
                var word = context.WordPool.Include(x => x.WordList_Word).Where(x => x.Value == value).FirstOrDefault();

                Assert.Equal(word.Value, newWord.Value);
                Assert.Equal(word.WordList_Word.First().WordList_Id, wordMathcing.WordList_Id);
            }
        }

        [Theory]
        [InlineData("List1")]
        public void AddWordLists(string name)
        {
            var newWordList = new WordList()
            {
                Name = name
            };

            using (var context = new EpaDbContext(options))
            {
                context.WordLists.Add(newWordList);
                context.SaveChanges();
            }

            using (var context = new EpaDbContext(options))
            {
                var word = context.WordLists.Where(x => x.Name == name).FirstOrDefault();

                Assert.Equal(word.Name, newWordList.Name);
            }
        }

        [Theory]
        [InlineData(1)]
        public void RemoveWordLists(int id)
        {
            using (var context = new EpaDbContext(options))
            {
                var wordList = context.WordLists.Find(id);
                context.WordLists.Remove(wordList);
                context.SaveChanges();
            }

            using (var context = new EpaDbContext(options))
            {
                var wordList = context.WordLists.ToList();

                Assert.DoesNotContain(wordList, x => x.Id == id);

            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void RemoveWordsFromWordPool(int id)
        {
            using (var context = new EpaDbContext(options))
            {
                var word = context.WordPool.Find(id);
                context.WordPool.Remove(word);
                context.SaveChanges();
            }

            using (var context = new EpaDbContext(options))
            {

                var words = context.WordPool.ToList();
                
                Assert.DoesNotContain(words, x => x.Id == id);

                var wordList = context.WordLists.Include(x => x.Words).FirstOrDefault();

                Assert.DoesNotContain(wordList.Words, x => x.Id == id);
            }
        }

        public void Dispose()
        {
            using (var context = new EpaDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            options = null;
        }
    }
}