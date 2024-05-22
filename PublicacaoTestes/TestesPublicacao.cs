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
        _options = new DbContextOptionsBuilder<dataBase>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

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
    private List<Foto> ObterFotos()
    {
        var options = new DbContextOptionsBuilder<dataBase>()
            .UseSqlServer("Server=MARCO\\MARCO;Database=photo_database;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;
        using (var context = new dataBase(options))
            return context.Foto.ToList();
    }

    private List<Publicacao> ObterPublicacoes()
    {
        var options = new DbContextOptionsBuilder<dataBase>()
            .UseSqlServer("Server=MARCO\\MARCO;Database=photo_database;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;
        using (var context = new dataBase(options))
            return context.Publicacao.ToList();
    }
    private void CriarMockup()
    {
        using (var context = new dataBase(_options))
        {
            var utente = new Utente { idUtente = 1, Nome = "John Doe", Valencia = "Valencia1", Sala = "Sala1", Autorizacao = 1 };

            if (!context.Utente.Any(u => u.idUtente == utente.idUtente))
            {
                context.Utente.Add(utente);
            }

            var encodingList = ObterEncodings();

            foreach (var encoding in encodingList)
            {
                if (!context.Encoding.Any(e => e.idEncoding== encoding.idEncoding))
                {
                    context.Encoding.Add(encoding);
                }
            }

            var publicacaoLista = ObterPublicacoes();

            foreach (var publicacao in publicacaoLista)
            {
                if (!context.Publicacao.Any(p => p.Publicacao_id == publicacao.Publicacao_id))
                {
                    context.Publicacao.Add(publicacao);
                }
            }

            var fotosLista = ObterFotos();

            foreach (var foto in fotosLista)
            {
                if (!context.Foto.Any(f => f.Foto_id== foto.Foto_id))
                {
                    context.Foto.Add(foto);
                }
            }

            context.SaveChanges();
        }
    }



    /// <summary>
    /// Testes para adição de publicaçoes.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task AdicionarPublicacao()
    {
        using (var context = new dataBase(_options))
        {
            
            var controller = new PublicacaoController(context);

            var filePath = @"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\website\Imagens\Uploads_Publicacoes\image.png";
            //var filePath = @"C:\Users\marco\Desktop\backupPDS\.gitattributes";
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

            var result = await controller.AdicionarPublicacao(pubFoto, dataPub, idUtlz, local) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
    /// <summary>
    /// Testes para remoção de publicações.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task ApagarPublicacao()
    {
        using (var context = new dataBase(_options))
        {
            var controller = new PublicacaoController(context);
            Publicacao removePub = new Publicacao();

            removePub.Local_Publicacaoid = LocalPublicacao.Sala;
            removePub.DataPublicacao = DateTime.Now;
            removePub.Publicacao_id = 1;
            removePub.CaminhoFoto = @"C:\Users\marco\source\repos\Projeto_PDS\ProjetoPDS\website\Imagens\Uploads_Publicacoes\image.png";
            removePub.IdUtilizador = 1;

            var result = await controller.ApagarPublicacao(removePub.Publicacao_id) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
