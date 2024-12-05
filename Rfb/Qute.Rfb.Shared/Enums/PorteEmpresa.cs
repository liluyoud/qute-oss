using System.ComponentModel;

namespace Qute.Rfb.Shared.Enums;

public enum PorteEmpresa
{
    [Description("Microempresa (ME)")]
    MicroEmpresa = 1,

    [Description("Empresa de pequeno porte (EPP)")]
    PequenoPorte = 3,

    [Description("Demais (grande porte)")]
    GrandePorte = 5
}
