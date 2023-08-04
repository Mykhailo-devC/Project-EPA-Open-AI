namespace Epa.Engine.Models.Entity_Models
{
    public class WordListWord
    {
        public int Id { get; set; }
        public int WordList_Id { get; set; }
        public int Word_Id { get; set; }
        public virtual Word Word { get; set; }
        public virtual WordList WordList { get; set; }
    }
}
