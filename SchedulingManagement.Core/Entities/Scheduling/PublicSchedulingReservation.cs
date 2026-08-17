using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities.Scheduling;

internal class PublicSchedulingReservation : EntityBase
{
    public PublicSchedulingReservation(
        Guid schedulingTypeId,
        Guid publicSchedulingPageId,
        Guid eventId,
        string customerName,
        DateTimeOffset start,
        DateTimeOffset end,
        bool requiresApproval = false,
        string? customerEmail = null,
        string? customerPhone = null)
    {
        SchedulingTypeId = schedulingTypeId;
        PublicSchedulingPageId = publicSchedulingPageId;
        EventId = eventId;
        CustomerName = NormalizeText(customerName);
        CustomerEmail = NormalizeOptionalText(customerEmail)?.ToLowerInvariant();
        CustomerPhone = NormalizeOptionalText(customerPhone);
        Start = start.ToUniversalTime();
        End = end.ToUniversalTime();
        Status = requiresApproval ? "Pending" : "Confirmed";

        ApplyBusinessRules();
    }

    public Guid SchedulingTypeId { get; private set; }
    public Guid PublicSchedulingPageId { get; private set; }
    public Guid EventId { get; private set; }
    public string CustomerName { get; private set; }
    public string? CustomerEmail { get; private set; }
    public string? CustomerPhone { get; private set; }
    public DateTimeOffset Start { get; private set; }
    public DateTimeOffset End { get; private set; }
    public string Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private void ApplyBusinessRules()
    {
        EnsureEventAssociation();
        EnsureInstantsAreNormalizedToUtc();
        EnsureEndIsAfterStart();
        DefineInitialStatusAccordingToApprovalPolicy();
    }

    private void EnsureEventAssociation()
    {
    }

    private void EnsureInstantsAreNormalizedToUtc()
    {
    }

    private void EnsureEndIsAfterStart()
    {
    }

    private void DefineInitialStatusAccordingToApprovalPolicy()
    {
    }
}
