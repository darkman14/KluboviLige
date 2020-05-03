using KluboviLige.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KluboviLige.Interfaces
{
    public interface ILeagueRepository
    {
        IQueryable<League> GetAll();
        League GetById(int id);
        IQueryable<League> GetByYear(int year);
        IQueryable<League> GetOldest();
        void Add(League league);
        void Update(League league);
        void Delete(League league);

    }
}
