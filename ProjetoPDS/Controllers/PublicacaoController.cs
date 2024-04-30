using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPDS.Classes;
using System.Collections.Generic;

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
            
            //baseDados.Publicacao.Add(pub);
            //await baseDados.SaveChangesAsync();
            FotoComDesfoque novoReconhecimento = new FotoComDesfoque();
            
            List<UtenteIdentificado> utentesIdentificados = novoReconhecimento.IdentificarUtentes(nomeDiretorio);
            var todosEncoding = baseDados.Encoding.ToList();
            List<UtenteVerificar> utentesPorVerificar = new List<UtenteVerificar>();

            int idUtente = 0;
            foreach(UtenteIdentificado utente in utentesIdentificados)
            {
                if (utente == null)
                    continue;
                //Caso não existam encodings na base de dados.
                if (todosEncoding.Count ==0)
                {
                    UtenteVerificar ut = new UtenteVerificar();
                    ut.Right = utente.Right;
                    ut.Left = utente.Left;
                    ut.Top = utente.Top;
                    ut.Bottom = utente.Bottom;
                    ut.Encoding = utente.Encoding;
                    utentesPorVerificar.Add(ut);
                }
                else
                {
                    foreach(Encoding encodingValue in todosEncoding)
                    {
                        if(novoReconhecimento.verificarEncoding(encodingValue.encoding, utente.Encoding.encoding))
                        {
                            idUtente = encodingValue.UTENTEidUtente;
                            var infoUtente = baseDados.Utente.FirstOrDefault(u => u.idUtente == idUtente);
                            if (infoUtente != null)
                            {
                                utente.Nome = infoUtente.Nome;
                                utente.Id = infoUtente.idUtente;
                            }
                        }
                        else
                        {
                            UtenteVerificar ut = new UtenteVerificar();
                            ut.Right = utente.Right;
                            ut.Left = utente.Left;
                            ut.Top = utente.Top;
                            ut.Bottom = utente.Bottom;
                            ut.Encoding = utente.Encoding;
                            utentesPorVerificar.Add(ut);
                        }
                    }
                }
            }
            string nomeDiretorioAux = "";
            if (utentesPorVerificar.Count > 0)
                nomeDiretorioAux = novoReconhecimento.MostrarNaoIdentificados(nomeFicheiro,nomeDiretorio, utentesPorVerificar);
            var fotoParaVerificar = new
            {
                nomeFoto = nomeFicheiro,
                listaNaoIdentificados = utentesPorVerificar,
                fotoOriginal = nomeDiretorio,
                diretorioFoto = nomeDiretorioAux,
            };
            return Ok(Json(fotoParaVerificar));
        }
        /// <summary>
        /// Na imagem caso seja clicado numa cara e escolhido a opção de censurar, irá verificar se está dentro dos limites identificados
        /// e recorrer à censura.
        /// </summary>
        /// <param name="imagemOriginal"></param>
        /// <param name="nomeFoto"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="utentesPorVerificar"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RealizarDesfoque")] 
        public async Task<IActionResult> Desfoque([FromForm] string imagemOriginal, [FromForm] string nomeFoto, [FromForm] int posX, [FromForm] int posY, [FromForm] string utentesPorVerificar)
        {
            if (nomeFoto == null || posX == 0 || posY == 0 || utentesPorVerificar == null)
                return BadRequest();    

            FotoComDesfoque novoDesfoque = new FotoComDesfoque();
            List<UtenteVerificar> listaDesfoque = JsonConvert.DeserializeObject<List<UtenteVerificar>>(utentesPorVerificar);
            
            novoDesfoque.AplicarDesfoque(nomeFoto, imagemOriginal, posX, posY, listaDesfoque);
            return Ok();
        }
        /// <summary>
        /// Realiza o registo de uma pessoa pelo click.
        /// </summary>
        /// <param name="imagemOriginal"></param>
        /// <param name="nomeFoto"></param>
        /// <param name="nome"></param>
        /// <param name="val"></param>
        /// <param name="sala"></param>
        /// <param name="aut"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="utentesPorVerificar"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RealizarRegisto")]
        public async Task<IActionResult> AdicionarUtente([FromForm] string imagemOriginal, [FromForm] string nomeFoto, [FromForm] string nome, [FromForm] string val, [FromForm] string sala, [FromForm] int aut, [FromForm] int posX, [FromForm] int posY, [FromForm] string utentesPorVerificar)
        {
            if (val == null || posX <= 0 || posY <= 0 || sala == null || aut == 0 || nome == null || utentesPorVerificar == null)
                return BadRequest();

            FotoComDesfoque novoRegisto = new FotoComDesfoque();
            List<UtenteVerificar> listaRegisto = JsonConvert.DeserializeObject<List<UtenteVerificar>>(utentesPorVerificar);
            Utente novoUtente = novoRegisto.AdicionarUtente(nomeFoto, imagemOriginal, posX, posY, listaRegisto, val, sala, aut, nome);

            baseDados.Utente.Add(novoUtente);
            baseDados.SaveChangesAsync();
            return Ok();
        }

    }
}
