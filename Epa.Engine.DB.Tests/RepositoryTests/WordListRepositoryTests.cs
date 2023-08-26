using EntityFrameworkCoreMock;
using EPA.Engine.DB;
using EPA.Engine.Models.DTO_Models;
using EPA.Engine.Models.Entity_Models;
using EPA.Engine.Models.Logic_Models;
using EPA.Engine.Repository;
using EPA.Engine.Repository.EntityRepositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EPA.Engine.Tests.RepositoryTests
{
    public class WordListRepositoryTests
    {
        public IRepository repository { get; set; }
        public DbContextMock<EpaDbContext> mockDbContext { get; set; }
        public WordListRepositoryTests()
        {
            mockDbContext = new DbContextMock<EpaDbContext>(new DbContextOptionsBuilder<EpaDbContext>().Options);
            var mockResolver = new Mock<ServiceResolver.RepositoryResolver>();
            var wordPoolMock = new Mock<WordPoolRepository>(mockDbContext.Object);
            mockResolver.Setup(x => x.Invoke(RepositoryType.WordPool)).Returns(wordPoolMock.Object);
            repository = new WordListRepository(mockDbContext.Object, new TransactionHandler(),mockResolver.Object);
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

            foreach (var actual in result.Result.Cast<WordList>())
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

            var result = await repository.Add(new WordListDTO { Name = name });
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
                             new WordList { Id = 1, Name = "SomeName", Words = new List<Word>() },
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
