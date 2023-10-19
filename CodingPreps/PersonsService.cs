﻿// See https://aka.ms/new-console-template for more information
using CodingPreps;

internal class PersonsService : IPersonsService
{
    private readonly IPersonsRepository _repo;

    public PersonsService(IPersonsRepository repo)
    {
        _repo = repo;
    }

    public Task<Person> GetById(int id)
    {
        return _repo.Get(id);
    }

    public Task<Person> Upsert(Person person)
    {
        return _repo.Save(person);
    }
}