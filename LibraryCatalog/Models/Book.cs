using System.Collections.Generic;

namespace LibraryCatalog.Models 
{
  public class Book 
  {
    public Book() 
    {
      this.JoinEntities = new HashSet <Author_Book> ();
    }

    public int BookId {get; set;}
    public string Title {get; set;}
    public string Description { get; set; }
    public virtual ICollection <Author_Book> JoinEntities {get; set;}
  }
}