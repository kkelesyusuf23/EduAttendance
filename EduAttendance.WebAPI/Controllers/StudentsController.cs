using EduAttendance.WebAPI.Context;
using EduAttendance.WebAPI.Dtos;
using EduAttendance.WebAPI.Models;
using EduAttendance.WebAPI.Validators;
using FluentValidation.Results;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduAttendance.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class StudentsController(ApplicationDbContext dbContext) : ControllerBase
{
    //ApplicationDbContext _dbContext;
    //public StudentsController(ApplicationDbContext dbContext)
    //{
    //    _dbContext = dbContext; 

    //}
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        List<Student> students = await dbContext.Students.ToListAsync(cancellationToken);
        return Ok(students);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentDto request, CancellationToken cancellationToken)
    {
        //Student student = new()
        //{
        //    FirstName = request.FirstName,
        //    LastName = request.LastName,
        //    IdentityNumber = request.IdentityNumber,
        //    PhoneNumber = request.PhoneNumber,
        //    Email = request.Email
        //}; bunlar yerine Mapster kullanarak aşağıdaki gibi yazabiliriz.

        CreateStudentValidator validator = new();
        ValidationResult validation = validator.Validate(request);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors.Select(x => x.ErrorMessage));
        }


        var isIdentityNumberExists = await dbContext.Students.AnyAsync(x => x.IdentityNumber == request.IdentityNumber, cancellationToken);
        if (isIdentityNumberExists)
        {
            return BadRequest("Bu kimlik numarasına sahip bir öğrenci zaten kayıtlı");
        }

        Student student = request.Adapt<Student>();
        dbContext.Add(student);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok("Öğrenci kaydı başarıyla yamamlandı");
    }
}
