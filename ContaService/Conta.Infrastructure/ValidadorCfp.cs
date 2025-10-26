public static class ValidadorCfp
{
    public static bool Validar(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += (tempCpf[i] - '0') * multiplicadores1[i];

        int resto = soma % 11;
        int primeiroDigito = resto < 2 ? 0 : 11 - resto;

        tempCpf += primeiroDigito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += (tempCpf[i] - '0') * multiplicadores2[i];

        resto = soma % 11;
        int segundoDigito = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{primeiroDigito}{segundoDigito}");
    }
}
