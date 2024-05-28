﻿using Microsoft.AspNetCore.Mvc;
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
        ///// <summary>
        ///// Na imagem caso seja clicado numa cara e escolhido a opção de censurar, irá verificar se está dentro dos limites identificados
        ///// </summary>
        ///// <param name="fotoOriginal"></param>
        ///// <param name="nomeFotoFicheiro"></param>
        ///// <param name="utentesPorVerificar"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("RealizarDesfoque")]
        //public async Task<IActionResult> Desfoque([FromForm] string fotoOriginal, [FromForm] string nomeFotoFicheiro, [FromForm] string local, [FromForm] string utentesPorVerificar)
        //{
        //    int numbLocal = 0;
        //    List<string> nomeFicheiros;
        //    string pathImages = "C:\\Users\\marco\\source\\repos\\Projeto_PDS\\ProjetoPDS\\website\\Imagens\\Fotos_Desfocadas";
        //    nomeFotoFicheiro = Path.GetFileNameWithoutExtension(nomeFotoFicheiro);
        //    string pathFotoDesfocada;
        //    string auxNomeFicheiro;
        //    nomeFicheiros = new List<string>();

        //    if (local != null)
        //    {
        //        if (local == "EncEdc")
        //            numbLocal = 1;
        //        else if (local == "Sala")
        //            numbLocal = 2;
        //        else if (local == "Mural")
        //            numbLocal = 3;
        //        else if (local == "Chat")
        //            numbLocal = 4;
        //        else
        //            return BadRequest(string.Format("Não existe o local inserido: {0}",local));
        //    }
        //    if (fotoOriginal == null || nomeFotoFicheiro == null)
        //        return BadRequest("Caminho da foto original ou nome do ficheiro não existentes.");
        //    FotoComDesfoque novoDesfoque = new FotoComDesfoque();
        //    List<UtenteVerificar> listaDesfoque = JsonConvert.DeserializeObject<List<UtenteVerificar>>(utentesPorVerificar);
        //    listaDesfoque[0].Nome = "costa rica";
        //    listaDesfoque[0].Autorizacao = 0;
        //    foreach (UtenteVerificar utente in listaDesfoque)
        //    {
        //        if (utente.Autorizacao < numbLocal && utente.Nome != null)
        //        {
        //            string nomeMinus = utente.Nome.Replace(" ", "-");
        //            auxNomeFicheiro = nomeFotoFicheiro + "_" + nomeMinus;
        //            //checkar se existe na base de dados uma imagem ja com esse nome, para nao haver duplicatas, se tiver coloca um _2 tipo isto
        //            pathFotoDesfocada = novoDesfoque.AplicarDesfoque(fotoOriginal, pathImages + "\\" + auxNomeFicheiro + ".png"/*auxNomeFicheiro*/, listaDesfoque, utente.Nome);
        //            nomeFicheiros.Add(string.Copy(pathFotoDesfocada));
        //        }
        //    }
        //    pathFotoDesfocada = novoDesfoque.AplicarDesfoque(fotoOriginal, pathImages + "\\" + nomeFotoFicheiro + ".png", listaDesfoque, "a");
        //    nomeFicheiros.Add(string.Copy(pathFotoDesfocada));

        //    return Ok(Json(nomeFicheiros));

        [HttpPost]
        [Route("RealizarDesfoque")]
        public async Task<IActionResult> Desfoque([FromForm] string fotoOriginal, [FromForm] string nomeFotoFicheiro, [FromForm] string utentesPorVerificar, [FromForm] string local, [FromForm] string utentesVerificados)
        {
            if (fotoOriginal == null || utentesPorVerificar.Count() < 1 || utentesVerificados.Count() < 1)
                return BadRequest();
            int numLocal = 0;
            FotoComDesfoque novoDesfoque = new FotoComDesfoque();

            List<UtenteVerificar> listaDesfoque = JsonConvert.DeserializeObject<List<UtenteVerificar>>(utentesPorVerificar);
            List<UtenteIdentificado> listaDesfoqueVerificados = JsonConvert.DeserializeObject<List<UtenteIdentificado>>(utentesVerificados);



            string auxNomeFicheiro = "";
            if (local != null)
            {
                if (local == "EncEdc")
                    numLocal = 1;
                else if (local == "Sala")
                    numLocal = 2;
                else if (local == "Mural")
                    numLocal = 3;
                else if (local == "Chat")
                    numLocal = 4;
                else
                    return BadRequest(string.Format("Não existe o local inserido: {0}", local));
            }
            string fotoDesfocada = "";
            //Caso um utilizador não esteja identificado, ele é desfocado automaticamente.
            foreach (UtenteVerificar u in listaDesfoque)
                fotoDesfocada = await novoDesfoque.AplicarDesfoque(fotoOriginal, nomeFotoFicheiro, listaDesfoque);



            List<UtenteIdentificado> auxListaDesfoqueVerificados = novoDesfoque.VerificarAutorizacaoVerificados(listaDesfoqueVerificados);

            Dictionary<string, string> fotosDesfocadasEncEdc = new Dictionary<string, string>();
            string auxFotoEncEdc = "";
            if (fotoDesfocada == "") fotoDesfocada = fotoOriginal;

            List<UtenteIdentificado> utentesDesfoqueVerificado = new List<UtenteIdentificado>();

            foreach (UtenteIdentificado u in auxListaDesfoqueVerificados)
            {
                if (u.Autorizacao < numLocal)
                {
                    utentesDesfoqueVerificado.Add(u);
                }
            }

            foreach (UtenteIdentificado u in utentesDesfoqueVerificado)
            {
                auxFotoEncEdc = await novoDesfoque.AplicarDesfoqueIdentificado(fotoDesfocada, nomeFotoFicheiro, utentesDesfoqueVerificado, u.Nome);
                fotosDesfocadasEncEdc.Add(u.Nome, auxFotoEncEdc);
            }

            var payloadFotoDesfocada = new
            {
                pathFotosDesfocadasEncEdc = fotosDesfocadasEncEdc,
                pathFotoDesfocada = fotoDesfocada,
            };
            return Ok(payloadFotoDesfocada);
        }


    }
}
