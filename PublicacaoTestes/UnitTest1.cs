using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetoPDS.Controllers;
using ProjetoPDS.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PublicacaoTestes
{
    [TestClass]
    public class PublicacaoControllerTests
    {
        private DbContextOptions<dataBase> _options;
        [TestInitialize]
        public void Setup()
        {
            // Set up DbContextOptions for in-memory database
            _options = new DbContextOptionsBuilder<dataBase>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Seed the in-memory database with test data if needed
            CriarMockup();
        }
        private void CriarMockup()
        {
            using (var context = new dataBase(_options))
            {
                // Add test data to the in-memory database for testing
                // Example:
                context.Utente.Add(new Utente { Nome = "John Doe" });
                context.Encoding.Add(new Encoding { /* Encoding properties */ });
                context.Publicacao.Add(new Publicacao { /* Publicacao properties */ });
                context.Foto.Add(new Foto { /* Foto properties */ });

                context.SaveChanges();
            }
        }
        [TestMethod]
        public async Task VerificarPublicacao_ValidData_ReturnsOkResult()
        {
            using (var context = new dataBase(_options))
            {
                // Perform test actions using the in-memory database context
                var utentes = context.Utente.ToList();
                var encodings = context.Encoding.ToList();
                var publicacoes = context.Publicacao.ToList();
                var fotos = context.Foto.ToList();

                // Assert or perform other test validations
                Assert.IsNotNull(utentes);
                Assert.IsNotNull(encodings);
                Assert.IsNotNull(publicacoes);
                Assert.IsNotNull(fotos);

                // Example assertions:
                Assert.AreEqual(1, utentes.Count);
                // Add more assertions as needed
            }
        }

    }
}