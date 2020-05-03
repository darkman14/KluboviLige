using KluboviLige.Interfaces;
using KluboviLige.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KluboviLige.Controllers
{
    public class LigeController : ApiController
    {
        ILeagueRepository _repository { get; set; }

        public LigeController(ILeagueRepository repository)
        {
            _repository = repository;
        }

        //GET api/lige
        [Authorize]
        public IQueryable<League> Get()
        {
            return _repository.GetAll();
        }

        //GET api/lige/{id}
        [Authorize]
        public IHttpActionResult Get(int id)
        {
            var league = _repository.GetById(id);
            if (league == null)
            {
                return NotFound();
            }

            return Ok(league);
        }

        //GET api/lige?godina={vrednost}
        [Authorize]
        //[Route("api/lige?godina={vrednost}")]
        public IQueryable<League> GetByYear([FromUri]int godina)
        {
            return _repository.GetByYear(godina);
        }

        //GET api/najstarije
        [Authorize]
        [Route("api/najstarije")]
        public IQueryable<League> GetOldest()
        {
            return _repository.GetOldest();
        }

    }
}
