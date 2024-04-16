/*
*	<copyright file="FotoComDesfoque" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/15/2024 7:27:17 PM</date>
*	<description></description>
**/

using Python.Runtime;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ProjetoPDS.Classes
{
    public class FotoComDesfoque
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private string urlFoto;
        private int numUtentesIdentificados;
        private List<UtenteIdentificado> listaIdentificados;
        private int numUtentes;
        private int numUtentesProcessados;
        private StatusFoto estadoFoto;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        /// <summary>
        /// Construtor estático
        /// </summary>
        static FotoComDesfoque()
        {
            Runtime.PythonDLL = "C:\\Users\\marco\\AppData\\Local\\Programs\\Python\\Python311\\python311.dll";
            PythonEngine.PythonPath = PythonEngine.PythonPath;
        }
        /// <summary>
        /// Construtor por defeito.
        /// </summary>
        public FotoComDesfoque()
        {

        }

        #endregion

        #region PROPRIEDADES

        /// <summary>
        /// Obter o id da foto desfocada
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// Obter o url da foto desfocada
        /// </summary>
        public string URL
        {
            get { return urlFoto; }
            set { urlFoto = value; }
        }
        /// <summary>
        /// Obter o numero de utentes identificados
        /// </summary>
        public int NumUtentesIdentificados
        {
            get { return numUtentesIdentificados; }
            set { numUtentesIdentificados = value; }
        }
        /// <summary>
        /// Obter a lista de utentes identificados.
        /// </summary>
        public List<UtenteIdentificado> ObterIdentificados
        {
            get { return listaIdentificados.ToList(); }
        }
        /// <summary>
        /// Obter o numero de utentes.
        /// </summary>
        public int NumUtentes
        {
            get { return numUtentes; }
            set { numUtentes = value;}
        }
        /// <summary>
        /// Obter o numero de utentes processados
        /// </summary>
        public int NumUtentesProcessados
        {
            get { return numUtentesProcessados; }
            set { numUtentesProcessados = value; }
        }

        #endregion

        #region OPERADORES

        #endregion

        #region OVERRIDES

        #endregion

        #region OUTROS METODOS
        /// <summary>
        /// Obtem o encoding de algum utente.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public byte[] IdentificarUtentes(string pathToFile)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");

                // Append the directory containing teste.py to the Python path
                sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.recognition;

                dynamic auxEncoding = loadEncFunc(pathToFile);


                var values = JsonConvert.DeserializeObject<Dictionary<int, byte[]>>(auxEncoding);

                List<Encoding> listaEncodings = new List<Encoding>();
                foreach(int a in auxEncoding)
                {
                   
                }
                byte[] encoding = (byte[])auxEncoding;

                return encoding;
            }
        }

        #endregion

        #endregion
    }
}