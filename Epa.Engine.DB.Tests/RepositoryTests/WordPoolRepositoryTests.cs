using EntityFrameworkCoreMock;
using Epa.Engine.DB;
using Epa.Engine.Models.DTO_Models;
using Epa.Engine.Models.Entity_Models;
using Epa.Engine.Repository;
using Epa.Engine.Repository.EntityRepositories;
using Microsoft.EntityFrameworkCore;

namespace Epa.Engine.Tests.RepositoryTests
{
    public class WordPoolRepositoryTests
    {
        public IRepository repository { get; set; }
        public DbContextMock<EpaDbContext> mockDbContext { get; set; }
        public WordPoolRepositoryTests()
        {
            mockDbContext = new DbContextMock<EpaDbContext>(new DbContextOptionsBuilder<EpaDbContext>().Options);
            repository = new WordPoolRepository(mockDbContext.Object);
        }

        [Theory]
        [InlineData("List1")]
        [InlineData("List2")]
        [InlineData("List3")]
        public async void AddNewWord(string name)
        {
            var expected = new List<Word> {
                             new Word { Id = 0, Value = name },
                         };

            var mockDbSet = mockDbContext.CreateDbSetMock(x => x.WordPool);

            var result = await repository.Add(new WordDTO { Value = name });
            Assert.True(result.Success, result.Message);

            var actual = mockDbSet.Object.First();
            Assert.Contains(expected, x => x.Id == actual.Id &&
                                           x.Value == actual.Value);
        }

        [Theory]
        [InlineData("List1")]
        [InlineData("List2")]
        [InlineData("List3")]
        public async void UpdateWord(string value)
        {
            var initData = new List<Word> {
                             new Word { Id = 1, Value = "SomeName" },
                         };

            var expected = new List<Word> {
                             new Word { Id = 1, Value = value },
                         };

            var mockDbSet = mockDbContext.CreateDbSetMock(x => x.WordPool, initData);

            var result = await repository.Update(1, new WordDTO { Value = value });
            Assert.True(result.Success, result.Message);

            var actual = mockDbSet.Object.First();
            Assert.Contains(expected, x => x.Id == actual.Id &&
                                           x.Value == actual.Value);
        }

        [Fact]
        public async void DeleteWordList()
        {
            var expected = new List<Word> {
                             new Word { Id = 1, Value = "SomeName" },
                         };

            var mockDbSet = mockDbContext.CreateDbSetMock(x => x.WordPool, expected);

            var result = await repository.Delete(1);
            Assert.True(result.Success, result.Message);

            var actual = result.Result.First() as Word;
            Assert.Contains(expected, x => x.Id == actual.Id &&
                                           x.Value == actual.Value);

            var emptyResult = mockDbSet.Object.ToList();
            Assert.Empty(emptyResult);
        }
    }

    
}
