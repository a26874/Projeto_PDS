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
    public class Foto
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private string fotoUrl;
        private int numUtentes;
        private int numUtentesIdentificados;
        private int numUtentesCensurados;
        private FotoStatus estadoFoto;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        public Foto()
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
        /// <summary>
        /// Obter o numero de utentes na foto.
        /// </summary>
        public int NumUtentes
        {
            get { return numUtentes; }
            set { numUtentes = value; }
        }
        /// <summary>
        /// Obter o numero de utentes processados na foto.
        /// </summary>
        public int NumProcessados
        {
            get { return numUtentesCensurados;}
            set { numUtentesCensurados = value; }
        }
        /// <summary>
        /// Obter o numero de utentes identificado na foto.
        /// </summary>
        public int NumIdentificados
        {
            get { return numUtentesIdentificados; }
            set { numUtentesIdentificados= value; }
        }
        /// <summary>
        /// Obter o estado da foto.
        /// </summary>
        public FotoStatus Estado
        {
            get { return estadoFoto; }
            set { estadoFoto = value; }
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