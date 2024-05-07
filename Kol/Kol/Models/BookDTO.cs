namespace Kol.Models;

public class BookDTO
{
    public int PK { get; set; }
    public string title { get; set; } = string.Empty;
    public List<AuthorDTO> Authors { get; set; } = null!;

}

public class AuthorDTO
{
    //public int PK { get; set; }
    public string first_na { get; set; } = string.Empty;
    public string last_na { get; set; } = string.Empty;
}