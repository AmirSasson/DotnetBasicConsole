﻿// See https://aka.ms/new-console-template for more information
internal interface IPersonsService
{
    Task<Person> GetById(int id);
    Task<Person> Upsert(Person person);


}