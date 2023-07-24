using EntityFrameworkCoreMock;
using Epa.Engine.DB;
using Epa.Engine.Models.DTO_Models;
using Epa.Engine.Models.Entity_Models;
using Epa.Engine.Repository;
using Epa.Engine.Repository.EntityRepositories;
using Microsoft.EntityFrameworkCore;

namespace Epa.Engine.Tests
{
    public class EpaRepositoryTests
    {
        public IRepository repository { get; set; }
        public DbContextMock<EpaDbContext> mockDbContext { get; set; }
        public EpaRepositoryTests()
        {
            mockDbContext = new DbContextMock<EpaDbContext>(new DbContextOptionsBuilder<EpaDbContext>().Options);
            repository = new WordListRepository(mockDbContext.Object);
        }

        [Fact]
        public async void GetAllWordLists()
        {
            var expected = new List<WordList> {
                             new WordList { Id = 1, Name = "Fruits" },
                             new WordList { Id = 2, Name = "Vagetables" }
                         };

            var mockDbSet = mockDbContext.CreateDbSetMock(x => x.WordLists, expected);

            var result = await repository.GetAll();
            Assert.True(result.Success, result.Message);

            foreach(var actual in result.Result.Cast<WordList>())
            {
                Assert.Contains(expected, x => x.Id == actual.Id &&
                                           x.Name == actual.Name);
            }
        }

        [Fact]
        public async void GetWordListById()
        {
            var expected = new List<WordList> {
                             new WordList { Id = 1, Name = "Fruits" }
                         };

            var mockDbSet = mockDbContext.CreateDbSetMock(x => x.WordLists, expected);

            var result = await repository.Get(1);
            Assert.True(result.Success, result.Message);

            var actual = result.Result.Single() as WordList;
            Assert.Contains(expected, x => x.Id == actual.Id &&
                                           x.Name == actual.Name);
        }

        [Theory]
        [InlineData("List1")]
        [InlineData("List2")]
        [InlineData("List3")]
        public async void AddNewWordList(string name)
        {
            var expected = new List<WordList> {
                             new WordList { Id = 0, Name = name },
                         };

            var mockDbSet = mockDbContext.CreateDbSetMock(x => x.WordLists);

            var result = await repository.Add(new WordListDTO {Name = name });
            Assert.True(result.Success, result.Message);

            var actual = repository.Get(0).Result.Result.Cast<WordList>().Single();
            Assert.Contains(expected, x => x.Id == actual.Id &&
                                           x.Name == actual.Name);
        }

        [Theory]
        [InlineData("List1")]
        [InlineData("List2")]
        [InlineData("List3")]
        public async void UpdateWordList(string name)
        {
            var initData = new List<WordList> {
                             new WordList { Id = 1, Name = "SomeName" },
                         };

            var expected = new List<WordList> {
                             new WordList { Id = 1, Name = name },
                         };

            var mockDbSet = mockDbContext.CreateDbSetMock(x => x.WordLists, initData);

            var result = await repository.Update(1, new WordListDTO { Name = name });
            Assert.True(result.Success, result.Message);

            var actual = repository.Get(1).Result.Result.Cast<WordList>().Single();
            Assert.Contains(expected, x => x.Id == actual.Id &&
                                           x.Name == actual.Name);
        }

        [Fact]
        public async void DeleteWordList()
        {
            var expected = new List<WordList> {
                             new WordList { Id = 1, Name = "SomeName" },
                         };

            var mockDbSet = mockDbContext.CreateDbSetMock(x => x.WordLists, expected);

            var result = await repository.Delete(1);
            Assert.True(result.Success, result.Message);

            var actual = result.Result.Single() as WordList;
            Assert.Contains(expected, x => x.Id == actual.Id &&
                                           x.Name == actual.Name);

            var emptyResult = await repository.GetAll();
            Assert.Empty(emptyResult.Result);
        }
    }
}
