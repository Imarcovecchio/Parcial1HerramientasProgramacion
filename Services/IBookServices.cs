using Microsoft.AspNetCore.Mvc.Rendering;

public interface IbookServices{
    void Create(Book book);
    void Update(Book book);
    void Delete(Book book);

    List<Book> QuerySearch(string str);

    List<Book>GetAll();
    Book? GetById(int id);

    SelectList GetAutoresSelectList();
}