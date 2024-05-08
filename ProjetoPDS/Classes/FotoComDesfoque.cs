/*
*	<copyright file="FotoComDesfoque" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/15/2024 7:27:17 PM</date>
*	<description></description>
**/

using Newtonsoft.Json;
using Python.Runtime;
using System.ComponentModel.DataAnnotations;

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
            PythonEngine.Initialize();
            dynamic sys = Py.Import("sys");
            sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");
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
        /// <summary>
        /// Retorna um encoding sozinho.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public byte[] addUtente(string pathToFile)
        {
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
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                var auxEncoding1 = JsonConvert.SerializeObject(encoding1);
                var auxEncoding2 = JsonConvert.SerializeObject(encoding2);
                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.compareEncoding;

                dynamic auxCompare = loadEncFunc(auxEncoding1, auxEncoding2);

                bool exists = false;
                string auxExist = auxCompare;
                if (auxExist == "True")
                    exists = true;
                return exists;
            }
        }

        /// <summary>
        /// Cria uma nova imagem que irá conter todas as caras identificadas.
        /// </summary>
        /// <param name="nomeFicheiro"></param>
        /// <param name="nomeDiretorio"></param>
        /// <param name="listaVerificados"></param>
        /// <returns></returns>
        public string MostrarIdentificados(string nomeFicheiro, string nomeDiretorio, List<UtenteIdentificado> listaVerificados, out string nomeFicheiroAux)
        {
            string imagemVerificarPath = "";
            dynamic facilRecMod = Py.Import("recognition");
            dynamic loadEncFunc = facilRecMod.censure_results_identified;

            bool firstIteration = false;
            string auxNomeFicheiro = Path.GetFileNameWithoutExtension(nomeFicheiro);
            Random numerosRandom = new Random();
            foreach (UtenteIdentificado u in listaVerificados)
            {
                if (!firstIteration)
                {
                    dynamic execFunc = loadEncFunc(auxNomeFicheiro, nomeDiretorio, u.PrimeiraCor, u.SegundaCor, u.TerceiraCor, u.Left, u.Top, u.Right, u.Bottom);
                    imagemVerificarPath = execFunc.ToString();
                    firstIteration = true;
                }
                else
                {
                    dynamic execFunc = loadEncFunc(auxNomeFicheiro, imagemVerificarPath, u.PrimeiraCor, u.SegundaCor, u.TerceiraCor, u.Left, u.Top, u.Right, u.Bottom);
                }
            }
            nomeFicheiroAux = Path.GetFileNameWithoutExtension(imagemVerificarPath);
            return imagemVerificarPath;
        }
        /// <summary>
        /// Cria uma nova imagem que irá conter todas as caras não identificadas.
        /// </summary>
        /// <param name="nomeDiretorio"></param>
        /// <param name="listaPorVerificar"></param>
        /// <returns></returns>
        public string MostrarNaoIdentificados(string nomeFicheiro, string nomeDiretorio, List<UtenteVerificar> listaPorVerificar)
        {
            string imagemVerificarPath = "";
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.censure_results_non_identified;
                bool firstIteration = false;
                string auxNomeFicheiro = Path.GetFileNameWithoutExtension(nomeFicheiro);
                Random numerosRandom = new Random();
                foreach (UtenteVerificar u in listaPorVerificar)
                {
                    int primeiraCor = numerosRandom.Next(256);
                    int segundaCor = numerosRandom.Next(256);
                    int terceiraCor = numerosRandom.Next(256);
                    u.PrimeiraCor = primeiraCor;
                    u.SegundaCor = segundaCor;
                    u.TerceiraCor = terceiraCor;
                    if (!firstIteration)
                    {
                        dynamic execFunc = loadEncFunc(auxNomeFicheiro, nomeDiretorio, primeiraCor, segundaCor, terceiraCor, u.Left, u.Top, u.Right, u.Bottom);
                        imagemVerificarPath = execFunc.ToString();
                        firstIteration = true;
                    }
                    else
                    {
                        dynamic execFunc = loadEncFunc(auxNomeFicheiro, imagemVerificarPath, primeiraCor, segundaCor, terceiraCor, u.Left, u.Top, u.Right, u.Bottom);
                    }
                }
            }
            return imagemVerificarPath;
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
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.censure_results_click;
                string fotoDesfocadaPath = "";
                foreach (UtenteVerificar u in listaUtentes)
                {
                    dynamic execFunc = loadEncFunc(nomeFicheiro, nomeDiretorio, posX, posY, u.Left, u.Top, u.Right, u.Bottom);
                    fotoDesfocadaPath = execFunc.ToString();
                }
                return fotoDesfocadaPath;
            }
            #endregion
            #endregion
        }
        /// <summary>
        /// Verifica se o clique numa certa imagem está de acordo com o reconhecimento da cara.
        /// Caso sim cria um novo utente com os dados e retorna esse mesmo utente para depois adicionar na base de dados.
        /// </summary>
        /// <param name="nomeFic"></param>
        /// <param name="nomeDir"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="listaUtentes"></param>
        /// <param name="val"></param>
        /// <param name="sala"></param>
        /// <param name="aut"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        public Utente AdicionarUtenteClick(string nomeFic, string nomeDir, int posX, int posY, List<UtenteVerificar> listaUtentes, string val, string sala, int aut, string nome, int corP, int corS, int corT)
        {
            
            dynamic facilRecMod = Py.Import("recognition");
            dynamic loadEncFunc = facilRecMod.add_utente_click;
            bool existe = false;
            foreach (UtenteVerificar u in listaUtentes)
            {
                if (!(u.PrimeiraCor == corP && u.SegundaCor == corS && u.TerceiraCor == corT))
                    continue;
                dynamic execFunc = loadEncFunc(nomeFic, nomeDir, posX, posY, u.Left, u.Top, u.Right, u.Bottom);
                string auxResult = execFunc.ToString();
                if (auxResult == "True")
                {
                    existe = true;
                    break;
                }
            }
            if (existe)
            {
                Utente novoUtente = new Utente();
                novoUtente.Nome = nome;
                novoUtente.Sala = sala;
                novoUtente.Valencia = val;
                novoUtente.Autorizacao = aut;
                return novoUtente;
            }
            return null;
        }

        /// <summary>
        /// Adiciona um utente a base de dados, conforme os dados introduzidos no website. Como não irá ter nome para a pessoa inicialmente
        /// Verificamos a cor à qual ela está associada e criamos o novo utente.
        /// </summary>
        /// <param name="listaUtentes"></param>
        /// <param name="val"></param>
        /// <param name="sala"></param>
        /// <param name="aut"></param>
        /// <param name="nome"></param>
        /// <param name="corP"></param>
        /// <param name="corS"></param>
        /// <param name="corT"></param>
        /// <returns></returns>
        public Dictionary<Utente, Encoding> AdicionarUtenteBaseDados(List<UtenteVerificar> listaUtentes, string val, string sala, int aut, string nome, int corP, int corS, int corT)
        {
            Dictionary<Utente, Encoding> novoListaUtentes = new Dictionary<Utente, Encoding>();
            bool existe = false;

            foreach (UtenteVerificar u in listaUtentes)
            {
                if (!(u.PrimeiraCor == corP && u.SegundaCor == corS && u.TerceiraCor == corT))
                    continue;
                existe = true;
                if (existe)
                {
                    Encoding novoEncoding = new Encoding();
                    Utente novoUtente = new Utente();
                    novoUtente.Nome = nome;
                    novoUtente.Sala = sala;
                    novoUtente.Valencia = val;
                    novoUtente.Autorizacao = aut;
                    novoEncoding = u.Encoding;
                    novoListaUtentes.Add(novoUtente, novoEncoding);
                }
            }
            return novoListaUtentes;
        }

        /// <summary>
        /// Função para alterar dados de um utilizador na base de dados.
        /// </summary>
        /// <param name="listaUtentes"></param>
        /// <param name="val"></param>
        /// <param name="sala"></param>
        /// <param name="aut"></param>
        /// <param name="nome"></param>
        /// <param name="corP"></param>
        /// <param name="corS"></param>
        /// <param name="corT"></param>
        /// <returns></returns>
        public Utente EditarUtenteBaseDados(List<UtenteVerificar> listaUtentes, string val, string sala, int aut, string nome, int corP, int corS, int corT)
        {

            bool editado = false;

            if (editado)
            {

            }
            return null;
        }
        ~FotoComDesfoque()
        {
            PythonEngine.Shutdown();
        }
    }
}