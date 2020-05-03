using KluboviLige.Interfaces;
using KluboviLige.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AutoMapper.QueryableExtensions;

namespace KluboviLige.Repository
{
    public class ClubRepository : IClubRepository, IDisposable
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public void Add(Club club)
        {
            db.Clubs.Add(club);
            db.SaveChanges();
        }

        public void Delete(Club club)
        {
            db.Clubs.Remove(club);
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

        public IQueryable<Club> GetAll()
        {
            return db.Clubs;
        }

        public IQueryable<ClubDTO> GetAllDto()
        {
            return db.Clubs.Include(leg => leg.League).ProjectTo<ClubDTO>();
        }

        public Club GetById(int id)
        {
            return db.Clubs.Find(id);
        }

        public IQueryable<ClubDTO> GetByYears(int? y1, int? y2)
        {
            return db.Clubs.Where(c => c.YearOfEst > y1 && c.YearOfEst < y2).OrderBy(c => c.YearOfEst).ProjectTo<ClubDTO>();
        }

        public void Update(Club club)
        {
            db.Entry(club).State = EntityState.Modified;

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