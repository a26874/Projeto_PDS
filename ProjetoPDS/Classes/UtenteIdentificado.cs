/*
*	<copyright file="UtenteIdentificado" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/15/2024 7:13:51 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;

namespace ProjetoPDS.Classes
{
    public class UtenteIdentificado
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private string nome;
        private int cordX;
        private int cordY;
        private int rectLargura;
        private int rectAltura;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        /// <summary>
        /// Construtor por defeito.
        /// </summary>
        public UtenteIdentificado()
        {

        }
        /// <summary>
        /// Construtor por parametros.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="cordX"></param>
        /// <param name="cordY"></param>
        /// <param name="rectLargura"></param>
        /// <param name="rectAltura"></param>
        public UtenteIdentificado(string nome, int cordX, int cordY, int rectLargura, int rectAltura)
        {
            this.nome = nome;
            this.cordX = cordX;
            this.cordY = cordY;
            this.rectLargura = rectLargura;
            this.rectAltura = rectAltura;
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