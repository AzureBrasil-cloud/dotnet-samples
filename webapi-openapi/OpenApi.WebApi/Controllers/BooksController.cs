using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OpenApi.WebApi.Controllers.Models;
using OpenApi.WebApi.Services;

namespace OpenApi.WebApi.Controllers;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
public class BooksController(
    BookService bookService,
    ILogger<BooksController> logger) : ControllerBase
{
    [HttpPost("add")]
    [EndpointDescription("Add a new book to the list")]
    [EndpointName("AddBook")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddBook([FromBody] Book book)
    {
        try
        {
            logger.LogInformation("Adding book: {BookName}", book.Name);
            bookService.AddBook(book);
            return CreatedAtAction(nameof(GetBooks), book);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error adding book");
            return BadRequest("An error occurred while adding the book");
        }
    }

    [HttpGet("list")]
    [EndpointDescription("Get all books, optionally filtered by name")]
    [EndpointName("GetBooks")]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    public IActionResult GetBooks(
        [Description("Optional name filter. If provided, only books containing this string in their name will be returned.")]
        string? name = null)
    {
        try
        {
            logger.LogInformation("Retrieving books with name filter: {NameFilter}", name ?? "none");
            var books = bookService.GetBooks(name);
            return Ok(books);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error retrieving books");
            return BadRequest("An error occurred while retrieving books");
        }
    }
}


