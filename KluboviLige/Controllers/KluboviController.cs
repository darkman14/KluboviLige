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
    public class KluboviController : ApiController
    {
        IClubRepository _repository { get; set; }

        public KluboviController(IClubRepository repository)
        {
            _repository = repository;
        }

        //GET api/klubovi
        public IQueryable<ClubDTO> Get()
        {
            return _repository.GetAllDto();
        }

        //GET api/klubovi/{id}
        [Authorize]
        public IHttpActionResult Get(int id)
        {
            var club = _repository.GetById(id);

            if (club == null)
            {
                return NotFound();
            }

            return Ok(club);

        }

        [Authorize]
        public IHttpActionResult Post(Club club)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(club);
            return CreatedAtRoute("DefaultApi", new { id = club.Id }, club);
        }

        [Authorize]
        public IHttpActionResult Put(int id, Club club)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != club.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(club);
            }
            catch
            {
                return BadRequest();
            }


            return Ok(club);
        }

        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            var club = _repository.GetById(id);

            if (club == null)
            {
                return NotFound();
            }

            _repository.Delete(club);
            return Ok();
        }

        [Authorize]
        [Route("api/pretraga")]
        public IQueryable<ClubDTO> Post([FromBody]Years years)
        {
            return _repository.GetByYears(years.Start, years.End);
        }

    }
}
