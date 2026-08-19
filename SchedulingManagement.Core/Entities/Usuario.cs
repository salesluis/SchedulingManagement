using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities.Identity;

internal class Usuario : EntidadeBase
{
    public Usuario(
        string nome,
        string email,
        string fusoHorario,
        string localidade = "pt-BR",
        string? telefone = null)
    {
        Nome = NormalizarTexto(nome);
        Email = NormalizarTexto(email).ToLowerInvariant();
        FusoHorario = NormalizarTexto(fusoHorario);
        Localidade = NormalizarTexto(localidade);
        Telefone = NormalizarTextoOpcional(telefone);

        AplicarRegrasDeNegocio();
    }

    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string FusoHorario { get; private set; }
    public string Localidade { get; private set; }
    public string? Telefone { get; private set; }
    public bool EstaAtivo { get; private set; } = true;

    protected override void AplicarRegrasDeNegocio()
    {
        GarantirFusoHorarioIana();
    }

    private void GarantirFusoHorarioIana()
    {
        ValidarFusoHorarioIana(FusoHorario, nameof(FusoHorario));
    }
}
