namespace CodingPreps;

internal interface IPersonsRepository
{
    Task<Person> Get(int id);
    Task<Person> Save(Person person);
}