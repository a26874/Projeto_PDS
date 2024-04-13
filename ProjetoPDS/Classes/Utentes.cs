/*
*	<copyright file="Utentes" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/12/2024 7:15:05 PM</date>
*	<description></description>
**/

namespace ProjetoPDS.Classes
{
    public class Utentes
    {
        #region ATRIBUTOS
        private static List<Utente> listaUtentes;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES
        /// <summary>
        /// Construtor por defeito.
        /// </summary>
        static Utentes()
        {
            listaUtentes = new List<Utente>();
        }
        #endregion

        #region PROPRIEDADES
        /// <summary>
        /// Obter os clientes existentes.
        /// </summary>
        public static List<Utente> ObterUtentes
        {
            get { return listaUtentes.ToList(); }
        }
        #endregion

        #region OPERADORES

        #endregion

        #region OVERRIDES

        #endregion

        #region OUTROS METODOS
        /// <summary>
        /// Adiciona um utente à lista de utentes.
        /// </summary>
        /// <param name="u1"></param>
        /// <returns></returns>
        public bool addUtente(Utente u1)
        {
            if (listaUtentes.Contains(u1))
                return false;
            listaUtentes.Add(u1);
            return true;
        }
        #endregion

        #endregion
    }
}