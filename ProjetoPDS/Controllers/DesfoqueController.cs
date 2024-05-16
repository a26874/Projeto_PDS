using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPDS.Classes;
using System.Drawing;

namespace ProjetoPDS.Controllers
{
    /// <summary>
    /// Controlador para fazer desfocagem.
    /// </summary>
    public class DesfoqueController : Controller
    {
        /// <summary>
        /// Na imagem caso seja clicado numa cara e escolhido a opção de censurar, irá verificar se está dentro dos limites identificados
        /// </summary>
        /// <param name="fotoOriginal"></param>
        /// <param name="nomeFotoFicheiro"></param>
        /// <param name="utentesPorVerificar"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RealizarDesfoque")]
        public async Task<IActionResult> Desfoque([FromForm] string fotoOriginal, [FromForm] string nomeFotoFicheiro, [FromForm] string absolutePath, [FromForm] string local, [FromForm] string utentesPorVerificar)
        {
            int numbLocal = 0;
            List<string> nomeFicheiros;
            int lastIndex = absolutePath.LastIndexOf('/');
            string pathImages = absolutePath.Substring(0, lastIndex);
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
                    return BadRequest();
            }
            if (fotoOriginal == null)
                return BadRequest();
            if (nomeFotoFicheiro == null)
                return BadRequest();
            nomeFicheiros.Add(nomeFotoFicheiro);

            FotoComDesfoque novoDesfoque = new FotoComDesfoque();

            List<UtenteVerificar> listaDesfoque = JsonConvert.DeserializeObject<List<UtenteVerificar>>(utentesPorVerificar);
            foreach (UtenteVerificar utente in listaDesfoque)
            {
                if (utente.Autorizacao < numbLocal)
                {
                    auxNomeFicheiro = nomeFotoFicheiro + "_" + utente.Nome;
                    nomeFicheiros.Add(pathImages + "\\" + auxNomeFicheiro);
                    //checkar se existe na base de dados uma imagem ja com esse nome, para nao haver duplicatas, se tiver coloca um _2 tipo isto
                    pathFotoDesfocada = novoDesfoque.AplicarDesfoque(fotoOriginal, auxNomeFicheiro, listaDesfoque, utente.Nome);
                    nomeFicheiros.Add(string.Copy(pathFotoDesfocada));
                }
            }
            pathFotoDesfocada = novoDesfoque.AplicarDesfoque(fotoOriginal, nomeFotoFicheiro, listaDesfoque, "");
            nomeFicheiros.Add(string.Copy(pathFotoDesfocada));
            return Ok(nomeFicheiros);
        }
    }
}
