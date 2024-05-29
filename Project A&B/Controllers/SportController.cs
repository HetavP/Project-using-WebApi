using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_A_B.Data;
using Project_A_B.Models;

namespace Project_A_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportController : ControllerBase
    {
        private readonly SummerGamesContext _context;

        public SportController(SummerGamesContext context)
        {
            _context = context;
        }

        // GET: api/Sport
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SportDTO>>> GetSports()
        {
            var sportDTOs = await _context.Sports
                .Select(s=> new SportDTO
                {
                    ID = s.ID,
                    Code = s.Code,
                    Name = s.Name,
                    RowVersion = s.RowVersion
                }).ToListAsync();

            if (sportDTOs.Count() > 0 )
            {
                return sportDTOs;
            }
            else
            {
                return NotFound(new { message = "Error:  Sport not found."});
            }
        }

        //Get: api/Sport/inc - Include the Athlete Collection
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<SportDTO>>> GetSportsInc()
        {
            var sportDTOs = await _context.Sports
                .Include(s => s.Athletes)
                .Select(s=> new SportDTO
                {
                    ID = s.ID,
                    Code = s.Code,
                    Name = s.Name,
                    RowVersion = s.RowVersion,
                    Athletes = s.Athletes.Select(sAthlete => new AthleteDTO
                    {
                        ID = sAthlete.ID,
                        FirstName = sAthlete.FirstName,
                        MiddleName = sAthlete.MiddleName,
                        LastName = sAthlete.LastName,
                        AthleteCode = sAthlete.AthleteCode,
                        DOB = sAthlete.DOB,
                        Height = sAthlete.Height,
                        Weight = sAthlete.Weight,
                        Affiliation = sAthlete.Affiliation,
                        MediaInfo = sAthlete.MediaInfo,
                        Gender = sAthlete.Gender,
                        SportID = sAthlete.SportID,
                        ContingentID = sAthlete.ContingentID,
                        Contingent = new ContingentDTO
                        {
                            ID = sAthlete.Contingent.ID,
                            Code = sAthlete.Contingent.Code,
                            Name = sAthlete.Contingent.Name,
                        }
                    }).ToList()
                })
                .ToListAsync();

            if (sportDTOs.Count() > 0)
            {
                return sportDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: Sport not found." });
            }
        }

        // GET: api/Sport/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SportDTO>> GetSport(int id)
        {
            var sportDTO = await _context.Sports
                .Select(s=> new SportDTO
                {
                    ID = s.ID,
                    Code = s.Code,
                    Name = s.Name,
                    RowVersion = s.RowVersion
                })
                .FirstOrDefaultAsync(a => a.ID == id);

            if (sportDTO == null)
            {
                return NotFound(new { message = "Error: Sport not found." });
            }

            return sportDTO;
        }

        // GET: api/Sport/inc/5
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<SportDTO>> GetSportInc(int id)
        {
            var sportDTO = await _context.Sports
                .Include(s => s.Athletes)
                .Select(s=> new SportDTO
                {
                    ID = s.ID,
                    Code = s.Code,
                    Name = s.Name,
                    RowVersion = s.RowVersion,
                    Athletes = s.Athletes.Select(sAthlete => new AthleteDTO
                    {
                        ID = sAthlete.ID,
                        FirstName = sAthlete.FirstName,
                        MiddleName = sAthlete.MiddleName,
                        LastName = sAthlete.LastName,
                        AthleteCode = sAthlete.AthleteCode,
                        DOB = sAthlete.DOB,
                        Height = sAthlete.Height,
                        Weight = sAthlete.Weight,
                        Affiliation = sAthlete.Affiliation,
                        MediaInfo = sAthlete.MediaInfo,
                        Gender = sAthlete.Gender,
                        SportID = sAthlete.SportID,
                        ContingentID = sAthlete.ContingentID,
                        Contingent = new ContingentDTO
                        {
                            ID = sAthlete.Contingent.ID,
                            Code = sAthlete.Contingent.Code,
                            Name = sAthlete.Contingent.Name,
                        },
                    }).ToList(),
                })
                .FirstOrDefaultAsync(a => a.ID == id);

            if (sportDTO == null)
            {
                return NotFound(new { message = "Error: Sport not found." });
            }

            return sportDTO;

        }

        //// PUT: api/Sport/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSport(int id, Sport sport)
        //{
        //    if (id != sport.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(sport).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SportExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Sport
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Sport>> PostSport(Sport sport)
        //{
        //    _context.Sports.Add(sport);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetSport", new { id = sport.ID }, sport);
        //}

        //// DELETE: api/Sport/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSport(int id)
        //{
        //    var sport = await _context.Sports.FindAsync(id);
        //    if (sport == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Sports.Remove(sport);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool SportExists(int id)
        //{
        //    return _context.Sports.Any(e => e.ID == id);
        //}
    }
}
