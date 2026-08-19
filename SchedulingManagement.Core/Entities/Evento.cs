using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities;

internal class Evento : EntidadeBase
{
    public Evento(
        Guid calendarioId,
        Guid organizadorId,
        string titulo,
        DateTimeOffset inicio,
        DateTimeOffset fim,
        string fusoHorario,
        bool diaInteiro = false,
        bool bloqueiaDisponibilidade = true,
        string visibilidade = "Default",
        string? descricao = null,
        string? local = null,
        string? url = null)
    {
        CalendarioId = calendarioId;
        OrganizadorId = organizadorId;
        Titulo = NormalizarTexto(titulo);
        Descricao = NormalizarTextoOpcional(descricao);
        Inicio = inicio.ToUniversalTime();
        Fim = fim.ToUniversalTime();
        FusoHorario = NormalizarTexto(fusoHorario);
        DiaInteiro = diaInteiro;
        BloqueiaDisponibilidade = bloqueiaDisponibilidade;
        Visibilidade = NormalizarTexto(visibilidade);
        Local = NormalizarTextoOpcional(local);
        Url = NormalizarTextoOpcional(url);

        AplicarRegrasDeNegocio();
    }

    public Guid CalendarioId { get; private set; }
    public Guid OrganizadorId { get; private set; }
    public string Titulo { get; private set; }
    public string? Descricao { get; private set; }
    public DateTimeOffset Inicio { get; private set; }
    public DateTimeOffset Fim { get; private set; }
    public string FusoHorario { get; private set; }
    public bool DiaInteiro { get; private set; }
    public bool BloqueiaDisponibilidade { get; private set; }
    public string Visibilidade { get; private set; }
    public string Estado { get; private set; } = "Confirmed";
    public int Versao { get; private set; } = 1;
    public string? Local { get; private set; }
    public string? Url { get; private set; }

    protected override void AplicarRegrasDeNegocio()
    {
        GarantirAssociacaoAoCalendario();
        GarantirAssociacaoAoOrganizador();
        GarantirFimPosteriorAoInicio();
        GarantirInstantesNormalizadosEmUtc();
        PreservarFusoHorarioOriginal();
        GarantirFusoHorarioIana();
        GarantirSemanticaDeDataParaEventoDeDiaInteiro();
    }

    private void GarantirAssociacaoAoCalendario()
    {
        if (CalendarioId == Guid.Empty)
        {
            AddNotification(new NotificacaoFlunt(
                nameof(CalendarioId),
                "O evento deve pertencer a um calendário."));
        }
    }

    private void GarantirAssociacaoAoOrganizador()
    {
        if (OrganizadorId == Guid.Empty)
        {
            AddNotification(new NotificacaoFlunt(
                nameof(OrganizadorId),
                "O evento deve possuir um organizador."));
        }
    }

    private void GarantirFimPosteriorAoInicio()
    {
        if (Fim <= Inicio)
        {
            AddNotification(new NotificacaoFlunt(
                nameof(Fim),
                "O fim do evento deve ser posterior ao início."));
        }
    }

    private void GarantirInstantesNormalizadosEmUtc()
    {
        if (Inicio.Offset != TimeSpan.Zero || Fim.Offset != TimeSpan.Zero)
        {
            AddNotification(new NotificacaoFlunt(
                nameof(Inicio),
                "Os instantes do evento devem estar normalizados em UTC."));
        }
    }

    private void PreservarFusoHorarioOriginal()
    {
        if (string.IsNullOrWhiteSpace(FusoHorario))
        {
            AddNotification(new NotificacaoFlunt(
                nameof(FusoHorario),
                "O fuso horário original do evento deve ser preservado."));
        }
    }

    private void GarantirFusoHorarioIana()
    {
        if (!string.IsNullOrWhiteSpace(FusoHorario))
        {
            ValidarFusoHorarioIana(FusoHorario, nameof(FusoHorario));
        }
    }

    private void GarantirSemanticaDeDataParaEventoDeDiaInteiro()
    {
        if (!DiaInteiro)
        {
            return;
        }

        if (Inicio.TimeOfDay != TimeSpan.Zero ||
            Fim.TimeOfDay != TimeSpan.Zero ||
            (Fim - Inicio).Ticks % TimeSpan.TicksPerDay != 0)
        {
            AddNotification(new NotificacaoFlunt(
                nameof(DiaInteiro),
                "Eventos de dia inteiro devem usar limites de dias completos em UTC."));
        }
    }
}
