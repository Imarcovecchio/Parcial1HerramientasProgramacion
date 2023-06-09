using Parcial.Data;
using Microsoft.EntityFrameworkCore;

public class AutorServices : IAutorServices
{
    private readonly AutorContext _context;
    public AutorServices(AutorContext context){
        _context = context;
    }
    public void Create(Autor aut)
    {
        _context.Add(aut);
        _context.SaveChanges();
    }


    public void Delete(Autor aut)
    {
       _context.Remove(aut);
       _context.SaveChanges();
    }

    public List<Autor> GetAll()
    {
        return _context.Autor.Include(r => r.Nombre).ToList();
    }

    public Autor? GetById(int id){

        var autor =  _context.Autor.FirstOrDefault(m=> m.Id == id);
        return autor;
    }
    public List<Autor> QuerySearch(string str){
        var query = from autor in _context.Autor select autor;
            if(!string.IsNullOrEmpty(str)){
                query = query.Where(x => x.Nombre.ToLower().Contains(str.ToLower()) || x.Apellido.ToLower().Contains(str.ToLower()) || x.Edad.ToString() == str);
            }
        return query.ToList();
    }



    public void Update(Autor aut)
    {
        _context.Update(aut);
        _context.SaveChanges();
    }
}