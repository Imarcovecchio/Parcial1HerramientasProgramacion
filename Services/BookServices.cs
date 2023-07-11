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

    public List<Categoria> QueryCategorias(BookCreateViewModel bookView){
        var existingCategoriaIds = _context.Categoria.Select(x => x.Id).ToList();
        var validCategoriaIds = bookView.CategoriaIds.Intersect(existingCategoriaIds).ToList();
        
        var categorias = _context.Categoria.Where(x => validCategoriaIds.Contains(x.Id)).ToList();
        

        return categorias;
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
    public void Devolver(Book book){
        book.EstaReservado =false;
        _context.SaveChanges();
    }
    

    public List<Book> GetAll()
    {
        return _context.Book.Include(r => r.Nombre).ToList();
    }

    public List<Book> GetBooksAvailable(){
        return _context.Book.Where(x=> !x.EstaReservado).ToList();
    }
    public List<Book> GetBooksUnAvailable(){
        return _context.Book.Where(x=> x.EstaReservado).ToList();
    }

    public List<Book> GetBooksReserved(){
        return _context.Book.Where(x=> x.EstaReservado).ToList();
    }

    public Book? GetById(int id)
    {
        return _context.Book
        .Include(b => b.Categorias) 
        .FirstOrDefault(b => b.Id == id);
    }

    public List<Book> QuerySearch(string str)
    {
        var query = from book in _context.Book select book; 
        if (!string.IsNullOrEmpty(str))
        {
            if (Enum.TryParse(str, true, out GeneroType generoType))
            {
                query = query.Where(x => x.Genero == generoType);
            }
            else if (int.TryParse(str,out int year))
            {
                query = query.Where(x => x.Año == year);
            }
            else
            {
                query = query.Where(x => x.Nombre.ToLower().Contains(str.ToLower()) || 
                                        x.Editorial.ToLower().Contains(str.ToLower()) || 
                                        x.Año.ToString() == str);
            }
        }

        var queryReady = query.Include(b => b.Autor).ToList();
        
        return queryReady;
    }
    public List<Categoria> Query(BookCreateViewModel viewmodel){
        var query = _context.Categoria.Where(x=> viewmodel.CategoriaIds.Contains(x.Id)).ToList();
        return query;
    }

    public void Update(Book book)
    {
        var existingBook = _context.Book
        .Include(b => b.Categorias) 
        .FirstOrDefault(b => b.Id == book.Id);

        if (existingBook != null)
        {
            existingBook.AutorId = book.AutorId;
            existingBook.Nombre = book.Nombre;
            existingBook.Editorial = book.Editorial;
            existingBook.Año = book.Año;
            existingBook.Genero = book.Genero;
            existingBook.EstaReservado = book.EstaReservado;

            var categoriesToRemove = existingBook.Categorias
                .Where(c => !book.Categorias.Any(bc => bc.Id == c.Id))
                .ToList();
            foreach (var category in categoriesToRemove)
            {
                existingBook.Categorias.Remove(category);
            }

            var categoriesToAdd = book.Categorias
                .Where(bc => !existingBook.Categorias.Any(c => c.Id == bc.Id))
                .ToList();
            foreach (var category in categoriesToAdd)
            {
                existingBook.Categorias.Add(category);
            }
            _context.SaveChanges();
        }

    }
        public List<Book> GetByCategoriaId(int categoriaId)
        {
           return _context.Book
                 .Include(b => b.Autor)
                 .Where(b => b.Categorias.Any(c => c.Id == categoriaId))
                 .ToList();
        }
}