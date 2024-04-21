/*
*	<copyright file="Publicacao" company="IPCA">
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
    public class Publicacao
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private int idUtilizador;
        private DateTime dataPublicacao;
        private LocalPublicacao local;
        private List<FotoOrigem> listaFotosOrigem;
        [NotMapped]
        private IFormFile foto;
        private string? caminhoFoto;
        //[NotMapped]
        //private string dataResult;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        /// <summary>
        /// Construtor por defeito.
        /// </summary>
        public Publicacao()
        {

        }
        /// <summary>
        /// Construtor com parametros.
        /// </summary>
        /// <param name="idPub"></param>
        /// <param name="idUtlz"></param>
        /// <param name="dataPub"></param>
        /// <param name="localPostagem"></param>
        /// <param name="listaFotosOrigem"></param>
        /// <param name="fotoPub"></param>
        /// <param name="caminhoFoto"></param>
        public Publicacao(int idPub, int idUtlz, DateTime dataPub, LocalPublicacao localPostagem, List<FotoOrigem> listaFotosOrigem, IFormFile fotoPub, string caminhoFoto)
        {
            id = idPub;
            idUtilizador= idUtlz;
            dataPublicacao= dataPub;
            local = localPostagem;
            this.listaFotosOrigem = listaFotosOrigem;
            foto = fotoPub;
            this.caminhoFoto = caminhoFoto;
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
        //[NotMapped]
        //public string DataResult
        //{
        //    get { return dataResult;}
        //    set { dataResult = value; } 
        //}
        /// <summary>
        /// Obter o local de publicação da foto.
        /// </summary>
        public LocalPublicacao Local_Publicacaoid
        {
            get { return local; }
            set { local = value; }
        }
        /// <summary>
        /// Obter o url da foto.
        /// </summary>
        public string? CaminhoFoto
        {
            get { return caminhoFoto; }
            set { caminhoFoto = value; }
        }
        /// <summary>
        /// Obter a foto da publicação
        /// </summary>
        [NotMapped]
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