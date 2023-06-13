public interface ICategoriaSerivces{
    void Create(Categoria categoria);
    void Update(Categoria categoria);
    void Delete(Categoria categoria);

    List<Categoria> QuerySearch(string str);
    List<Categoria> ObtenerCategoriasDisponibles();
    List<Book> ObtenerBooks();
    Categoria? GetCategoria(int id);
    

}