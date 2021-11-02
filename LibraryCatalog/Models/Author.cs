using System.Collections.Generic;

namespace LibraryCatalog.Models {
  public class Author {
    public Author() {
      this.JoinEntities = new HashSet <Author_Book> ();
    }

    public int AuthorId {get; set;}
    public string Name {get; set;}
    public virtual ICollection <Author_Book> JoinEntities {get; set;}
  }
}