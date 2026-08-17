using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities.Calendar;

internal class Calendar : EntityBase
{
    public Calendar(
        Guid ownerId,
        string name,
        string timeZone,
        string color = "#3B82F6",
        string visibility = "Default")
    {
        OwnerId = ownerId;
        Name = NormalizeText(name);
        TimeZone = NormalizeText(timeZone);
        Color = NormalizeText(color);
        Visibility = NormalizeText(visibility);

        ApplyBusinessRules();
    }

    public Guid OwnerId { get; private set; }
    public string Name { get; private set; }
    public string TimeZone { get; private set; }
    public string Color { get; private set; }
    public string Visibility { get; private set; }
    public bool IsArchived { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private void ApplyBusinessRules()
    {
        EnsureSingleLogicalOwner();
        EnsureIanaTimeZone();
    }

    private void EnsureSingleLogicalOwner()
    {
    }

    private void EnsureIanaTimeZone()
    {
    }
}
