public class Autor{
    public int Id { get; set; }
    public string Nombre{get;set;}
    public string Apellido { get; set; }
    public int Edad { get; set; }
    public string Genero { get; set; } 

    public virtual List<Book> Books {get;set;}
}