/*
*	<copyright file="UtenteVerificar" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 2:30:58 PM</date>
*	<description></description>
**/

namespace ProjetoPDS.Classes
{
    /// <summary>
    /// Classe para utentes que estão por verificar.
    /// </summary>
    public class UtenteVerificar
    {
        #region ATRIBUTOS
        private string nomeUtz;
        private int cordTop;
        private int cordBottom;
        private int cordRight;
        private int cordLeft;
        private int primeiraCor;
        private int segundaCor;
        private int terceiraCor;
        private int autorizacao;
        private Encoding encoding;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        public UtenteVerificar()
        {

        }
        #endregion

        #region PROPRIEDADES
        /// <summary>
        /// Obter cord top
        /// </summary>
        public int Top
        {
            get { return cordTop; }
            set { cordTop = value; }
        }
        /// <summary>
        /// Obter cord bottom
        /// </summary>
        public int Bottom
        {
            get { return cordBottom; }
            set { cordBottom = value; }
        }
        /// <summary>
        /// Obter cord right
        /// </summary>
        public int Right
        {
            get { return cordRight; }
            set { cordRight = value; }
        }
        /// <summary>
        /// Obter cord left
        /// </summary>
        public int Left
        {
            get { return cordLeft; }
            set { cordLeft = value; }
        }
        /// <summary>
        /// Retorna o encoding
        /// </summary>
        public Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }
        /// <summary>
        /// Retorna das 3 cores para identificação a primeira associada.
        /// </summary>
        public int PrimeiraCor
        {
            get { return primeiraCor; }
            set { primeiraCor = value; }
        }
        /// <summary>
        /// Retorna das 3 cores para identificação a segunda associada.
        /// </summary>
        public int SegundaCor
        {
            get { return segundaCor; }
            set { segundaCor = value; }
        }
        /// <summary>
        /// Retorna das 3 cores para identificação a terceira associada.
        /// </summary>
        public int TerceiraCor
        {
            get { return terceiraCor; }
            set { terceiraCor = value; }
        }
        /// <summary>
        /// Obter o nivel de autorização.
        /// </summary>
        public int Autorizacao
        {
            get { return autorizacao; }
            set {  autorizacao = value; }
        }
        /// <summary>
        /// Obter o nome de utente
        /// </summary>
        public string Nome
        {
            get { return nomeUtz; }
            set {  nomeUtz = value; }
        }

        #endregion

        #region OPERADORES

        #endregion

        #region OVERRIDES

        #endregion

        #region OUTROS METODOS

        #endregion

        #endregion
    }
}