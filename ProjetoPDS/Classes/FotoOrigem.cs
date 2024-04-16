/*
*	<copyright file="FotoOrigem" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 4:10:18 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;

namespace ProjetoPDS.Classes
{
    public class FotoOrigem
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private string fotoUrl;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        public FotoOrigem()
        {

        }
        #endregion

        #region PROPRIEDADES
        /// <summary>
        /// Obter o id da foto.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// Obter o url da foto.
        /// </summary>
        public string URL
        {
            get { return fotoUrl; }
            set { fotoUrl = value; }
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