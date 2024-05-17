/*
*	<copyright file="Foto" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 4:10:18 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;

namespace ProjetoPDS.Classes
{
    /// <summary>
    /// Classe para identificar uma foto de origem.
    /// </summary>
    public class Foto
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private string fotoUrl;
        private int numeroTotalUtentes;
        private int numeroIdentificados;
        private int numeroCensurados;
        private StatusFoto fotoStatusId;
        private int pubId;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        /// <summary>
        /// Construtor por defeito.
        /// </summary>
        public Foto()
        {

        }
        /// <summary>
        /// Construtor por parametros.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fotoUrl"></param>
        /// <param name="numeroTotalUtentes"></param>
        /// <param name="numeroIdentificados"></param>
        /// <param name="numeroCensurados"></param>
        /// <param name="fotoStatusId"></param>
        /// <param name="pubId"></param>
        public Foto(int id, string fotoUrl, int numeroTotalUtentes, int numeroIdentificados, int numeroCensurados, StatusFoto fotoStatusId, int pubId)
        {
            this.id = id;
            this.fotoUrl = fotoUrl;
            this.numeroTotalUtentes = numeroTotalUtentes;
            this.numeroIdentificados = numeroIdentificados;
            this.numeroCensurados = numeroCensurados;
            this.fotoStatusId = fotoStatusId;
            this.pubId = pubId;
        }
        #endregion

        #region PROPRIEDADES
        /// <summary>
        /// Obter o id da foto.
        /// </summary>
        public int Foto_id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// Obter o url da foto.
        /// </summary>
        public string url
        {
            get { return fotoUrl; }
            set { fotoUrl = value; }
        }
        /// <summary>
        /// Obter o numero total de utentes numa foto
        /// </summary>
        public int numero_utentes
        {
            get { return numeroTotalUtentes; }
            set { numeroTotalUtentes = value; }
        }
        /// <summary>
        /// Obter o numero de utentes identificados numa foto
        /// </summary>
        public int numero_utentes_identificados
        {
            get { return numeroIdentificados; }
            set { numeroIdentificados = value; }
        }
        /// <summary>
        /// Obter o numero de utentes censurados numa foto.
        /// </summary>
        public int numero_utentes_censurados
        {
            get { return numeroCensurados; }
            set { numeroCensurados = value; }
        }
        /// <summary>
        /// Obter o id do estado da foto.
        /// </summary>
        public StatusFoto FOTO_STATUSID
        {
            get { return fotoStatusId; }
            set { fotoStatusId = value; }
        }
        /// <summary>
        /// Obter o id de publicação.
        /// </summary>
        public int PUBLICACAOPublicacao_id
        {
            get { return pubId; }
            set { pubId = value; }
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