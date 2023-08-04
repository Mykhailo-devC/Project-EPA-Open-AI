namespace Epa.Engine.Models.Entity_Models
{
    public class Word : Entity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public virtual IEnumerable<WordList> WordLists { get; set; }
        public virtual IEnumerable<WordListWord> WordList_Word { get; set; }
    }
}
