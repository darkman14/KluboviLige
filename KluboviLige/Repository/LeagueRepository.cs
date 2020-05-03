using KluboviLige.Interfaces;
using KluboviLige.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;


namespace KluboviLige.Repository
{
    public class LeagueRepository : ILeagueRepository, IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Add(League league)
        {
            db.Leagues.Add(league);
            db.SaveChanges();
        }

        public void Delete(League league)
        {
            db.Leagues.Remove(league);
            db.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<League> GetAll()
        {
            return db.Leagues;
        }

        public League GetById(int id)
        {
            return db.Leagues.Find(id);
        }

        public IQueryable<League> GetByYear(int year)
        {
            return db.Leagues.Where(leg=>leg.YearOfEst>year).OrderByDescending(leg=>leg.YearOfEst);
        }

        public IQueryable<League> GetOldest()
        {
            return db.Leagues.OrderBy(leg=>leg.YearOfEst).Take(2);
        }

        public void Update(League league)
        {
            db.Entry(league).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;

            }
        }
    }
}