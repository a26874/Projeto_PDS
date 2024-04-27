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
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text;

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
            set { numUtentes = value; }
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
        public List<UtenteIdentificado> IdentificarUtentes(string pathToFile)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");

                sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.recognition;

                dynamic auxEncoding = loadEncFunc(pathToFile);


                Dictionary<string, byte[]> encodingDict = new Dictionary<string, byte[]>();

                foreach (PyObject key in auxEncoding)
                {
                    string faceLocation = key.ToString();
                    string base64encoding = auxEncoding[key].ToString();
                    byte[] encoding = Convert.FromBase64String(base64encoding);
                    encodingDict.Add(faceLocation, encoding);
                }

                List<UtenteIdentificado> utentesIdentificados = new List<UtenteIdentificado>();
                foreach (var key in encodingDict)
                {
                    string[] parts = key.Key.Trim('(', ')').Split(',');
                    UtenteIdentificado novoUtente = new UtenteIdentificado();
                    novoUtente.Top = int.Parse(parts[0].Trim());
                    novoUtente.Right = int.Parse(parts[1].Trim());
                    novoUtente.Bottom = int.Parse(parts[2].Trim());
                    novoUtente.Left = int.Parse(parts[3].Trim());
                    novoUtente.Encoding = new Encoding();
                    novoUtente.Encoding.encoding = key.Value;
                    utentesIdentificados.Add(novoUtente);
                }
                return utentesIdentificados;
            }
        }
        /// <summary>
        /// Retorna um encoding sozinho.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public byte[] addUtente(string pathToFile)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");

                sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.singleRecognition;

                dynamic auxEncoding = loadEncFunc(pathToFile);

                byte[] encoding = auxEncoding;
                return encoding;
            }
        }
        /// <summary>
        /// Verifica se o encoding já existe na base de dados.
        /// </summary>
        /// <param name="encoding1"></param>
        /// <param name="encoding2"></param>
        /// <returns></returns>
        public bool verificarEncoding(byte[] encoding1, byte[] encoding2)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.compareEncoding;

                dynamic auxCompare = loadEncFunc(encoding1, encoding2);

                bool exists = false;
                string auxExist = auxCompare;
                if (auxExist == "True")
                    exists = true;
                return exists;
            }
        }
        /// <summary>
        /// Cria uma nova imagem que irá conter todas as caras não identificadas.
        /// </summary>
        /// <param name="nomeDiretorio"></param>
        /// <param name="listaPorVerificar"></param>
        /// <returns></returns>
        public string MostrarNaoIdentificados(string nomeFicheiro, string nomeDiretorio, List<UtenteVerificar> listaPorVerificar)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.censure_results_2;
                bool firstIteration = false;
                string imagemVerificarPath = "";
                string auxNomeFicheiro = Path.GetFileNameWithoutExtension(nomeFicheiro);
                foreach (UtenteVerificar u in listaPorVerificar)
                {
                    if (!firstIteration)
                    {
                        dynamic execFunc = loadEncFunc(auxNomeFicheiro, nomeDiretorio, u.Left, u.Top, u.Right, u.Bottom);
                        imagemVerificarPath = execFunc.ToString();
                        firstIteration = true;
                    }
                    else
                    {
                        dynamic execFunc = loadEncFunc(auxNomeFicheiro, imagemVerificarPath, u.Left, u.Top, u.Right, u.Bottom);
                    }
                }
                return imagemVerificarPath;
            }
        }
        /// <summary>
        /// Aplica desfoque baseado no click.
        /// </summary>
        /// <param name="nomeFicheiro"></param>
        /// <param name="nomeDiretorio"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="listaUtentes"></param>
        /// <returns></returns>
        public string AplicarDesfoque(string nomeFicheiro, string nomeDiretorio, int posX, int posY, List<UtenteVerificar> listaUtentes)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.censure_results_click;
                string fotoDesfocadaPath = "";
                foreach(UtenteVerificar u in listaUtentes)
                {
                    dynamic execFunc = loadEncFunc(nomeFicheiro, nomeDiretorio, posX, posY, u.Left, u.Top, u.Right, u.Bottom);
                    fotoDesfocadaPath = execFunc.ToString();
                }


                return fotoDesfocadaPath;
            }
            #endregion
            #endregion
        }
    }
}