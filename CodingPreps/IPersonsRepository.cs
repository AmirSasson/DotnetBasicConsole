namespace CodingPreps;

internal interface IPersonsRepository
{
    Task<Person> Get(int id);
    Task<IEnumerable<Person>> Get();
    Task<Person> Save(Person person);
}