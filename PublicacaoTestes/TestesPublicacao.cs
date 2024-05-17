using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoPDS.Classes;
using ProjetoPDS.Controllers;

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

    private List<Encoding> ObterEncodings()
    {
        var options = new DbContextOptionsBuilder<dataBase>()
            .UseSqlServer("Server=MARCO\\MARCO;Database=photo_database;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;
        using (var context = new dataBase(options))
        {
            return context.Encoding.ToList();
        }
    }
    private void CriarMockup()
    {
        using (var context = new dataBase(_options))
        {
            // Add test data to the in-memory database for testing
            var utente = new Utente { idUtente = 1, Nome = "John Doe", Valencia = "Valencia1", Sala = "Sala1", Autorizacao = 1 };
            context.Utente.Add(utente);

            // Fetch and add encoding data from the actual database
            var encodingList = ObterEncodings();
            context.Encoding.AddRange(encodingList);

            context.SaveChanges();
        }
    }


    [TestMethod]
    public async Task VerificarPublicacao_ValidData_ReturnsOkResult()
    {
        using (var context = new dataBase(_options))
        {
            // Arrange
            var controller = new PublicacaoController(context);

            var filePath = @"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\website\Imagens\Uploads_Publicacoes\image.png";
            var fileName = Path.GetFileName(filePath);
            var stream = new FileStream(filePath, FileMode.Open);

            IFormFile pubFoto = new FormFile(stream, 0, stream.Length, "pubFoto", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };

            string dataPub = DateTime.Now.ToString("yyyy-MM-dd");
            int idUtlz = 1;
            string local = "Sala";

            var result = await controller.VerificarPublicacao(pubFoto, dataPub, idUtlz, local) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
