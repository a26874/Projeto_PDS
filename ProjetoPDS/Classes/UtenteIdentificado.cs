/*
*	<copyright file="UtenteIdentificado" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/15/2024 7:13:51 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoPDS.Classes
{
    public class UtenteIdentificado
    {
        #region ATRIBUTOS
        private int id;
        private string nome;
        private int cordTop;
        private int cordBottom;
        private int cordRight;
        private int cordLeft;
        private int autorizacao;
        private Encoding encoding;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        /// <summary>
        /// Construtor por defeito.
        /// </summary>
        public UtenteIdentificado()
        {

        }
        public UtenteIdentificado(int id, string nome, int cordTop, int cordBottom, int cordRight, int cordLeft,int autorizacao, Encoding encoding, int top, int bottom, int right, int left)
        {
            this.nome = nome;
            this.cordTop = cordTop;
            this.cordBottom = cordBottom;
            this.cordRight = cordRight;
            this.cordLeft = cordLeft;
            this.autorizacao = autorizacao;
            this.encoding = encoding;
        }

        #endregion

        #region PROPRIEDADES

        /// <summary>
        /// Obter o id de um utente identificado.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Obter autorizacao
        /// </summary>
        public int Autorizacao
        {
            get { return autorizacao; }
            set { autorizacao = value; }
        }

        /// <summary>
        /// Obter o nome de um utente identificado.
        /// </summary>
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
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