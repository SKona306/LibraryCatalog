namespace LibraryCatalog.Models 
{
  public class Author_Book 
  {
    public int Author_BookId {get; set;}
    public int AuthorId {get; set;}
    public int BookId {get; set;}
    public virtual Author Author {get; set;}
    public virtual Book Book {get; set;}
  }
}