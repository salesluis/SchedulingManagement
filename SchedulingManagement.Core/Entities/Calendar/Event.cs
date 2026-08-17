using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities.Calendar;

internal class Event : EntityBase
{
    public Event(
        Guid calendarId,
        Guid organizerId,
        string title,
        DateTimeOffset start,
        DateTimeOffset end,
        string timeZone,
        bool isAllDay = false,
        bool blocksAvailability = true,
        string visibility = "Default",
        string? description = null,
        string? location = null,
        string? url = null)
    {
        CalendarId = calendarId;
        OrganizerId = organizerId;
        Title = NormalizeText(title);
        Description = NormalizeOptionalText(description);
        Start = start.ToUniversalTime();
        End = end.ToUniversalTime();
        TimeZone = NormalizeText(timeZone);
        IsAllDay = isAllDay;
        BlocksAvailability = blocksAvailability;
        Visibility = NormalizeText(visibility);
        Location = NormalizeOptionalText(location);
        Url = NormalizeOptionalText(url);

        ApplyBusinessRules();
    }

    public Guid CalendarId { get; private set; }
    public Guid OrganizerId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset Start { get; private set; }
    public DateTimeOffset End { get; private set; }
    public string TimeZone { get; private set; }
    public bool IsAllDay { get; private set; }
    public bool BlocksAvailability { get; private set; }
    public string Visibility { get; private set; }
    public string Status { get; private set; } = "Confirmed";
    public int Version { get; private set; } = 1;
    public string? Location { get; private set; }
    public string? Url { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private void ApplyBusinessRules()
    {
        EnsureCalendarAssociation();
        EnsureEndIsAfterStart();
        EnsureInstantsAreNormalizedToUtc();
        PreserveOriginalTimeZone();
        EnsureIanaTimeZone();
        EnsureDateSemanticsForAllDayEvent();
    }

    private void EnsureCalendarAssociation()
    {
    }

    private void EnsureEndIsAfterStart()
    {
    }

    private void EnsureInstantsAreNormalizedToUtc()
    {
    }

    private void PreserveOriginalTimeZone()
    {
    }

    private void EnsureIanaTimeZone()
    {
    }

    private void EnsureDateSemanticsForAllDayEvent()
    {
    }
}
