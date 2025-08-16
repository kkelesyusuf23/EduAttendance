namespace EduAttendance.WebAPI.Dtos
{
    public class CreateStudentDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string IdentityNumber { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
