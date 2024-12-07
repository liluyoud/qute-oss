using System.ComponentModel;

namespace Qute.Rfb.Shared.Enums;

public enum FaixaEtaria
{
    [Description("Não Informado")]
    NaoInformado = 0,
    
    [Description("Menor de 18 anos")]
    Menor18 = 1,

    [Description("De 18 a 25 anos")]
    De18a25 = 2,

    [Description("De 26 a 30 anos")]
    De26a30 = 3,

    [Description("De 31 a 40 anos")]
    De31a40 = 4,

    [Description("De 41 a 50 anos")]
    De41a50 = 5,

    [Description("De 51 a 60 anos")]
    De51a60 = 6,

    [Description("De 61 a 70 anos")]
    De61a70 = 7,

    [Description("De 71 a 80 anos")]
    De71a80 = 8,

    [Description("Mais de 80 anos")]
    Maior80 = 9
}
