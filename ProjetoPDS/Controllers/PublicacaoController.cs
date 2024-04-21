using Microsoft.AspNetCore.Mvc;
using ProjetoPDS.Classes;

namespace ProjetoPDS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicacaoController : Controller
    {
        private readonly string guardarFotoCaminho;
        private readonly dataBase baseDados;
        public PublicacaoController(dataBase baseDados)
        {
            guardarFotoCaminho = Path.Combine(Directory.GetCurrentDirectory(), "Uploads_Publicacoes");
            if (!Directory.Exists(guardarFotoCaminho))
            {
                Directory.CreateDirectory(guardarFotoCaminho);
            }
            this.baseDados = baseDados;
        }


        [HttpPost]
        [Route("VerificarPublicacao")]
        public async Task<IActionResult> VerificarPublicacao([FromForm] Publicacao pub)
        {
            if (pub == null || pub.Foto == null)
                return BadRequest();
            string nomeFicheiro = Path.GetFileName(pub.Foto.FileName);
            string nomeDiretorio = Path.Combine(guardarFotoCaminho, nomeFicheiro);


            if (!Path.Exists(nomeDiretorio))
                using (var stream = new FileStream(nomeDiretorio, FileMode.Create))
                    await pub.Foto.CopyToAsync(stream);
            
            pub.CaminhoFoto = nomeDiretorio;
            
            baseDados.Publicacao.Add(pub);
            await baseDados.SaveChangesAsync();
            FotoComDesfoque novoDesfoque = new FotoComDesfoque();
            
            List<UtenteIdentificado> utentesIdentificados = novoDesfoque.IdentificarUtentes(nomeDiretorio);
            var todosEncoding = baseDados.Encoding.ToList();

            int idUtente = 0;
            foreach(UtenteIdentificado utente in utentesIdentificados)
            {
                if (utente == null)
                    continue;
                foreach(Encoding encodingValue in todosEncoding)
                {
                    if(novoDesfoque.verificarEncoding(encodingValue.encoding, utente.Encoding.encoding))
                    {
                        idUtente = encodingValue.UTENTEidUtente;
                        var infoUtente = baseDados.Utente.FirstOrDefault(u => u.idUtente == idUtente);
                        if (infoUtente != null)
                        {
                            utente.Nome = infoUtente.Nome;
                            utente.Id = infoUtente.idUtente;
                            break;
                        }
                    }
                }
            }
            return Ok();
        }

    }
}
