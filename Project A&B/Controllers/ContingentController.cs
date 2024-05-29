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
    public class ContingentController : ControllerBase
    {
        private readonly SummerGamesContext _context;

        public ContingentController(SummerGamesContext context)
        {
            _context = context;
        }

        // GET: api/Contingent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContingentDTO>>> GetContingents()
        {
            var contingentDTOs = await _context.Contingents
                .Select(c => new ContingentDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion
                })
                .ToListAsync();

            if (contingentDTOs.Count() > 0)
            {
                return contingentDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: Contingent not found." });
            }
        }

        //Get: api/Contingent/inc - Include the Athlete Collection
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<ContingentDTO>>> GetContingentInc()
        {
            var contingentDTOs = await _context.Sports
                .Include(c => c.Athletes)
                .Select(c => new ContingentDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion,
                    Athletes = c.Athletes.Select(cAthlete => new AthleteDTO
                    {
                        ID = cAthlete.ID,
                        FirstName = cAthlete.FirstName,
                        MiddleName = cAthlete.MiddleName,
                        LastName = cAthlete.LastName,
                        AthleteCode = cAthlete.AthleteCode,
                        DOB = cAthlete.DOB,
                        Height = cAthlete.Height,
                        Weight = cAthlete.Weight,
                        Affiliation = cAthlete.Affiliation,
                        MediaInfo = cAthlete.MediaInfo,
                        Gender = cAthlete.Gender,
                        SportID = cAthlete.SportID,
                        Sport = new SportDTO
                        {
                            ID = cAthlete.Sport.ID,
                            Code = cAthlete.Sport.Code,
                            Name = cAthlete.Sport.Name
                        },
                        ContingentID = cAthlete.ContingentID
                    }).ToList()
                })
                .ToListAsync();

            if (contingentDTOs.Count() > 0)
            {
                return contingentDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: Contingent not found." });
            }
        }


        // GET: api/Contingent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContingentDTO>> GetContingent(int id)
        {
            var contingentDTO = await _context.Contingents
                .Select(c=> new ContingentDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion
                })
                .FirstOrDefaultAsync(a => a.ID == id);

            if (contingentDTO == null)
            {
                return NotFound(new { message = "Error: Contingent not found." });
            }

            return contingentDTO;
        }

        // GET: api/Contingent/inc/5
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<ContingentDTO>> GetContingentInc(int id)
        {
            var contingentDTO = await _context.Sports
                .Include(c => c.Athletes)
                .Select(c => new ContingentDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion,
                    Athletes = c.Athletes.Select(cAthlete => new AthleteDTO
                    {
                        ID = cAthlete.ID,
                        FirstName = cAthlete.FirstName,
                        MiddleName = cAthlete.MiddleName,
                        LastName = cAthlete.LastName,
                        AthleteCode = cAthlete.AthleteCode,
                        DOB = cAthlete.DOB,
                        Height = cAthlete.Height,
                        Weight = cAthlete.Weight,
                        Affiliation = cAthlete.Affiliation,
                        MediaInfo = cAthlete.MediaInfo,
                        Gender = cAthlete.Gender,
                        SportID = cAthlete.SportID,
                        Sport = new SportDTO
                        {
                            ID = cAthlete.Sport.ID,
                            Code = cAthlete.Sport.Code,
                            Name = cAthlete.Sport.Name
                        },
                        ContingentID = cAthlete.ContingentID
                    }).ToList()
                })
                .FirstOrDefaultAsync(a => a.ID == id);

            if (contingentDTO == null)
            {
                return NotFound(new { message = "Error: Contingent not found." });
            }

            return contingentDTO;

        }

        //// PUT: api/Contingent/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutContingent(int id, Contingent contingent)
        //{
        //    if (id != contingent.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(contingent).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ContingentExists(id))
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

        //// POST: api/Contingent
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Contingent>> PostContingent(Contingent contingent)
        //{
        //    _context.Contingents.Add(contingent);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetContingent", new { id = contingent.ID }, contingent);
        //}

        //// DELETE: api/Contingent/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteContingent(int id)
        //{
        //    var contingent = await _context.Contingents.FindAsync(id);
        //    if (contingent == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Contingents.Remove(contingent);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ContingentExists(int id)
        //{
        //    return _context.Contingents.Any(e => e.ID == id);
        //}
    }
}
