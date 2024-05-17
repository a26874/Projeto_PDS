using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPDS.Classes;

namespace ProjetoPDS.Controllers
{
    /// <summary>
    /// Controlador para fazer desfocagem.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DesfoqueController : Controller
    {
        /// <summary>
        /// Na imagem caso seja clicado numa cara e escolhido a opção de censurar, irá verificar se está dentro dos limites identificados
        /// </summary>
        /// <param name="fotoOriginal"></param>
        /// <param name="nomeFotoFicheiro"></param>
        /// <param name="absolutePath"></param>
        /// <param name="utentesPorVerificar"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RealizarDesfoque")]
        public async Task<IActionResult> Desfoque([FromForm] string fotoOriginal, [FromForm] string nomeFotoFicheiro, [FromForm] string absolutePath, [FromForm] string utentesPorVerificar)
        {
            //Esta função é antiga, tem de ser reescrita, pois ainda está por click.
            if (fotoOriginal == null)
                return BadRequest();

            FotoComDesfoque novoDesfoque = new FotoComDesfoque();

            List<UtenteVerificar> listaDesfoque = JsonConvert.DeserializeObject<List<UtenteVerificar>>(utentesPorVerificar);

            var pathFotoDesfocada = novoDesfoque.AplicarDesfoque(fotoOriginal, nomeFotoFicheiro, absolutePath, listaDesfoque);
            return Ok(pathFotoDesfocada);
        }
    }
}
