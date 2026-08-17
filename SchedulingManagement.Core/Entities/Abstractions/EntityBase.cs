namespace SchedulingManagement.Core.Entities.Abstractions;

internal abstract class EntityBase() : Notifiable<FluntNotification>
{
    protected EntityBase(params Contract<FluntNotification>[] contracts)
        : this()
    {
        foreach (var contract in contracts)
        {
            AddNotifications(contract);
        }
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    protected static string NormalizeText(string? value) => value?.Trim() ?? string.Empty;

    protected static string? NormalizeOptionalText(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
