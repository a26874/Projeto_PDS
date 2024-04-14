/*
*	<copyright file="FotoStatus" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 4:11:43 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;

namespace ProjetoPDS.Classes
{
    public class FotoStatus
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private string estado;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        public FotoStatus()
        {

        }
        #endregion

        #region PROPRIEDADES
        /// <summary>
        /// Obter o id de estado da foto.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// Obter o estado da foto.
        /// </summary>
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
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