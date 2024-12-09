namespace Qute.Shared.Extensions;

public static class StringExtensions
{
    public static int GetCnpjSimples(this string valor)
    {
        var strArray = valor.ToCharArray();
        var cnpj = "";
        foreach (var caracter in strArray)
        {
            if (char.IsDigit(caracter))
                cnpj += caracter;
        }
        if (cnpj.Length < 8)
            cnpj = cnpj.PadLeft(8, '0');
        cnpj = cnpj.Substring(0, 8);
        return int.Parse(cnpj);
    }

    public static (int cnpj, int ordem, int dv) GetCnpj(this string valor)
    {
        var strArray = valor.ToCharArray();
        var cnpj = "";
        foreach (var caracter in strArray)
        {
            if (char.IsDigit(caracter))
                cnpj += caracter;
        }
        if (cnpj.Length < 14)
            cnpj = cnpj.PadLeft(14, '0');
        var id = int.Parse(cnpj.Substring(0, 8));
        var ordem = int.Parse(cnpj.Substring(8, 4));
        var dv = int.Parse(cnpj.Substring(12, 2));
        return (id, ordem, dv);
    }
}
