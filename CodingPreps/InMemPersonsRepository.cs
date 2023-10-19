using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodingPreps
{
    internal class InMemPersonsRepository : IPersonsRepository
    {
        Dictionary<long, Person> _db = new Dictionary<long, Person>();

        public Task<Person> Get(int id)
        {
            _db.TryGetValue(id, out Person person);
            return Task.FromResult< Person >( person!);
        }

        public Task<IEnumerable<Person>> Get()
        {
            var persons = _db.Values;

            return Task.FromResult(persons.AsEnumerable());
        }

        public Task<Person> Save(Person person)
        {
            _db[person.Id] = person;

            return Task.FromResult(person);
        }
    }
}
