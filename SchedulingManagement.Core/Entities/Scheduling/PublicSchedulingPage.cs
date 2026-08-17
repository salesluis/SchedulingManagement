using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities.Scheduling;

internal class PublicSchedulingPage : EntityBase
{
    public PublicSchedulingPage(
        Guid userId,
        Guid schedulingTypeId,
        string slug,
        string timeZone)
    {
        UserId = userId;
        SchedulingTypeId = schedulingTypeId;
        Slug = NormalizeText(slug).ToLowerInvariant();
        TimeZone = NormalizeText(timeZone);

        ApplyBusinessRules();
    }

    public Guid UserId { get; private set; }
    public Guid SchedulingTypeId { get; private set; }
    public string Slug { get; private set; }
    public string TimeZone { get; private set; }
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
