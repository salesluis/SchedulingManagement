using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities;

internal class Calendario : EntidadeBase
{
    public Calendario(
        Guid proprietarioId,
        string nome,
        string fusoHorario,
        string cor = "#3B82F6",
        string visibilidade = "Default")
    {
        ProprietarioId = proprietarioId;
        Nome = NormalizarTexto(nome);
        FusoHorario = NormalizarTexto(fusoHorario);
        Cor = NormalizarTexto(cor);
        Visibilidade = NormalizarTexto(visibilidade);

        AplicarRegrasDeNegocio();
    }

    public Guid ProprietarioId { get; private set; }
    public string Nome { get; private set; }
    public string FusoHorario { get; private set; }
    public string Cor { get; private set; }
    public string Visibilidade { get; private set; }
    public bool EstaArquivado { get; private set; }

    protected override void AplicarRegrasDeNegocio()
    {
        GarantirProprietarioLogicoUnico();
        GarantirNomeInformado();
        GarantirFusoHorarioIana();
    }

    private void GarantirProprietarioLogicoUnico()
    {
        if (ProprietarioId == Guid.Empty)
        {
            AddNotification(new NotificacaoFlunt(
                nameof(ProprietarioId),
                "O calendário deve possuir um proprietário."));
        }
    }

    private void GarantirNomeInformado()
    {
        if (string.IsNullOrWhiteSpace(Nome))
        {
            AddNotification(new NotificacaoFlunt(
                nameof(Nome),
                "O calendário deve possuir um nome."));
        }
    }

    private void GarantirFusoHorarioIana()
    {
        ValidarFusoHorarioIana(FusoHorario, nameof(FusoHorario));
    }
}
