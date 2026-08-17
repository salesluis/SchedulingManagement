using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities.Identity;

internal class User : EntityBase
{
    public User(
        string name,
        string email,
        string timeZone,
        string locale = "pt-BR",
        string? phone = null)
    {
        Name = NormalizeText(name);
        Email = NormalizeText(email).ToLowerInvariant();
        Phone = NormalizeOptionalText(phone);
        TimeZone = NormalizeText(timeZone);
        Locale = NormalizeText(locale);

        ApplyBusinessRules();
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public string TimeZone { get; private set; }
    public string Locale { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private void ApplyBusinessRules()
    {
        EnsureIanaTimeZone();
    }

    private void EnsureIanaTimeZone()
    {
    }
}
