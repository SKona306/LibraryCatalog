using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace LibraryCatalog.Controllers
{
  public class AuthorsController: Controller
  {
    private readonly LibraryCatalogContext _db;

    public AuthorsController(LibraryCatalogContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Author> model = _db.Author.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.BookId = new SelectList(_db.Book, "BookId", "Title");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Author author, int BookId)
    {
      _db.Author.Add(author);
      _db.SaveChanges();
      if(BookId != 0)
      {
        _db.Author_Book.Add(new Author_Book() {BookId = BookId, AuthorId = author.AuthorId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details (int id)
    {
      var thisAuthor = _db.Author
        .Include(author => author.JoinEntities)
        .ThenInclude(join => join.Book)
        .FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.BookId = new SelectList(_db.Book, "BookId", "Title");
      var thisAuthor = _db.Author.FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    [HttpPost]
    public ActionResult Edit(Author author, int BookId)
    {
      if(BookId != 0)
      {
        _db.Author_Book.Add(new Author_Book() {BookId = BookId, AuthorId = author.AuthorId});
      }
      _db.SaveChanges();
      _db.Entry(author).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete (int id)
    {
      var thisAuthor = _db.Author.FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisAuthor= _db.Author.FirstOrDefault(author => author.AuthorId == id);
      _db.Author.Remove(thisAuthor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddBook(int id) {
      var thisAuthor = _db.Author.FirstOrDefault(author => author.AuthorId == id);
      ViewBag.BookId = new SelectList(_db.Book, "BookId", "Title");
      return View(thisAuthor);
    }

    [HttpPost]
    public ActionResult AddBook(Author author, int BookId) {
      if (BookId != 0) {
        _db.Author_Book.Add(new Author_Book() {BookId = BookId, AuthorId = author.AuthorId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteBook(int joinId)
    {
      var joinEntry = _db.Author_Book.FirstOrDefault(entry => entry.Author_BookId == joinId);
      _db.Author_Book.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }  
}