using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ProjetoPDS.Classes;
using Python.Runtime;

namespace ProjetoPDS.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AddUtenteController : Controller
    {
        private readonly string guardarFotoCaminho;
        private readonly dataBase baseDados;
        public AddUtenteController(dataBase baseDados)
        {
            guardarFotoCaminho = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(guardarFotoCaminho))
            {
                Directory.CreateDirectory(guardarFotoCaminho);
            }
            this.baseDados = baseDados;
        }
        [HttpPost]
        public async Task<IActionResult> addUtentes([FromForm] Utente utente)
        {
            if (utente == null)
                return BadRequest();
            string fileName = Path.GetFileName(utente.Foto.FileName);
            string filePath = Path.Combine(guardarFotoCaminho, fileName);

            if (!Path.Exists(filePath))
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await utente.Foto.CopyToAsync(stream);
                }

            ReconhecimentoFacial novoReconhecimento = new ReconhecimentoFacial();

            var binaryEncoding = novoReconhecimento.getFacialEncoding(filePath);
            Encoding novoEncoding = new Encoding();
            novoEncoding.encoding = binaryEncoding;
            var existeUtente = baseDados.Utente.FirstOrDefault(u => u.Nome == utente.Nome );
            if (existeUtente != null)
            {
                novoEncoding.UTENTEidUtente = existeUtente.idUtente;
                baseDados.Encoding.Add(novoEncoding);
                await baseDados.SaveChangesAsync();
            }
            else
            {
                baseDados.Utente.Add(utente);
                await baseDados.SaveChangesAsync();
                novoEncoding.UTENTEidUtente = utente.idUtente;
                baseDados.Encoding.Add(novoEncoding);
                await baseDados.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
