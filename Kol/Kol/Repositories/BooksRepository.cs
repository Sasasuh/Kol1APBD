using System.Data;
using Kol.Models;
using Microsoft.Data.SqlClient;

namespace Kol.Repositories;

public class BooksRepository : IBooksRepository
{
    private readonly IConfiguration _configuration;

    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesTitleExists(int pk)
    {
        var query = "SELECT 1 FROM books WHERE PK = @PK";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@PK", pk);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<BookDTO> GetAuthors(int pk)
    {
        var query =
            @"SELECT books.PK AS bookPK, books.title as title, authors.first_name as FirstName, authors.last_name as LastName 
            FROM books, authors
            JOIN books_authors ON books_authors.FK_author = authors.PK WHERE books.PK = @PK
            ";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@PK", pk);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        var bookPkOrdinal = reader.GetOrdinal("bookPK");
        var titleOrdinal = reader.GetOrdinal("title");
        var firstNameOrdinal = reader.GetOrdinal("FirstName");
        var lastNameOrdinal = reader.GetOrdinal("LastName");

        BookDTO bookDto = null;

        while (await reader.ReadAsync())
        {
            if (bookDto is not null)
            {
                bookDto.Authors.Add(new AuthorDTO()
                {
                    first_na = reader.GetString(firstNameOrdinal),
                    last_na = reader.GetString(lastNameOrdinal)
                });
            }
            else
            {
                bookDto = new BookDTO()
                {
                    PK = reader.GetInt32(bookPkOrdinal),
                    title = reader.GetString(titleOrdinal),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO()
                        {
                            first_na = reader.GetString(firstNameOrdinal),
                            last_na = reader.GetString(lastNameOrdinal)
                        }
                    }
                };
            }
        }

        if (bookDto is null) throw new Exception();
        return bookDto;
    }

    public async Task AddBook(string title)
    {
        var insert = @"INSERT INTO BOOK VALUES(@title)";
        
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = insert;
        command.Parameters.AddWithValue("@title", title);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }

    public async Task AddAuthor(string firstName, string lastName)
    {
        var insert = @"INSERT INTO authors VALUES(@firstName, @lastName)";
        
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = insert;
        command.Parameters.AddWithValue("@firstName", firstName);
        command.Parameters.AddWithValue("@lastName", lastName);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
}