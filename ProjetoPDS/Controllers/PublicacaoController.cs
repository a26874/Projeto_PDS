﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPDS.Classes;

namespace ProjetoPDS.Controllers
{
    /// <summary>
    /// Aqui é criado o controlador para um publicação
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PublicacaoController : Controller
    {
        private readonly string guardarFotoAux;
        private readonly string guardarFotoAux2;
        private readonly string guardarFotoCaminho;
        private readonly dataBase baseDados;
        /// <summary>
        /// É criado paths para obter os uploads de publicações.
        /// É recebido também uma conexão do sql.
        /// Esta conexão é definida no appsettings.json
        /// </summary>
        /// <param name="baseDados"></param>
        public PublicacaoController(dataBase baseDados)
        {
            guardarFotoAux = Path.Combine(Directory.GetCurrentDirectory(), "website");
            guardarFotoAux2 = Path.Combine(guardarFotoAux, "Imagens");
            guardarFotoCaminho = Path.Combine(guardarFotoAux2, "Uploads_Publicacoes");

            if (!Directory.Exists(guardarFotoCaminho))
            {
                Directory.CreateDirectory(guardarFotoCaminho);
            }
            this.baseDados = baseDados;
        }

        /// <summary>
        /// Recebe uma publicação e verifica a mesma.
        /// </summary>
        /// <param name="pub"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VerificarPublicacao")]
        public async Task<IActionResult> VerificarPublicacao([FromForm] IFormFile pubFoto, [FromForm] string dataPub, [FromForm] int idUtlz, [FromForm] string local)
        {
            if (pubFoto == null || dataPub == null || idUtlz <= 0|| local == null)
                return BadRequest();
            //Começamos por obter o nome do ficheiro, neste caso é o nome da imagem e o path onde ela vai ser guardada.
            Publicacao novaPublicacao = new Publicacao();
            string nomeFicheiro = Path.GetFileName(pubFoto.FileName);
            string nomeDiretorio = Path.Combine(guardarFotoCaminho, nomeFicheiro);


            if (!Path.Exists(nomeDiretorio))
                using (var stream = new FileStream(nomeDiretorio, FileMode.Create))
                    await pubFoto.CopyToAsync(stream);
            
            DateTime dataPublicacaoAux = Convert.ToDateTime(dataPub);
            novaPublicacao.DataPublicacao = dataPublicacaoAux;
            novaPublicacao.CaminhoFoto = nomeDiretorio;
            novaPublicacao.IdUtilizador = idUtlz;
            if(local!=null)
            {
            if (local == "EncEdc")
                novaPublicacao.Local_Publicacaoid = LocalPublicacao.EncEduc + 1;
            else if (local == "Sala")
                novaPublicacao.Local_Publicacaoid = LocalPublicacao.Sala + 1;
            else if (local == "Mural")
                novaPublicacao.Local_Publicacaoid = LocalPublicacao.Mural + 1;
            else if (local == "Chat")
                novaPublicacao.Local_Publicacaoid = LocalPublicacao.Chat + 1;
            else
                return BadRequest();
            }

            var existePublicacao = baseDados.Publicacao.FirstOrDefault(a => a.Publicacao_id== novaPublicacao.Publicacao_id);
            if(existePublicacao != null)
            {
                novaPublicacao.Publicacao_id = existePublicacao.Publicacao_id;
                baseDados.Publicacao.Add(novaPublicacao);
                await baseDados.SaveChangesAsync();
            }
            else
            {
                baseDados.Publicacao.Add(novaPublicacao);
                await baseDados.SaveChangesAsync();
            }

            //Criado uma nova classe de fotocomdesfoque para identificar os utentes.
            FotoComDesfoque novoReconhecimento = new FotoComDesfoque();

            List<UtenteIdentificado> utentesIdentificados = novoReconhecimento.IdentificarUtentes(nomeDiretorio);
            //É pedido à base de dados todos os encodings para uma lista.
            var todosEncoding = baseDados.Encoding.ToList();
            List<UtenteVerificar> utentesPorVerificar = new List<UtenteVerificar>();

            int idUtente = 0;
            int todosEncodingsIterados = 0;
            int idEncoding = 0;
            //Por cada utente na lista de utentesPorVerificar, inicialmente caso não sejam obtidos nenhum encoding da base de dados
            //É logo criado a lista inteira de cada um dos utentes por verificar.
            foreach (UtenteIdentificado utente in utentesIdentificados)
            {
                todosEncodingsIterados = 0;
                if (utente == null)
                    continue;
                //Caso não existam encodings na base de dados.
                if (todosEncoding.Count == 0)
                {
                    UtenteVerificar ut = new UtenteVerificar();
                    ut.Right = utente.Right;
                    ut.Left = utente.Left;
                    ut.Top = utente.Top;
                    ut.Bottom = utente.Bottom;
                    ut.Encoding = utente.Encoding;
                    ut.Autorizacao = utente.Autorizacao;
                    utentesPorVerificar.Add(ut);
                }
                else
                {
                    //Caso a base de dados retorne algum encoding, vamos percorrer todos os encodings
                    //Caso o verificar encoding retorne true, obtemos o id do utente, que vai ser o que está guardado na base de dados
                    //Como UTENTEidUtente na tabela de encodings.
                    //Obtemos a informação do utente a partir do id.
                    //Obtemos informação do encoding (Penso que já esteja a repetir aqui valores)
                    //Caso ambos recebam valores, é preenchido todos os campos necessários para esse utente.
                    foreach (Encoding encodingValue in todosEncoding)
                    {
                        if (novoReconhecimento.verificarEncoding(encodingValue.encoding, utente.Encoding.encoding))
                        {
                            idUtente = encodingValue.UTENTEidUtente;
                            var infoUtente = baseDados.Utente.FirstOrDefault(u => u.idUtente == idUtente);
                            var infoEncoding = baseDados.Encoding.FirstOrDefault(e => e.UTENTEidUtente == idUtente);
                            if (infoUtente != null && infoEncoding!=null)
                            {
                                utente.Nome = infoUtente.Nome;
                                utente.Id = infoUtente.idUtente;
                                utente.Encoding.idEncoding = infoEncoding.idEncoding;
                                utente.Encoding.UTENTEidUtente = infoEncoding.UTENTEidUtente;
                                Random numerosRandom = new Random();
                                utente.PrimeiraCor = numerosRandom.Next(256);
                                utente.SegundaCor = numerosRandom.Next(256);
                                utente.TerceiraCor = numerosRandom.Next(256);
                                utente.Valencia = infoUtente.Valencia;
                                utente.Sala = infoUtente.Sala;
                                utente.Autorizacao = infoUtente.Autorizacao;
                                break;
                            }
                        }
                        todosEncodingsIterados++;
                        if(todosEncodingsIterados == todosEncoding.Count) 
                        { 
                            UtenteVerificar ut = new UtenteVerificar();
                            ut.Right = utente.Right;
                            ut.Left = utente.Left;
                            ut.Top = utente.Top;
                            ut.Bottom = utente.Bottom;
                            ut.Encoding = utente.Encoding;
                            utentesPorVerificar.Add(ut);
                            break;
                        }   
                    }
                }
            }
            //Aqui é criado uma lista auxiliar, onde apenas irão ficar os utentes que realmente já foram identificados.
            List<UtenteIdentificado> auxListaUtentesIdentificados = new List<UtenteIdentificado>();
            foreach(UtenteIdentificado u in utentesIdentificados)
            {
                if (u.Nome != null)
                    auxListaUtentesIdentificados.Add(u);
            }
            //Criar metodo para mostraridentificados
            string nomeDiretorioAux = "";
            string nomeFicheiroAux = "";
            //Caso a lista tenha algum utente, faz o metodo de MostrarIdentificados.
            if(utentesIdentificados.Count>0)
                nomeDiretorioAux = novoReconhecimento.MostrarIdentificados(nomeFicheiro, nomeDiretorio, auxListaUtentesIdentificados, out nomeFicheiroAux);
            //Caso a lista tenha algum utente por verificar, faz o metodo de MostrarNãoIdentificados.
            if (utentesPorVerificar.Count > 0)
                if(nomeFicheiroAux!="")
                    nomeDiretorioAux = novoReconhecimento.MostrarNaoIdentificados(nomeFicheiroAux, nomeDiretorioAux, utentesPorVerificar);
                else
                    nomeDiretorioAux = novoReconhecimento.MostrarNaoIdentificados(nomeFicheiro, nomeDiretorio, utentesPorVerificar);
            //Aqui envia de volta para o javascript algumas informações que são necessárias para criar tabelas com nomes etc.
            var fotoParaVerificar = new
            {
                nomeFoto = nomeFicheiro,
                listaNaoIdentificados = utentesPorVerificar,
                fotoOriginal = nomeDiretorio,
                diretorioFoto = nomeDiretorioAux,
                listaIdentificados = auxListaUtentesIdentificados, 
            };
            return Ok(Json(fotoParaVerificar));
        }
    }
}
