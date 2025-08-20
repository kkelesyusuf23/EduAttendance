using EduAttendance.WebAPI.Dtos;
using FluentValidation;

namespace EduAttendance.WebAPI.Validators;

public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentValidator()
    {
        RuleFor(p => p.FirstName)
            .MinimumLength(3).WithMessage("Öğrenci adı en az 3 karakter olabilir.");
        RuleFor(p => p.LastName)
            .MinimumLength(3).WithMessage("Öğrenci soyadı en az 3 karakter olabilir.");
        RuleFor(p => p.PhoneNumber)
            .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Öğrenci telefon numarası geçerli bir formatta olmalıdır.");
        RuleFor(p => p.Email)
            .EmailAddress()
            .WithMessage("Öğrenci e-posta adresi geçerli bir formatta olmalıdır.");
        RuleFor(p => p.IdentityNumber)
            .Must(IsValidTckn).WithMessage("Öğrenci kimlik numarası geçerli bir TCKN formatında olmalıdır.");
    }



    public static bool IsValidTckn(string? tckn)
    {
        if (string.IsNullOrWhiteSpace(tckn))
            return false;

        tckn = tckn.Trim();

        // 11 hane ve sadece rakam
        if (tckn.Length != 11 || !tckn.All(char.IsDigit))
            return false;

        // İlk hane 0 olamaz
        if (tckn[0] == '0')
            return false;

        // Rakamları int dizisine çevir
        Span<int> d = stackalloc int[11];
        for (int i = 0; i < 11; i++)
            d[i] = tckn[i] - '0';

        int sumOdd = d[0] + d[2] + d[4] + d[6] + d[8];   // 1,3,5,7,9
        int sumEven = d[1] + d[3] + d[5] + d[7];          // 2,4,6,8

        int digit10 = ((sumOdd * 7) - sumEven) % 10;
        if (digit10 < 0) digit10 += 10; // teorik olarak negatif mod koruması

        if (d[9] != digit10)
            return false;

        int sumFirst10 = 0;
        for (int i = 0; i < 10; i++)
            sumFirst10 += d[i];

        int digit11 = sumFirst10 % 10;
        if (d[10] != digit11)
            return false;

        return true;
    }
}
