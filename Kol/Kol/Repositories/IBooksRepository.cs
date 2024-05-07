using Kol.Models;

namespace Kol.Repositories;

public interface IBooksRepository
{

    public Task<bool> DoesTitleExists(int pk);
    public Task<BookDTO> GetAuthors(int pk);
    public Task AddBook(string title);
    public Task AddAuthor(string firstName, string lastname);
    //public Task AddBooksAuthors(NewBooksAuthors newBooksAuthors);
    //Nie mam czasu zeby dopisac, ale tutaj trzeba bylo
    //stworzyc nowe model dla tabeli books_authors
    //za pomocą transaction = await connection.BeginTransactionAsync();
    //zrobic inserta 
    //dopisac post controller


}