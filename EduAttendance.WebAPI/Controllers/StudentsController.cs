using EduAttendance.WebAPI.Context;
using EduAttendance.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduAttendance.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class StudentsController(ApplicationDbContext dbContext) :ControllerBase
    {
        //ApplicationDbContext _dbContext;
        //public StudentsController(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext; 

        //}
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            List<Student> students =await dbContext.Students.ToListAsync(cancellationToken);
            return Ok(students);
        }
    }
}
