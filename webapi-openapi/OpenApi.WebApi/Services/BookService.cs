using OpenApi.WebApi.Controllers.Models;

namespace OpenApi.WebApi.Services;

public class BookService
{
    private static readonly List<Book> Books = new();

    public void AddBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        Books.Add(book);
    }

    public List<Book> GetBooks(string? nameFilter = null)
    {
        if (string.IsNullOrWhiteSpace(nameFilter))
            return Books.ToList();

        return Books.Where(b => b.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}


