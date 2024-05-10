using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ProjetoPDS.Classes;
using Python.Runtime;
using System.Linq;

namespace ProjetoPDS.Controllers
{
    /// <summary>
    /// Todas as funções que estão aqui foram iniciais e apenas para teste. Já não são usadas nenhumas.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    
    public class UtenteController : Controller
    {
        private readonly string guardarFotoCaminho;
        private readonly dataBase baseDados;
        public UtenteController(dataBase baseDados)
        {
            guardarFotoCaminho = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(guardarFotoCaminho))
            {
                Directory.CreateDirectory(guardarFotoCaminho);
            }
            this.baseDados = baseDados;
        }
        /// <summary>
        /// Função para criação de utente e adicionar as suas informações e encoding à base de dados.
        /// </summary>
        /// <param name="utente"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addSingleUtente")]
        public async Task<IActionResult> addSingleUtente([FromForm] Utente utente)
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

            FotoComDesfoque novoReconhecimento = new FotoComDesfoque();

            var binaryEncoding = novoReconhecimento.addUtente(filePath);
            Encoding novoEncoding = new Encoding();
            novoEncoding.encoding = binaryEncoding;
            var existeUtente = baseDados.Utente.FirstOrDefault(u => u.Nome == utente.Nome);
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

        /// <summary>
        /// Dada uma foto, verifica se essa pessoa existe na base de dados conforme o seu encoding.
        /// </summary>
        /// <param name="utente"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("verificarUtente")]
        //public async Task<IActionResult> verificarUtente([FromForm] UtenteVerificar utente)
        //{
        //    int idUtente = 0;
        //    if (utente == null)
        //        return BadRequest();
        //    string fileName = Path.GetFileName(utente.Foto.FileName);
        //    string filePath = Path.Combine(guardarFotoCaminho, fileName);

        //    if (!Path.Exists(filePath))
        //        return BadRequest();

        //    FotoComDesfoque novoReconhecimento = new FotoComDesfoque();

        //    var binaryEncoding = novoReconhecimento.IdentificarUtentes(filePath);

        //    var todosEncoding = baseDados.Encoding.ToList();
        //    foreach(Encoding encodingValue in todosEncoding)
        //    {
        //        if (encodingValue.encoding.SequenceEqual(binaryEncoding))
        //        {
        //            idUtente = encodingValue.UTENTEidUtente;
        //            break;
        //        }
        //    }
        //    if (idUtente == 0)
        //        return BadRequest();

        //    Utente auxUtente= new Utente();

        //    var infoUtente = baseDados.Utente.FirstOrDefault(u => u.idUtente == idUtente);
        //    if (infoUtente == null)
        //        return BadRequest("Nao existe.");
        //    auxUtente.idUtente = infoUtente.idUtente;
        //    auxUtente.Nome = infoUtente.Nome;
        //    auxUtente.Valencia = infoUtente.Valencia;
        //    auxUtente.Sala = infoUtente.Sala;
        //    auxUtente.Autorizacao = infoUtente.Autorizacao;
        //    return Ok(auxUtente);
        //}
    }
    
}
