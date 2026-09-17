using Microsoft.AspNetCore.Mvc;
using IPpr1.Models;

namespace IPpr1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static List<Book> books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "Clean Code",
                Author = "Robert Martin",
                Year = 2008,
                Genre = "Programming"
            },

            new Book
            {
                Id = 2,
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                Year = 1937,
                Genre = "Fantasy"
            },

            new Book
            {
                Id = 3,
                Title = "1984",
                Author = "George Orwell",
                Year = 1949,
                Genre = "Dystopian"
            }
        };


        [HttpGet]
        public ActionResult<List<Book>> GetAll()
        {
            return Ok(books);
        }


        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }


        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            book.Id = books.Count > 0
                ? books.Max(x => x.Id) + 1
                : 1;

            books.Add(book);

            return CreatedAtAction(
                nameof(GetById),
                new { id = book.Id },
                book);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, Book updatedBook)
        {
            var book = books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Year = updatedBook.Year;
            book.Genre = updatedBook.Genre;

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            books.Remove(book);

            return NoContent();
        }
    }
}