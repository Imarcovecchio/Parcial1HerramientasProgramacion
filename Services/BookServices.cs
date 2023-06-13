using Parcial.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


public class BookServices : IbookServices
{
    private readonly AutorContext _context;

    public BookServices(AutorContext context)
    {
        _context = context;
    }
    
    public SelectList GetAutoresSelectList(){
        var autores=_context.Autor.ToList();
        var selectList = new SelectList(autores, "Id", "Nombre");

        return selectList;
    }

    public List<Categoria> GetCategorias(){
        var categorias = _context.Categoria.ToList();
        return categorias;
    }

    public SelectList GetCategoriaSelectList()
        {
            var categorias = _context.Categoria.ToList();
            var selectList = new SelectList(categorias,"Id","Nombre");
            
            return selectList;
        }

    

    public List<Categoria>? GetCategoriaSelectList(List<int> selectedCategoryIds){

        var categorias = _context.Categoria.Where(c =>selectedCategoryIds.Contains(c.Id)).ToList();
        
        return categorias;
    }
        
    public void Create(Book book)
    {
        
        _context.Add(book);
        _context.SaveChanges();
    }

    public void Delete(Book book)
    {
        _context.Remove(book);
       _context.SaveChanges();
    }
    public void Reservar(Book book){
        _context.Entry(book).State = EntityState.Modified;
        _context.SaveChanges();
        
    }

    public List<Book> GetAll()
    {
        return _context.Book.Include(r => r.Nombre).ToList();
    }

    public Book? GetById(int id)
    {
        var book =  _context.Book.FirstOrDefault(m=> m.Id == id);
        return book;
    }

    public List<Book> QuerySearch(string str)
    {
        var query = from book in _context.Book select book; 
            if (!string.IsNullOrEmpty(str))
            {
                query = query.Where(x=> x.Nombre.ToLower().Contains(str.ToLower()) || x.Editorial.ToLower().Contains(str.ToLower()) || x.Año.ToString() == str);
            }

            var queryReady =  query.Include(b =>b.Autor).ToList();
        
        return queryReady;
    }

    public List<Categoria> Query(BookCreateViewModel viewmodel){
        var query = _context.Categoria.Where(x=> viewmodel.CategoriaIds.Contains(x.Id)).ToList();
        return query;
    }

    public void Update(Book book)
    {
        _context.Update(book);
        _context.SaveChanges();
    }
}