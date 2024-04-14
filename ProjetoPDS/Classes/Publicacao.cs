/*
*	<copyright file="Publicacao" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 3:59:41 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;

namespace ProjetoPDS.Classes
{
    public class Publicacao
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private int idUtilizador;
        private DateTime dataPublicacao;
        private LocalPublicacao local;
        private string dataResult;
        private string caminhoFoto;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        public Publicacao()
        {

        }
        #endregion

        #region PROPRIEDADES
        /// <summary>
        /// Obter o id de publicação
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// Obter o id do utilizador.
        /// </summary>
        public int IdUtilizador
        {
            get { return idUtilizador;}
            set { idUtilizador = value;}
        }
        /// <summary>
        /// Obter a data de publicação.
        /// </summary>
        public DateTime DataPublicacao
        { 
            get { return dataPublicacao; }
            set {  dataPublicacao = value; }
        }
        /// <summary>
        /// Obter o resultado do processamento da foto.
        /// </summary>
        public string DataResult
        {
            get { return dataResult;}
            set { dataResult = value; } 
        }
        /// <summary>
        /// Obter o local de publicação da foto.
        /// </summary>
        public LocalPublicacao Local
        {
            get { return local; }
            set { local = value; }
        }
        /// <summary>
        /// Obter o url da foto.
        /// </summary>
        public string CaminhoFoto
        {
            get { return caminhoFoto;}
            set { caminhoFoto = value;}
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