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
}
