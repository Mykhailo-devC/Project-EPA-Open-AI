namespace Epa.Engine.Models.Entity_Models
{
    public class Word : Entity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public IEnumerable<WordList> WordLists { get; set; }
        public IEnumerable<WordListWord> WordList_Word { get; set; }
    }
}
