public class BookCreateViewModel{
    public int Id { get; set; }
    public int AutorId {get;set;}
    public string Nombre{get;set;}
    public string Editorial { get; set; }
    public int Año { get; set; }
    public GeneroType Genero { get; set; } 

    public bool EstaReservado {get; set;} = false;

    public List<int> CategoriaIds{get;set;}

}