using DOOR.EF.Data;
using DOOR.EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text.Json;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http.Headers;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using DOOR.Server.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Numerics;
using DOOR.Shared.DTO;
using DOOR.Shared.Utils;
using DOOR.Server.Controllers.Common;
using static System.Collections.Specialized.BitVector32;

namespace DOOR.Server.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradeController : BaseController
    {
        public GradeController(DOOROracleContext DBcontext, OraTransMsgs _OraTransMsgs) : base(DBcontext, _OraTransMsgs)
        {
        }

        [HttpGet]
        [Route("GetGrade")]
        public async Task<IActionResult> GetGrade()
        {
            List<GradeDTO> lst = await _context.Grades
                .Select(sp => new GradeDTO
                {
                    Comments = sp.Comments,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    GradeCodeOccurrence = sp.GradeCodeOccurrence,
                    GradeTypeCode = sp.GradeTypeCode,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    NumericGrade = sp.NumericGrade,
                    SchoolId = sp.SchoolId,
                    SectionId = sp.SectionId,
                    StudentId = sp.StudentId,

                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetGrade/{_SchoolId}")]
        public async Task<IActionResult> GetGrade(int _SchoolId)
        {
            GradeDTO? lst = await _context.Grades
                .Where(x => x.SchoolId == _SchoolId)
                .Select(sp => new GradeDTO
                {
                    Comments = sp.Comments,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    GradeCodeOccurrence = sp.GradeCodeOccurrence,
                    GradeTypeCode = sp.GradeTypeCode,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    NumericGrade = sp.NumericGrade,
                    SchoolId = sp.SchoolId,
                    SectionId = sp.SectionId,
                    StudentId = sp.StudentId,

                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostGrade")]
        public async Task<IActionResult> PostGrade([FromBody] GradeDTO _GradeDTO)
        {
            try
            {
                Grade c = await _context.Grades.Where(x => x.SchoolId == _GradeDTO.SchoolId).FirstOrDefaultAsync();

                if (c == null)
                {
                    c = new Grade
                    {
                        Comments = _GradeDTO.Comments,
                        CreatedBy = _GradeDTO.CreatedBy,
                        CreatedDate = _GradeDTO.CreatedDate,
                        GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence,
                        GradeTypeCode = _GradeDTO.GradeTypeCode,
                        ModifiedBy = _GradeDTO.ModifiedBy,
                        ModifiedDate = _GradeDTO.ModifiedDate,
                        NumericGrade = _GradeDTO.NumericGrade,
                        SchoolId = _GradeDTO.SchoolId,
                        SectionId = _GradeDTO.SectionId,
                        StudentId = _GradeDTO.StudentId,
                    };
                    _context.Grades.Add(c);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }








        [HttpPut]
        [Route("PutGrade")]
        public async Task<IActionResult> PutGrade([FromBody] GradeDTO _GradeDTO)
        {
            try
            {
                Grade c = await _context.Grades.Where(x => x.SchoolId == _GradeDTO.SchoolId).FirstOrDefaultAsync();

                if (c != null)
                {
                    c.Comments = _GradeDTO.Comments;
                    c.CreatedBy = _GradeDTO.CreatedBy;
                    c.CreatedDate = _GradeDTO.CreatedDate;
                    c.GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence;
                    c.GradeTypeCode = _GradeDTO.GradeTypeCode;
                    c.ModifiedBy = _GradeDTO.ModifiedBy;
                    c.ModifiedDate = _GradeDTO.ModifiedDate;
                    c.NumericGrade = _GradeDTO.NumericGrade;
                    c.SchoolId = _GradeDTO.SchoolId;
                    c.SectionId = _GradeDTO.SectionId;
                    c.StudentId = _GradeDTO.StudentId;

                    _context.Grades.Update(c);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }


        [HttpDelete]
        [Route("DeleteGrade/{_SchoolId}")]
        public async Task<IActionResult> DeleteGrade(int _SchoolId)
        {
            try
            {
                Grade c = await _context.Grades.Where(x => x.SchoolId == _SchoolId).FirstOrDefaultAsync();

                if (c != null)
                {
                    _context.Grades.Remove(c);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }

    }
}
