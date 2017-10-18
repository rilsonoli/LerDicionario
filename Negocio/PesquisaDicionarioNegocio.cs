using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilitario;

namespace Negocio
{
    public class PesquisaDicionarioNegocio
    {
        /// <summary>
        /// Busca a posição da palavra no serviço
        /// </summary>
        /// <param name="aPalavra"></param>
        /// <param name="aRange"></param>
        /// <param name="aUrl"></param>
        /// <returns></returns>
        public Tuple<long?, long> BuscarPosicaoDicionario(string aPalavra, int? aRange, string aUrl)
        {

            string url = !string.IsNullOrWhiteSpace(aUrl) ? aUrl : "http://testes.ti.lemaf.ufla.br/api/Dicionario";
            aPalavra = aPalavra.Trim();
            Tuple<long?, long> retorno = new Tuple<long?, long>(null, 0);

            try
            {   // Validando a url
                Utilitario.JSONHelper.ValidaURL(aUrl);
            }
            catch (Exception)
            {
                throw;
            }

            // Se a palavra não começa com caracter especial, vai fazer a busca por range
            if (Regex.IsMatch(aPalavra.Substring(0, 1).ToUpper(), ("^[ABCDEFGHIJKLMNOPQRSTUVWXYZÀÁÂÈÉÊÌÍÎÒÓÔÙÚÛ]")))
            {
                retorno = BuscaPalavraOrdem(aPalavra, aRange, url);
            }
            else
            {
                // A palavra começa com um caractere especial, vai fazer a busca sequencial
                retorno = BuscaPalavraSequencial(aPalavra, url);
            }

            return retorno;

        }

        /// <summary>
        /// Realiza a busca considerando a ordem da primeira letra da palavra pulando ranges
        /// </summary>
        /// <param name="aPalavra"></param>
        /// <param name="aRange"></param>
        /// <param name="aUrl"></param>
        /// <returns></returns>
        private Tuple<long?, long> BuscaPalavraOrdem(string aPalavra, int? aRange, string aUrl)
        {
            string strJSON = string.Empty;
            int i = 0;
            int j = aRange.HasValue ? aRange.Value : 10000;
            int divisor = 10;
            long gatinhos = 0;


            try
            {
                while (aPalavra.ToUpper() != strJSON.ToUpper() && i < 10000000)
                {
                    try
                    {
                        // Busca a palavra pela posição no serviço 
                        strJSON = JSONHelper.GetJSONString(String.Format(aUrl + "/{0}", i)).Replace("\"", "").Replace("\\", "").Trim();
                        gatinhos++;
                        if (aPalavra.ToUpper() == strJSON.ToUpper())
                            break;
                    }
                    catch (Exception)
                    {
                        // A busca chegou ao fim 
                        if (j == 1)
                            break;
                        // volta ao range  
                        i = i - j;
                        // verificando se já é o mínimo
                        divisor = j < 10 ? 1 : divisor;
                        // diminui o range
                        j = j / divisor;

                    }

                    // Compara se a palavra buscada começa com a mesma letra da palavra que está buscando a posição
                    int ret = aPalavra.Substring(0, 1).ToUpper().CompareTo(strJSON.Substring(0, 1).ToUpper());

                    // A letra inicial da palavra buscada é maior que da palavra que está buscando a posição
                    if (ret == -1 && j > 1)
                    {
                        // volta ao range 
                        i = i - j;
                        // verificando se já é o mínimo
                        divisor = j < 10 ? 1 : divisor;
                        //diminui o range
                        j = j / divisor;
                    }
                    // Se já é a mesma letra
                    else if (ret == 0 && j > 1)
                    {
                        // Se não está no range inicial, volta o range
                        i = i > j ? i - j : i;
                        // diminui o range
                        j = j > divisor ? j / divisor : 1;
                    }

                    i = i + j;                    

                }

                if (aPalavra.ToUpper() == strJSON.ToUpper())
                    return new Tuple<long?, long>(i, gatinhos);
                else
                    return new Tuple<long?, long>(null, gatinhos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Realiza a busca sequencial no serviço
        /// </summary>
        /// <param name="aPalavra"></param>
        /// <param name="aUrl"></param>
        /// <returns></returns>
        private Tuple<long?, long> BuscaPalavraSequencial(string aPalavra, string aUrl)
        {
            string strJSON = string.Empty;
            int i = 0;
            long gatinhos = 0;

            // Realiza a busca gulosa
            while (aPalavra.ToUpper() != strJSON.ToUpper() && i < 10000000)
            {
                try
                {
                    // Busca a palavra pela posição no serviço 
                    strJSON = JSONHelper.GetJSONString(String.Format(aUrl + "/{0}", i)).Replace("\"", "").Replace("\\", "");
                }
                catch (Exception)
                {
                    break;
                }

                i++;
            }
            gatinhos = i;

            if (aPalavra.ToUpper() == strJSON.ToUpper())
                return new Tuple<long?, long>(i - 1, gatinhos);
            else
                return new Tuple<long?, long>(null, gatinhos);
        }
    }
}
