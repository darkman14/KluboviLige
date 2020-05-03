using KluboviLige.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KluboviLige.Interfaces
{
    public interface IClubRepository
    {
        IQueryable<Club> GetAll();
        IQueryable<ClubDTO> GetAllDto();
        IQueryable<ClubDTO> GetByYears(int? y1, int? y2);
        Club GetById(int id);
        void Add(Club club);
        void Update(Club club);
        void Delete(Club club);

    }
}
