using Parcial.Data;

public class CategoriaServices : ICategoriaSerivces
{
    private readonly AutorContext _context;
    public CategoriaServices(AutorContext context){
        _context = context;
    }
    
    public void Create(Categoria cat)
    {
        _context.Add(cat);
        _context.SaveChanges();
    }


    public void Delete(Categoria cat)
    {
       _context.Remove(cat);
       _context.SaveChanges();
    }

    public void Update(Categoria cat)
    {
        _context.Update(cat);
        _context.SaveChanges();
    }

    public List<Categoria> QuerySearch(string str){
        var query= from categoria in _context.Categoria select categoria;
            if(!string.IsNullOrEmpty(str)){
                query = query.Where(x => x.Nombre.ToLower().Contains(str.ToLower()) || x.Descripcion.ToLower().Contains(str.ToLower()));
            }
        return query.ToList();
    }

    public List<Categoria> ObtenerCategoriasDisponibles(){

        var categorias = _context.Categoria.ToList();
        return categorias;
    }
    public List<Book> ObtenerBooks(){
        var libros = _context.Book.ToList();
        return libros;
    }

    public Categoria? GetCategoria(int id)
    {
        var categoria = _context.Categoria.FirstOrDefault(m=> m.Id== id);
        return categoria;
    }
}