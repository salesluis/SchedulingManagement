using SchedulingManagement.Core.Entities.Abstractions;

namespace SchedulingManagement.Core.Entities;

internal class Profissional : EntidadeBase
{
    public string Nome { get; private set; } = string.Empty;
    public int Indice { get; private set; }

    protected override void AplicarRegrasDeNegocio()
    {
    }
}
