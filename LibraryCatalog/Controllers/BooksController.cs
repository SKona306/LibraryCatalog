using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryCatalog.Controllers
{
  public class BooksController : Controller
  {
    private readonly LibraryCatalogContext _db;

    public BooksController(LibraryCatalogContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Book> model = _db.Book.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.AuthorsId = new SelectList(_db.Author, "AuthorId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Book book, int AuthorId)
    {
      _db.Book.Add(book);
      _db.SaveChanges();
      if(AuthorId != 0)
      {
        _db.Author_Book.Add(new Author_Book() {AuthorId = AuthorId, BookId = book.BookId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisBook = _db.Book
        .Include(author => author.JoinEntities)
        .ThenInclude(join => join.Author)
        .FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    public ActionResult Edit(int id)
    {
      var thisBook = _db.Book.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult Edit(Book book, int AuthorId)
    {
      if(AuthorId != 0)
      {
        _db.Author_Book.Add(new Author_Book() {AuthorId = AuthorId, BookId = book.BookId});
      }
      _db.SaveChanges();
      _db.Entry(book).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisBook = _db.Book.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisBook = _db.Book.FirstOrDefault(book => book.BookId == id);
      _db.Book.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddAuthor(int id) {
      var thisBook = _db.Book.FirstOrDefault(book => book.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Author, "AuthorId", "Name");
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult AddAuthor(Book book, int AuthorId) 
    {
      if (AuthorId != 0)
      {
        _db.Author_Book.Add(new Author_Book() {AuthorId = AuthorId, BookId = book.BookId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteAuthor(int joinId)
    {
      var joinEntry = _db.Author_Book.FirstOrDefault(entry => entry.Author_BookId == joinId);
      _db.Author_Book.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}