using Microsoft.AspNetCore.Mvc;
using Homework_1.Models;

namespace BooksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "Война и мир",
                Author = "Лев Толстой",
                Year = 1869
            },
            new Book
            {
                Id = 2,
                Title = "Преступление и наказание",
                Author = "Фёдор Достоевский",
                Year = 1866
            },
            new Book
            {
                Id = 3,
                Title = "Мастер и Маргарита",
                Author = "Михаил Булгаков",
                Year = 1967
            }
        };

        [HttpGet]
        public ActionResult<List<Book>> GetBooks()
        {
            return Ok(books);
        }

        [HttpPost]
        public ActionResult<Book> AddBook(Book book)
        {
            book.Id = books.Count + 1;

            books.Add(book);

            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }
    }
}