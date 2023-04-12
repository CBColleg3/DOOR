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
using static Duende.IdentityServer.Models.IdentityResources;

namespace DOOR.Server.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : BaseController
    {
        public StudentController(DOOROracleContext DBcontext, OraTransMsgs _OraTransMsgs) : base(DBcontext, _OraTransMsgs)
        {
        }

        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> GetStudent()
        {
            List<StudentDTO> lst = await _context.Students
                .Select(sp => new StudentDTO
                {
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    Employer = sp.Employer,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    Phone = sp.Phone,
                    RegistrationDate = sp.RegistrationDate,
                    Salutation = sp.Salutation,
                    SchoolId = sp.SchoolId,
                    StreetAddress = sp.StreetAddress,
                    StudentId = sp.StudentId,
                    Zip = sp.Zip,

                }).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetStudent/{_StudentID}")]
        public async Task<IActionResult> GetStudent(int _StudentID)
        {
            StudentDTO? lst = await _context.Students
                .Where(x => x.StudentId == _StudentID)
                .Select(sp => new StudentDTO
                {
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    Employer = sp.Employer,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    Phone = sp.Phone,
                    RegistrationDate = sp.RegistrationDate,
                    Salutation = sp.Salutation,
                    SchoolId = sp.SchoolId,
                    StreetAddress = sp.StreetAddress,
                    StudentId = sp.StudentId,
                    Zip = sp.Zip,

                }).FirstOrDefaultAsync();
            return Ok(lst);
        }

        [HttpPost]
        [Route("PostStudent")]
        public async Task<IActionResult> PostStudent([FromBody] StudentDTO _StudentDTO)
        {
            try
            {
                Student c = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId).FirstOrDefaultAsync();

                if (c == null)
                {
                    c = new Student
                    {
                        CreatedBy = _StudentDTO.CreatedBy,
                        CreatedDate = _StudentDTO.CreatedDate,
                        Phone = _StudentDTO.Phone,
                        ModifiedDate = _StudentDTO.ModifiedDate,
                        Employer = _StudentDTO.Employer,
                        FirstName = _StudentDTO.FirstName,
                        LastName = _StudentDTO.LastName,
                        ModifiedBy = _StudentDTO.ModifiedBy,
                        RegistrationDate = _StudentDTO.RegistrationDate,
                        Salutation = _StudentDTO.Salutation,
                        SchoolId = _StudentDTO.SchoolId,
                        StreetAddress = _StudentDTO.StreetAddress,
                        StudentId = _StudentDTO.StudentId,
                        Zip = _StudentDTO.Zip,

                    };
                    _context.Students.Add(c);
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
        [Route("PutStudent")]
        public async Task<IActionResult> PutStudent([FromBody] StudentDTO _StudentDTO)
        {
            try
            {
                Student s = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId).FirstOrDefaultAsync();

                if (s != null)
                {
                    s.CreatedBy = _StudentDTO.CreatedBy;
                    s.CreatedDate = _StudentDTO.CreatedDate;
                    s.Phone = _StudentDTO.Phone;
                    s.ModifiedDate = _StudentDTO.ModifiedDate;
                    s.Employer = _StudentDTO.Employer;
                    s.FirstName = _StudentDTO.FirstName;
                    s.LastName = _StudentDTO.LastName;
                    s.ModifiedBy = _StudentDTO.ModifiedBy;
                    s.RegistrationDate = _StudentDTO.RegistrationDate;
                    s.Salutation = _StudentDTO.Salutation;
                    s.SchoolId = _StudentDTO.SchoolId;
                    s.StreetAddress = _StudentDTO.StreetAddress;
                    s.StudentId = _StudentDTO.StudentId;
                    s.Zip = _StudentDTO.Zip;

                    _context.Students.Update(s);
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
        [Route("DeleteStudent/{_StudentID}")]
        public async Task<IActionResult> DeleteStudent(int _StudentID)
        {
            try
            {
                Student s = await _context.Students.Where(x => x.StudentId == _StudentID).FirstOrDefaultAsync();

                if (s != null)
                {
                    _context.Students.Remove(s);
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
