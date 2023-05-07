namespace Epa.Engine.Models.Entity_Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int? WordList_Id { get; set; }
        public WordList WordList { get; set; }
    }
}
