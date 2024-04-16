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
        public async Task<IActionResult> VerificarPublicacao([FromForm] InfoPublicacao pub)
        {
            if (pub == null)
                return BadRequest();
            string nomeFicheiro = Path.GetFileName(pub.Foto.FileName);
            string nomeDiretorio = Path.Combine(guardarFotoCaminho, nomeFicheiro);

            if (!Path.Exists(nomeDiretorio))
                using (var stream = new FileStream(nomeDiretorio, FileMode.Create))
                {
                    await pub.Foto.CopyToAsync(stream);
                }

            FotoComDesfoque novoDesfoque = new FotoComDesfoque();

            return Ok();
        }

    }
}
