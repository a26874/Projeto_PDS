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
        public async Task<IActionResult> Desfoque([FromForm] string fotoOriginal, [FromForm] string nomeFotoFicheiro, [FromForm] string local, [FromForm] string utentesPorVerificar)
        {
            int numbLocal = 0;
            List<string> nomeFicheiros;
            string pathImages = "C:\\VisualStudioProjetos\\Projeto_PDS\\ProjetoPDS\\website\\Imagens\\Fotos_Desfocadas";
            nomeFotoFicheiro = Path.GetFileNameWithoutExtension(nomeFotoFicheiro);
            string pathFotoDesfocada;
            string auxNomeFicheiro;
            nomeFicheiros = new List<string>();

            if (local != null)
            {
                if (local == "EncEdc")
                    numbLocal = 1;
                else if (local == "Sala")
                    numbLocal = 2;
                else if (local == "Mural")
                    numbLocal = 3;
                else if (local == "Chat")
                    numbLocal = 4;
                else
                    return BadRequest(string.Format("Não existe o local inserido: {0}",local));
            }
            if (fotoOriginal == null || nomeFotoFicheiro == null)
                return BadRequest("Caminho da foto original ou nome do ficheiro não existentes.");
            FotoComDesfoque novoDesfoque = new FotoComDesfoque();
            List<UtenteVerificar> listaDesfoque = JsonConvert.DeserializeObject<List<UtenteVerificar>>(utentesPorVerificar);
            foreach (UtenteVerificar utente in listaDesfoque)
            {
                if (utente.Autorizacao < numbLocal && utente.Nome != null)
                {
                    auxNomeFicheiro = nomeFotoFicheiro + "_" + utente.Nome;
                    //checkar se existe na base de dados uma imagem ja com esse nome, para nao haver duplicatas, se tiver coloca um _2 tipo isto
                    pathFotoDesfocada = novoDesfoque.AplicarDesfoque(fotoOriginal, pathImages + "\\" + auxNomeFicheiro + ".png"/*auxNomeFicheiro*/, listaDesfoque, utente.Nome);
                    nomeFicheiros.Add(string.Copy(pathFotoDesfocada));
                }
            }
            pathFotoDesfocada = novoDesfoque.AplicarDesfoque(fotoOriginal, pathImages + "\\" + nomeFotoFicheiro + ".png", listaDesfoque, "a");
            nomeFicheiros.Add(string.Copy(pathFotoDesfocada));
            return Ok(Json(nomeFicheiros));
        }
    }
}
