using Kol.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kol.Controllers;

public class BooksController : ControllerBase
{
    private readonly IBooksRepository _booksRepository;
    public BooksController(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }

    [HttpGet("api/books/{id}/authors")]
    public async Task<IActionResult> GetAuthors(int id)
    {
        if (!await _booksRepository.DoesTitleExists(id))
            return NotFound($"Title with given ID - {id} doesn't exist");

        var authors = await _booksRepository.GetAuthors(id);
        return Ok(authors);

    }

}