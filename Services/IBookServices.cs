using Microsoft.AspNetCore.Mvc.Rendering;

public interface IbookServices{
    void Create(Book book);
    void Update(Book book);
    void Delete(Book book);
    void Reservar(Book book);
    void Devolver(Book book);

    List<Book> QuerySearch(string str);

    List<Categoria> Query(BookCreateViewModel viewmodel);
    List<Book>GetAll();

    List<Book> GetBooksAvailable();
    List<Book> GetBooksUnAvailable();
    Book? GetById(int id);

    SelectList GetAutoresSelectList();
    SelectList GetCategoriaSelectList();
    
    List<Categoria> GetCategorias();
    List<Categoria> QueryCategorias(BookCreateViewModel viewModel);
    List<Book> GetByCategoriaId(int categoriaId);

    List<Book> GetBooksReserved();

    
    List<Categoria>? GetCategoriaSelectList(List<int> selectedCategoryIds);
}