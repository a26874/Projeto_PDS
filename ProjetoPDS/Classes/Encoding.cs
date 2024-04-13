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
        public Encoding()
        {

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