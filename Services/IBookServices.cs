using Microsoft.AspNetCore.Mvc.Rendering;

public interface IbookServices{
    void Create(Book book);
    void Update(Book book);
    void Delete(Book book);
    void Reservar(Book book);

    List<Book> QuerySearch(string str);

    List<Categoria> Query(BookCreateViewModel viewmodel);
    List<Book>GetAll();

    List<Book> GetBooksAvailable();
    Book? GetById(int id);

    SelectList GetAutoresSelectList();
    SelectList GetCategoriaSelectList();
    
    List<Categoria> GetCategorias();
    List<Categoria> QueryCategorias(BookCreateViewModel viewModel);
    List<Book> GetByCategoriaId(int categoriaId);

    
    List<Categoria>? GetCategoriaSelectList(List<int> selectedCategoryIds);
}