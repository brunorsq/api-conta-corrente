using System.Text.RegularExpressions;

namespace ContaCorrente.Domain._Shared
{
    public class Cpf
    {
        public string? Numero { get; }

        public Cpf(string? numero)
        {
            if (!string.IsNullOrEmpty(numero)) 
            {
                Numero = UnformatCpf(numero);

                if (!IsValidCpf(Numero))
                    throw new DomainException("CPF inválido.", "INVALID_DOCUMENT");
            }            
        }

        private static bool IsValidCpf(string cpf)
        {
            return cpf.Length == 11 && cpf.All(char.IsDigit);
        }
        private static string UnformatCpf(string cpf)
        {
            return Regex.Replace(cpf, @"\D", "");
        }
    }
}
