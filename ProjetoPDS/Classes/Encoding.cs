/*
*	<copyright file="Encoding" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/13/2024 6:12:32 PM</date>
*	<description></description>
**/

using Python.Runtime;
using System.ComponentModel.DataAnnotations;

namespace ProjetoPDS.Classes
{
    /// <summary>
    /// Classe que identifica um encoding.
    /// </summary>
    public class Encoding
    {
        #region ATRIBUTOS
        [Key]
        private int idEnc;
        private byte[] enc;
        private int idUtente;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        /// <summary>
        /// Construtor por defeito.
        /// </summary>
        public Encoding()
        {

        }
        /// <summary>
        /// Construtor por parametros.
        /// </summary>
        /// <param name="idEnc"></param>
        /// <param name="enc"></param>
        /// <param name="idUtente"></param>
        public Encoding(int idEnc, byte[] enc, int idUtente)
        {
            this.idEnc = idEnc;
            this.enc = enc;
            this.idUtente = idUtente;
        }
        #endregion

        #region PROPRIEDADES
        /// <summary>
        /// Obter id do encoding
        /// </summary>
        public int idEncoding
        {
            set { idEnc = value; }
            get { return idEnc; }
        }
        /// <summary>
        /// Obter encoding
        /// </summary>
        public byte[] encoding
        {
            set { enc = value;}
            get { return enc; }
        }
        /// <summary>
        /// Obter idUtente
        /// </summary>
        public int UTENTEidUtente
        {
            set { idUtente = value;}
            get { return idUtente; }
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