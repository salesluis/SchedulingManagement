using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities.Scheduling;

internal class SchedulingType : EntityBase
{
    public SchedulingType(
        Guid userId,
        Guid calendarId,
        string name,
        TimeSpan duration,
        bool requiresApproval = false,
        TimeSpan? bufferBefore = null,
        TimeSpan? bufferAfter = null,
        string? location = null,
        string? url = null)
    {
        UserId = userId;
        CalendarId = calendarId;
        Name = NormalizeText(name);
        Duration = duration;
        RequiresApproval = requiresApproval;
        BufferBefore = bufferBefore ?? TimeSpan.Zero;
        BufferAfter = bufferAfter ?? TimeSpan.Zero;
        Location = NormalizeOptionalText(location);
        Url = NormalizeOptionalText(url);

        ApplyBusinessRules();
    }

    public Guid UserId { get; private set; }
    public Guid CalendarId { get; private set; }
    public string Name { get; private set; }
    public TimeSpan Duration { get; private set; }
    public bool RequiresApproval { get; private set; }
    public TimeSpan BufferBefore { get; private set; }
    public TimeSpan BufferAfter { get; private set; }
    public string? Location { get; private set; }
    public string? Url { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private void ApplyBusinessRules()
    {
        EnsureSufficientSlotDuration();
        RespectAvailabilityBuffers();
        DefineConfirmationPolicy();
    }

    private void EnsureSufficientSlotDuration()
    {
    }

    private void RespectAvailabilityBuffers()
    {
    }

    private void DefineConfirmationPolicy()
    {
    }
}
