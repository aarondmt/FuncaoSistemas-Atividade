using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Funcoes
{
    public static class Util
    {
        /// <summary>
        /// Remove caracteres não numéricos
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string RemoveNaoNumericos(string text)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
            string ret = reg.Replace(text, string.Empty);
            return ret;
        }

        /// <summary>
        /// Valida se um cpf é válido
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool ValidaCPF(string cpf)
        {
            //Remove formatação do número, ex: "123.456.789-01" vira: "12345678901"
            cpf = RemoveNaoNumericos(cpf);

            if (cpf.Length != 11)
                return false;

            int[] arrNumeroCpf = new int[11];

            for (int i = 0; i < 11; i++)
            {
                arrNumeroCpf[i] = int.Parse(cpf[i].ToString());
            }

            int v1 = 0;
            int v2 = 0;
            for (int i = 0, p = 10; i < arrNumeroCpf.Length - 2; i++, p--)
            {
                v1 += arrNumeroCpf[i] * p;
            }

            v1 = (v1 * 10) % 11;

            if (v1 == 10)
                v1 = 0;

            if (v1 != arrNumeroCpf[9])
                return false;

            for (int i = 0, p = 11; i < arrNumeroCpf.Length - 1; i++, p--)
            {
                v2 += arrNumeroCpf[i] * p;
            }

            v2 = (v2 * 10) % 11;

            if (v2 == 10)
                v2 = 0;

            if (v2 != arrNumeroCpf[10])
                return false;

            return true;
        }
    }
}