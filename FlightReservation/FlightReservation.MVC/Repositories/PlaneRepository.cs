﻿using FlightReservation.MVC.Context;
using FlightReservation.MVC.Models;
using System.Collections;

namespace FlightReservation.MVC.Repositories;

public sealed class PlaneRepository(AppDbContext context)
{
    public IEnumerable<Plane> GetAll()
    {
        return context.Set<Plane>().OrderBy(p=> p.Name).ToList();
    }

    public void Add(Plane plane)
    {
        context.Set<Plane>().Add(plane);
        context.SaveChanges();
    }

    public bool CheckTailNumberExist(string tailNumber)
    {
        return context.Set<Plane>().Any(p => p.TailNumber == tailNumber);//Belirtilen kuyruk numarasına sahip bir Plane nesnesinin veritabanında olup olmadığını kontrol eder.
    }

    public void RemoveById(Guid id)
    {
        Plane? plane = context.Set<Plane>().Find(id);
        if(plane is not null)
        {
            context.Remove(plane);
            context.SaveChanges();
        }
    }
}
