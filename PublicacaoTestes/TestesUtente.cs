/*
*	<copyright file="TestesUtente" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 5/22/2024 10:09:28 PM</date>
*	<description></description>
**/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoPDS.Classes;
using ProjetoPDS.Controllers;

[TestClass]
public class UtenteControllerTests
{
    private DbContextOptions<dataBase> options;
    [TestInitialize]
    public void Setup()
    {
        options = new DbContextOptionsBuilder<dataBase>()
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

    private void CriarMockup()
    {
        using (var context = new dataBase(options))
        {
            var encodingList = ObterEncodings();

            foreach (var encoding in encodingList)
            {
                if (!context.Encoding.Any(e => e.idEncoding == encoding.idEncoding))
                {
                    context.Encoding.Add(encoding);
                    break;
                }
            }
        }
    }

    [TestMethod]
    public async Task AdicionarUtente()
    {
        using (var context = new dataBase(options))
        {
            var controller =  new UtenteController(context);

            Utente utenteTeste = new Utente();
            utenteTeste.Nome = "testeUtente";
            utenteTeste.idUtente = 1;
            utenteTeste.Sala = "sala1";
            utenteTeste.Valencia = "val1";
            utenteTeste.Autorizacao = 2;
            
            var result =await controller.AdicionarUtenteAux(utenteTeste) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200,result.StatusCode);

        }
    }
}