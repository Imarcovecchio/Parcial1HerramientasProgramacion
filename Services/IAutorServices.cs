public interface IAutorServices{
    void Create(Autor aut);
    void Update(Autor aut);
    void Delete(Autor aut);

    List<Autor> QuerySearch(string str);

    List<Autor>GetAll();
    Autor? GetById(int id);

}