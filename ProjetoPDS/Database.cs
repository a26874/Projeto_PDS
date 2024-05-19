/*
*	<copyright file="Database" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/12/2024 11:37:24 PM</date>
*	<description></description>
**/

using Microsoft.EntityFrameworkCore;

namespace ProjetoPDS.Classes
{
    public class dataBase : DbContext
    {
        public dataBase(DbContextOptions<dataBase> options) : base(options)
        {

        }
        /// <summary>
        /// Aqui verifica se existe algum dos ids, para criação de novos ids a partir do ultimo inserido.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utente>().HasKey(u => u.idUtente); 
            modelBuilder.Entity<Encoding>().HasKey(u => u.idEncoding);
            modelBuilder.Entity<Publicacao>().HasKey(u => u.Publicacao_id);
            modelBuilder.Entity<Foto>().HasKey(u => u.Foto_id);
        }


        public DbSet<Utente> Utente { get; set; }
        public DbSet<Encoding> Encoding { get; set; }
        public DbSet<Publicacao> Publicacao { get; set; }
        public DbSet<Foto> Foto { get; set; }
    }
}
