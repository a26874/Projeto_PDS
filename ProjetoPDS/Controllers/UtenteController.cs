using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPDS.Classes;
using System.Drawing;

namespace ProjetoPDS.Controllers
{
    /// <summary>
    /// Controlador para utentes.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UtenteController : Controller
    {
        private readonly dataBase baseDados;
        public UtenteController(dataBase baseDados) 
        { 
            this.baseDados = baseDados;
        }
        /// <summary>
        /// Realiza o registo de um utente.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="val"></param>
        /// <param name="sala"></param>
        /// <param name="aut"></param>
        /// <param name="utentesPorVerificar"></param>
        /// <param name="corP"></param>
        /// <param name="corS"></param>
        /// <param name="corT"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RealizarRegisto")]
        public async Task<IActionResult> AdicionarUtente([FromForm] string nome, [FromForm] string val, [FromForm] string sala, [FromForm] int aut, [FromForm] string utentesPorVerificar
                                                         , [FromForm] int corP, [FromForm] int corS, [FromForm] int corT)
        {
            if (val == null ||  sala == null || aut == 0 || nome == null || utentesPorVerificar == null)
                return BadRequest();
            //Aqui recebe a lista de todos os utentes do javascript e converte numa lista de utentes para verificar.
            FotoComDesfoque novoRegisto = new FotoComDesfoque();
            List<UtenteVerificar> listaRegisto = JsonConvert.DeserializeObject<List<UtenteVerificar>>(utentesPorVerificar);

            //É criado um novo dicionário de utentes e encodings, que irá receber do método AdicionarUtenteBaseDados.
            Dictionary<Utente, Encoding> listaNovosUtentes = novoRegisto.AdicionarUtenteBaseDados(listaRegisto, val, sala, aut, nome, corP, corS, corT);

            //Por cada utente no dicionário, obtém o utente e o encoding.
            //Verifica se existe pelo nome (deveria ser pelo ID agora que penso) e caso exista o utente adiciona apenas o encoding.
            //Caso contrário adiciona o utente e o encoding.
            foreach (var u in listaNovosUtentes)
            {
                Utente utente = u.Key;
                Encoding encoding = u.Value;

                var existeUtente = baseDados.Utente.FirstOrDefault(u => u.Nome == utente.Nome);
                if (existeUtente != null)
                {
                    try
                    {
                        encoding.UTENTEidUtente = existeUtente.idUtente;
                        baseDados.Encoding.Add(encoding);
                        await baseDados.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        return StatusCode(500, "Ocorreu um erro ao processar o seu pedido. " + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        baseDados.Utente.Add(utente);
                        await baseDados.SaveChangesAsync();
                        encoding.UTENTEidUtente = utente.idUtente;
                        baseDados.Encoding.Add(encoding);
                        await baseDados.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        return StatusCode(500,"Ocorreu um erro ao processar o seu pedido. "+ ex.Message);
                    }
                }
            }
            return Ok(string.Format("O utente com o nome:{0} foi inserido com sucesso.",nome));
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
                return BadRequest("Verifique os dados inseridos.");

            //Convertemos para uma lista de utentes verificados do C#
            List<UtenteIdentificado> listaExistentes = JsonConvert.DeserializeObject<List<UtenteIdentificado>>(utentesVerificados);

            //Caso ele exista, é atribuido os novos campos que foram alterados.
            var existeUtente = baseDados.Utente.FirstOrDefault(u => u.idUtente == idUtente);
            if (existeUtente != null)
            {
                existeUtente.Nome = nome;
                existeUtente.Sala = sala;
                existeUtente.Valencia = val;
                existeUtente.Autorizacao = aut;

                try
                {
                    baseDados.Update(existeUtente);
                    await baseDados.SaveChangesAsync();

                    return Ok(string.Format("Os dados do utilizador {0}",existeUtente.idUtente+ " foram alterados com sucesso."));
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Ocorreu um erro ao processar o seu pedido. "+ ex.Message);
                }
            }
            return NotFound(string.Format("O utente com o id: {0} não existe.",idUtente));
        }

        [HttpGet]
        [Route("ObterUtente/{idUtente}")]
        public async Task<IActionResult> ObterUtente(int idUtente)
        {
            if (idUtente <= 0)
                return BadRequest(string.Format("Id de utente invalido, ID: {0}.", idUtente));

            try
            {
                var obterUtenteBd = baseDados.Utente.FirstOrDefault(u => u.idUtente == idUtente);
                if (obterUtenteBd == null)
                    return NotFound(string.Format("Não foi encontrado o utente com o ID:{0}",idUtente));

                return Ok(obterUtenteBd);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro a processar o seu pedido. "+ ex.Message);
            }
        }
        /// <summary>
        /// Apaga um utente.
        /// </summary>
        /// <param name="idUtente"></param>
        /// <returns></returns>

        [HttpDelete]
        [Route("ApagarUtente/{idUtente}")]
        public async Task<IActionResult> ApagarUtente(int idUtente)
        {
            if(idUtente<= 0)
                return BadRequest(string.Format("Id de utente invalido, ID: {0}.", idUtente));
            try
            {
                var obterUtenteBd = baseDados.Utente.FirstOrDefault(u => u.idUtente == idUtente);

                if (obterUtenteBd == null)
                    return NotFound(string.Format("Não foi encontrado o utente com o ID: {0}", idUtente));

                var obterEncodingUtente = baseDados.Encoding.Where(e => e.UTENTEidUtente == obterUtenteBd.idUtente);

                
                baseDados.Encoding.RemoveRange(obterEncodingUtente);
                baseDados.Utente.Remove(obterUtenteBd);

                await baseDados.SaveChangesAsync();

                return Ok(string.Format("Foi apagado o utente com ID: {0} e os seus respetivos encodings.", obterUtenteBd.idUtente));
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao processar o seu pedido. " + ex.Message);
            }
        }
    }
}
