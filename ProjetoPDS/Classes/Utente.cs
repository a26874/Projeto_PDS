/*
*	<copyright file="Utente" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/12/2024 7:07:42 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoPDS.Classes
{
    /// <summary>
    /// Classe para um utente.
    /// </summary>
    public class Utente
    {
        #region ATRIBUTOS
        [Key]
        private int id;
        private string nome;
        private string valencia;
        private string sala;
        private int autorizacao;
        [NotMapped]
        private IFormFile uploadFoto;
        #endregion

        #region COMPORTAMENTO

        #region CONSTRUTORES

        /// <summary>
        /// Construtor por defeito
        /// </summary>
        public Utente()
        {

        }
        /// <summary>
        /// Construtor por parametros.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="valencia"></param>
        /// <param name="sala"></param>
        /// <param name="autorizacao"></param>
        /// <param name="foto"></param>
        public Utente(string nome, string valencia, string sala, int autorizacao, IFormFile foto)
        {
            this.nome = nome;
            this.valencia = valencia;
            this.sala = sala;
            this.autorizacao = autorizacao;
            uploadFoto = foto;
        }

        #endregion

        #region PROPRIEDADES

        /// <summary>
        /// Obter nome utente
        /// </summary>
        public string Nome
        {
            set { nome = value; }
            get { return nome; }
        }
        /// <summary>
        /// Obter valencia
        /// </summary>
        public string Valencia
        {
            set { valencia = value; }
            get { return valencia; }
        }
        /// <summary>
        /// Obter sala
        /// </summary>
        public string Sala
        {
            set { sala = value; }
            get { return sala; }
        }
        /// <summary>
        /// Obter Autorização
        /// </summary>
        public int Autorizacao
        {
            set { autorizacao = value; }
            get { return autorizacao; }
        }
        /// <summary>
        /// Obter uploadFoto.
        /// </summary>
        /// 
        [NotMapped]

        public IFormFile Foto
        {
            set { uploadFoto = value; }
            get { return uploadFoto; }
        }
        /// <summary>
        /// Obter id do utente
        /// </summary>
        public int idUtente
        {
            get {  return id; } 
            set {  id = value; }
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