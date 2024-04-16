/*
*	<copyright file="InfoPublicacao" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 3:59:41 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoPDS.Classes
{
    public class InfoPublicacao
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private int idUtilizador;
        private DateTime dataPublicacao;
        private LocalPublicacao local;
        private string dataResult;
        private List<FotoOrigem> listaFotosOrigem;
        private string caminhoFoto;
        [NotMapped]
        private IFormFile foto;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        public InfoPublicacao()
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
        /// <summary>
        /// Obter a foto da publicação
        /// </summary>
        public IFormFile Foto
        {
            get { return foto; }
            set { foto = value; }
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