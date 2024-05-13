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
    /// <summary>
    /// Classe para trabalhar com foto e aplicar desfoque e identificação de utentes.
    /// </summary>
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
            //Aqui é definido a localização do DLL de python.
            Runtime.PythonDLL = "C:\\Users\\marco\\AppData\\Local\\Programs\\Python\\Python311\\python311.dll";
            //Não me lembro disto, mas para ter uma variavel igual a outra deve ser useless.
            PythonEngine.PythonPath = PythonEngine.PythonPath;
            //Inicializa o python
            PythonEngine.Initialize();
            //Isto é feito para dizer onde está o ficheiro de python, neste caso é o recognition (aqui é apenas a pasta).
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
            //Como lá em cima o folder do script de python foi definido para uma certa pasta, aqui só é necessário dar append do que
            //está lá, neste caso o ficheiro em si.
            dynamic facilRecMod = Py.Import("recognition");
            //É dado load do método que está no ficheiro, neste caso "recognition"
            dynamic loadEncFunc = facilRecMod.recognition;

            //Aqui recebe o encoding que é retornado do resultado da função, tanto nesta função como em todas as outras que usam encodings
            //É recebida a lista de encodings, que no ficheiro python esse encoding é convertido para binário e de seguida para uma string
            //base64, assim será mais fácil de interpretar no C# pelo que pesquisei.
            dynamic auxEncoding = loadEncFunc(pathToFile);

            //É criado um dicionário para guardar encodings e a localizações de caras.
            Dictionary<string, byte[]> encodingDict = new Dictionary<string, byte[]>();


            //Por cada objecto de python na variável auxEncoding, transformamos a key que é sempre a localização da cara (uma tuple de 4 valores)
            //para string. Também é criado uma nova variável para o valor do encoding recebido para aquela localização.
            foreach (PyObject key in auxEncoding)
            {
                string faceLocation = key.ToString();
                string base64encoding = auxEncoding[key].ToString();
                //Aqui converte-se novamente para um array de bytes, conforme a string base64.
                byte[] encoding = Convert.FromBase64String(base64encoding);
                encodingDict.Add(faceLocation, encoding);
            }

            //Criado uma nvoa lista de utentes que foram identificados. Não sei se é o melhor approach, porque depois uso esta classe para outra função
            //Para já resulta.
            List<UtenteIdentificado> utentesIdentificados = new List<UtenteIdentificado>();
            //Aqui percorremos também o dicionário que criamos acima, é divido a key que neste caso é a localização da cara nas partes necessárias
            //Neste caso são 4. Por exemplo é recebido desta forma se me lembro corretamente (180,200,100,210) neste caso ficaria
            //parts[0] = 180, parts[1] = 200, etc... A partir daqui é criado um novo utente, onde dizemos as localizações da cara, bottom, left... etc
            //Também damos add do encoding a esse utente.
            //No final é retornada a lista.
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
        /// Verifica se o encoding já existe na base de dados.
        /// </summary>
        /// <param name="encoding1"></param>
        /// <param name="encoding2"></param>
        /// <returns></returns>
        public bool verificarEncoding(byte[] encoding1, byte[] encoding2)
        {
            //Aqui é usado o GIL que é o interpretador global de python pelo que pesquisei. Acho que dá conflito quando é executado 2 vezes 
            //Seguidas. Deste genero, por exemplo é usado o py.gil numa outra função anterior a esta, em principio tudo daria certo e de seguida
            //Usariamos esta função, ao chegar novamente ao py.gil tem alturas que ele "falha" e nada acontece. Até que tem funções que retirei
            //Isto do py.gil. Tirando isto esta função apenas chama uma outra função no python para comparar 2 encodings.
            using (Py.GIL())
            {
                //Este append do caminho da pasta onde o ficheiro python já está feito na inicialização da classe.
                //dynamic sys = Py.Import("sys");
                //sys.path.append(@"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\FaceRecognition");

                //Convertemos ambos os encodings para json, assim é mais facil descodificar no python.
                var auxEncoding1 = JsonConvert.SerializeObject(encoding1);
                var auxEncoding2 = JsonConvert.SerializeObject(encoding2);
                //Aqui é so dar novamente o nome do ficheiro e a função que queremos utilizar.
                dynamic facilRecMod = Py.Import("recognition");
                dynamic loadEncFunc = facilRecMod.compareEncoding;

                //Aqui recebe como string se é verdade ou falso, não arranjei maneira de retornar o "True" or "False" porque aqui, depois
                //não estou a conseguir converter esse objecto python em bool do c#
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
            //Aqui é feito o mesmo processo para as funções de python.
            string imagemVerificarPath = "";
            dynamic facilRecMod = Py.Import("recognition");
            dynamic loadEncFunc = facilRecMod.censure_results_identified;

            //Obtemos o nome do ficheiro sem a extensão, irá ser necessário para criar uma imagem auxiliar
            bool firstIteration = false;
            string auxNomeFicheiro = Path.GetFileNameWithoutExtension(nomeFicheiro);
            //Aqui é usado random para criar valores entre 0 e 255 para atribuir uma "cor" a cada utente identificado, para ser possivel
            //saber qual dos utentes é.
            Random numerosRandom = new Random();
            foreach (UtenteIdentificado u in listaVerificados)
            {
                //Agora que vejo a ordem está "errada" devia ser se for a primeira iteração faz isto, se não faz o resto, mas dá a mesma cena
                //É so trocar de false para true na inicialização.
                //Basicamente aqui é executada a função censure_results_identified, o nome não é o mais indicado, porque apenas desenha os quadrados.
                //Envia o nome do ficheiro e o diretório, ambos são necessários para depois escrever a nova imagem.
                //É sempre recebido o caminho da nova imagem, como string.
                //Caso já seja depois da primeira iteração, em vez de passarmos a imagem original pelo "nomeDiretorio" passamos a imagem auxiliar
                //imagemVerificarPath, que já foi "reescrita" e já tem quadrados desenhados à volta de utentes.
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
            //É dado um out desta variável, porque como esta função é sempre executada primeiro, se a imagem realmente for escrita com novos quadrados (identificações)
            //Vai ser necessário path do ficheiro que contém as pessoas identificadas, para que depois ao executar a função de não identificados
            //Já contenha o resultado desta imagem.
            nomeFicheiroAux = Path.GetFileNameWithoutExtension(imagemVerificarPath);
            return imagemVerificarPath;
        }
        /// <summary>
        /// Cria uma nova imagem que irá conter todas as caras não identificadas.
        /// </summary>
        /// <param name="nomeFicheiro"></param>
        /// <param name="nomeDiretorio"></param>
        /// <param name="listaPorVerificar"></param>
        /// <returns></returns>
        public string MostrarNaoIdentificados(string nomeFicheiro, string nomeDiretorio, List<UtenteVerificar> listaPorVerificar)
        {
            //Esta função é igual à função acima, apenas é gerado novas cores para cada um dos utentes que estão para verificar 
            //(Utentes que não estão na base de dados, são utentes por verificar).
            string imagemVerificarPath = "";

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
            return imagemVerificarPath;
        }
        /// <summary>
        /// Aplica desfoque.
        /// </summary>
        /// <param name="fotoOriginal"></param>
        /// <param name="nomeFotoFicheiro"></param>
        /// <param name="absolutePath"></param>
        /// <param name="listaUtentes"></param>
        /// <returns></returns>
        public string AplicarDesfoque(string fotoOriginal, string nomeFotoFicheiro, string absolutePath, List<UtenteVerificar> listaUtentes)
        {
            //Esta função vai ser a próxima a ser reescrita. Basicamente inicialmente ela apenas conforme a posição clicada na imagem no site
            //Iria ver se estava dentro dos limites da cara no python e dar blur caso estivesse dentro da posição da cara.
            dynamic facilRecMod = Py.Import("recognition");
            dynamic loadEncFunc = facilRecMod.censure_results_utente;
            string fotoDesfocadaPath = "";
            string auxNomeFicheiro = Path.GetFileNameWithoutExtension(nomeFotoFicheiro);
            bool firstIteration = true;
            foreach (UtenteVerificar u in listaUtentes)
            {
                if (firstIteration)
                {
                    dynamic execFunc = loadEncFunc(auxNomeFicheiro, fotoOriginal, u.Left, u.Top, u.Right, u.Bottom);
                    fotoDesfocadaPath = execFunc.ToString();
                    firstIteration = false;
                }
                else
                {
                    dynamic execFunc = loadEncFunc(auxNomeFicheiro, fotoDesfocadaPath, u.Left, u.Top, u.Right, u.Bottom);
                    fotoDesfocadaPath = execFunc.ToString();
                }
            }

            return fotoDesfocadaPath;
        }
            #endregion
            #endregion
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
            //Esta função trata de adicionar um utente à base de dados, ela irá receber do controller o nome da valencia, da sala, da autorização, do nome, e as cores.
            //E a lista de utentes, por verificar.
            Dictionary<Utente, Encoding> novoListaUtentes = new Dictionary<Utente, Encoding>();
            bool existe = false;

            //Por cada um dos utentes na lista verifica se a cor é a correspondente, caso não seja avança para o prox.
            //Caso exista, cria um novo encoding e um novo utente, dá append das variáveis necessárias para todos os campos de cada classe.
            //No final adiciona-se apenas ao dicionário, assim será mais fácil no controller percorrer cada utente que vai ser adicionado e o seu encoding correspondente.
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


        //Criei um deconstrutor, para tentar corrigir aquele erro acima do py.gil, mas penso que isto não faz diferença
        ~FotoComDesfoque()
        {
            PythonEngine.Shutdown();
        }
    }
}