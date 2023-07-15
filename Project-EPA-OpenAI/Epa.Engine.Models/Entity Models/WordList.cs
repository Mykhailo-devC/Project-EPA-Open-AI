namespace Epa.Engine.Models.Entity_Models
{
    public class WordList : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        // public int User_Id { get; set; }
        // public User User { get; set; }

        public virtual IEnumerable<Word> Words { get; set; }


    }
}
