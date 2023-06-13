public class Categoria{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }

    public virtual List<Book> Libros { get; set; }

}