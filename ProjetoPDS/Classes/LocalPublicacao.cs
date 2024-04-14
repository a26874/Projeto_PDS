/*
*	<copyright file="LocalPublicacao" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 4:01:01 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;

namespace ProjetoPDS.Classes
{
    public class LocalPublicacao
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private string nome;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        public LocalPublicacao()
        {

        }
        #endregion

        #region PROPRIEDADES
        /// <summary>
        /// Obter id da publicação
        /// </summary>
        public int Id
        {
            set { id = value; }
            get { return id; }
        }
        /// <summary>
        /// Obter o nome do local da publicação
        /// </summary>
        public string Nome
        {
            set { nome = value; }
            get { return nome; }
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