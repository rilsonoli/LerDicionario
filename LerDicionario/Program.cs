using Negocio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LerDicionario
{
    class Program
    {
        static void Main(string[] args)
        {
            new BuscarPosicao(args).RealizarBusca();
        }
               
    }

    public class BuscarPosicao
    {
        public string[] Parametros { get; set; }        

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="args">Parâmetros de entrada</param>
        public BuscarPosicao(string[] args)
        {
            Parametros = args;
        }

        /// <summary>
        /// Método que realiza a busca de posição no dicionário
        /// </summary>
        public void RealizarBusca()
        {
            try
            {
                if (ValdiarParametros(Parametros))
                {
                    Console.WriteLine("Buscando posição...");
                    // Busca a posição da palavra passada como parâmetro
                    var tuplePosicao = new PesquisaDicionarioNegocio().BuscarPosicaoDicionario(Parametros[0], BuscarRangeConfig(), BuscarUrlConfig());

                    if (tuplePosicao.Item1.HasValue)
                        Console.WriteLine($" Palavra { Parametros[0] } encontrada! Posição: { tuplePosicao.Item1.Value }. Foram mortos {tuplePosicao.Item2} gatinhos. ");
                    else
                        Console.WriteLine($" A palavra { Parametros[0] } não foi encontrada! Foram mortos {tuplePosicao.Item2} gatinhos.");
                }
                else
                    Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine($" Não foi possível realizar a busca pois ocorreu o seguinte erro: {e.Message}");
            }
        }
        
        
        /// <summary>
        /// Valida os parâmetros de entrada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public bool ValdiarParametros(string[] parametros)
        {
            try
            {
                // Não foi fornecido nenhum parâmetro
                if (parametros.Count() == 0)
                {
                    Console.WriteLine("Por favor, insira uma palavra para buscar a posição no dicionário.");
                    Parametros = Console.ReadLine().Split(' ');

                    RealizarBusca();
                    return false;
                }
                else
                // Foi fornecido mais que uma palavra
                if (parametros.Count() > 1)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Só é possível pesquisar uma palavra por vez.");
                    Console.WriteLine("Por favor, insira uma palavra para buscar a posição no dicionário.");
                    Parametros = Console.ReadLine().Split(' ');

                    RealizarBusca();
                    return false;
                }
                else
                    return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possível realizar a busca pois ocorreu o seguinte erro: {e.Message}");
                return false;
            }

        }

        /// <summary>
        /// Busca o range para a busca alfabética
        /// </summary>
        /// <returns></returns>
        public int? BuscarRangeConfig()
        {
            try
            {
                return Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("range"));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Busca a url do serviço no app.config
        /// </summary>
        /// <returns></returns>
        public string BuscarUrlConfig()
        {
            try
            {
                return System.Configuration.ConfigurationSettings.AppSettings.Get("urlServico");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
