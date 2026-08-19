namespace SchedulingManagement.Core.Entities.Abstractions;

internal abstract class EntidadeBase : Notifiable<NotificacaoFlunt>
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTimeOffset CriadoEm { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset AtualizadoEm { get; private set; } = DateTimeOffset.UtcNow;

    public void Atualizar()
    {
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    protected abstract void AplicarRegrasDeNegocio();

    protected void ValidarFusoHorarioIana(string fusoHorario, string nomePropriedade)
    {
        if (string.IsNullOrWhiteSpace(fusoHorario) ||
            !TimeZoneInfo.TryConvertIanaIdToWindowsId(fusoHorario, out _))
        {
            AddNotification(new NotificacaoFlunt(
                nomePropriedade,
                "O fuso horário deve ser um identificador IANA válido."));
        }
    }

    protected static string NormalizarTexto(string? valor) => valor?.Trim() ?? string.Empty;

    protected static string? NormalizarTextoOpcional(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
