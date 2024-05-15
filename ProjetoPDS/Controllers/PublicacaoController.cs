using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> VerificarPublicacao([FromForm] Publicacao pub)
        {
            if (pub == null || pub.Foto == null)
                return BadRequest();
            //Começamos por obter o nome do ficheiro, neste caso é o nome da imagem e o path onde ela vai ser guardada.
            string nomeFicheiro = Path.GetFileName(pub.Foto.FileName);
            string nomeDiretorio = Path.Combine(guardarFotoCaminho, nomeFicheiro);


            if (!Path.Exists(nomeDiretorio))
                using (var stream = new FileStream(nomeDiretorio, FileMode.Create))
                    await pub.Foto.CopyToAsync(stream);

            //Aqui dizemos que o path da foto é o nomeDiretorio.
            pub.CaminhoFoto = nomeDiretorio;

            //baseDados.Publicacao.Add(pub);
            //await baseDados.SaveChangesAsync();

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
                if (nomeFicheiroAux != "") { 
                nomeDiretorioAux = novoReconhecimento.MostrarNaoIdentificados(nomeFicheiroAux, nomeDiretorioAux, utentesPorVerificar);
                }
                else
                {
                    nomeDiretorioAux = novoReconhecimento.MostrarNaoIdentificados(nomeFicheiro, nomeDiretorio, utentesPorVerificar);
                }
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

            var pathFotoDesfocada = novoDesfoque.AplicarDesfoque(fotoOriginal, nomeFotoFicheiro, absolutePath,listaDesfoque);
            return Ok(pathFotoDesfocada);
        }
        /// <summary>
        /// Realiza o registo de uma pessoa pelo click.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="val"></param>
        /// <param name="sala"></param>
        /// <param name="aut"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="utentesPorVerificar"></param>
        /// <param name="corP"></param>
        /// <param name="corS"></param>
        /// <param name="corT"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RealizarRegisto")]
        public async Task<IActionResult> AdicionarUtente([FromForm] string nome, [FromForm] string val, [FromForm] string sala, [FromForm] int aut, [FromForm] int posX, [FromForm] int posY, [FromForm] string utentesPorVerificar
                                                         , [FromForm] int corP, [FromForm] int corS, [FromForm] int corT)
        {
            if (val == null || posX <= 0 || posY <= 0 || sala == null || aut == 0 || nome == null || utentesPorVerificar == null)
                return BadRequest();
            //Aqui recebe a lista de todos os utentes do javascript e converte numa lista de utentes para verificar.
            FotoComDesfoque novoRegisto = new FotoComDesfoque();
            List<UtenteVerificar> listaRegisto = JsonConvert.DeserializeObject<List<UtenteVerificar>>(utentesPorVerificar);
            
            //É criado um novo dicionário de utentes e encodings, que irá receber do método AdicionarUtenteBaseDados.
            Dictionary<Utente, Encoding> listaNovosUtentes = novoRegisto.AdicionarUtenteBaseDados(listaRegisto, val, sala, aut, nome, corP, corS, corT);

            //Por cada utente no dicionário, obtém o utente e o encoding.
            //Verifica se existe pelo nome (deveria ser pelo ID agora que penso) e caso exista o utente adiciona apenas o encoding.
            //Caso contrário adiciona o utente e o encoding.
            foreach(var u in listaNovosUtentes)
            {
                Utente utente = u.Key;
                Encoding encoding = u.Value;
             
                var existeUtente = baseDados.Utente.FirstOrDefault(u => u.Nome == utente.Nome);
                if(existeUtente!=null)
                {
                    encoding.UTENTEidUtente = existeUtente.idUtente;
                    baseDados.Encoding.Add(encoding);
                    await baseDados.SaveChangesAsync();
                }
                else
                {
                    baseDados.Utente.Add(utente);
                    await baseDados.SaveChangesAsync();
                    encoding.UTENTEidUtente = utente.idUtente;
                    baseDados.Encoding.Add(encoding);
                    await baseDados.SaveChangesAsync();
                }
            }
            return Ok();
        }
        /// <summary>
        /// Edita um utente.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="val"></param>
        /// <param name="sala"></param>
        /// <param name="aut"></param>
        /// <param name="utentesVerificados"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("EditarRegisto")]

        public async Task<IActionResult> EditarUtente([FromForm] int idUtente, [FromForm] string nome, [FromForm] string val, [FromForm] string sala, 
            [FromForm] int aut, [FromForm] string utentesVerificados)
        {
            //Aqui fazemos um put. Recebemos do javascript uma lista de utentes verificados
            if (val == null || sala == null || aut == 0 || nome == null || utentesVerificados == null)
                return BadRequest();
            
            //Convertemos para uma lista de utentes verificados do C#
            List<UtenteIdentificado> listaExistentes = JsonConvert.DeserializeObject<List<UtenteIdentificado>>(utentesVerificados);

            //Caso ele exista, é atribuido os novos campos que foram alterados.
            var existeUtente = baseDados.Utente.FirstOrDefault(u => u.idUtente == idUtente);
            if(existeUtente!=null)
            {
                existeUtente.Nome = nome;
                existeUtente.Sala = sala;
                existeUtente.Valencia = val;
                existeUtente.Autorizacao = aut;

                baseDados.Update(existeUtente);
                await baseDados.SaveChangesAsync();
            }
            return Ok();
        }

    }
}
