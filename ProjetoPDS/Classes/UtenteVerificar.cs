/*
*	<copyright file="UtenteVerificar" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 2:30:58 PM</date>
*	<description></description>
**/

namespace ProjetoPDS.Classes
{
    public class UtenteVerificar
    {
        #region ATRIBUTOS
        private IFormFile foto;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        public UtenteVerificar()
        {

        }
        #endregion

        #region PROPRIEDADES
        /// <summary>
        /// Obter informação sobre a foto
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