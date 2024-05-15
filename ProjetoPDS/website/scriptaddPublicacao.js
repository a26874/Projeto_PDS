document.getElementById('addPublicacao').addEventListener('submit', async function(event) {
    event.preventDefault();
    
    const bodyTag = document.body
    const formData = new FormData();
    const fileInput = document.querySelector('input[type="file"]');
    formData.append('foto', fileInput.files[0]);
    

    const idUtlz = document.getElementById('idUtlz').value;
    const currentDate = new Date();
    const dataPub = currentDate.getDate();

    const localPub = document.getElementById('localPostagem').value;
    formData.append('infopublicacao.idUtilizador', idUtlz);
    formData.append('infopublicacao.dataPublicacao',dataPub);
    formData.append('infopublicacao.Local',localPub)
    try {
        const apiURLPhoto = 'https://localhost:7248/Publicacao/VerificarPublicacao';
        const requestOptions = {
            method: 'POST',
            body: formData,
        };
        
        const response = await fetch(apiURLPhoto, requestOptions);
        const data = await response.json();
         
        let buttonCreated = false;

        const absolutePath = data.value.diretorioFoto;
        const fotoOriginal = data.value.fotoOriginal;
        const utentesVerificar = data.value.listaNaoIdentificados;
        const utentesVerificados = data.value.listaIdentificados;
        const nomeFotoFicheiro = data.value.nomeFoto;
        const relativePath = getRelativePath(absolutePath);
        const fotoParaVerificar = document.getElementById('fotoParaVerificar')
        fotoParaVerificar.src = relativePath;
        fotoParaVerificar.addEventListener('click', function(event) {
            const rect = fotoParaVerificar.getBoundingClientRect();
            const posX = event.clientX - rect.left;
            const posY = event.clientY - rect.top;
            let auxPosX = parseInt(posX);
            let auxPosY = parseInt(posY);
            document.getElementById('posX').innerHTML= "Posicao X:" + auxPosX;
            document.getElementById('posY').innerHTML= "Posicao Y:" + auxPosY;

            if (!buttonCreated) {

                const headerTabelaReconhecidos = document.createElement('h1')
                headerTabelaReconhecidos.innerHTML = "Pessoas Reconhecidas: " + utentesVerificados.length
                bodyTag.appendChild(headerTabelaReconhecidos)
                //Criação de tabela com dados de utentes verificados.
                const tabelaDadosIdentificados = document.createElement('table');
                tabelaDadosIdentificados.style.border = "1px solid black";
                bodyTag.appendChild(tabelaDadosIdentificados);
                const headerTabelaDadosIdentificados = document.createElement('tr');
                tabelaDadosIdentificados.append(headerTabelaDadosIdentificados);
                //Criação de headers.
                const headerDadosNomeIdentificados = document.createElement('th');
                headerDadosNomeIdentificados.innerHTML = "Nome";
                headerDadosNomeIdentificados.style.border = "1px solid black";
                headerTabelaDadosIdentificados.append(headerDadosNomeIdentificados);

                const headerDadosValenciaIdentificados = document.createElement('th');
                headerDadosValenciaIdentificados.innerHTML = "Valencia";
                headerDadosValenciaIdentificados.style.border = "1px solid black";
                headerTabelaDadosIdentificados.append(headerDadosValenciaIdentificados);

                const headerDadosSalaIdentificados = document.createElement('th');
                headerDadosSalaIdentificados.innerHTML = "Sala";
                headerDadosSalaIdentificados.style.border = "1px solid black";
                headerTabelaDadosIdentificados.append(headerDadosSalaIdentificados);
 
                const headerDadosAutorizacaoIdentificados = document.createElement('th');
                headerDadosAutorizacaoIdentificados.innerHTML = "Autorizacao";
                headerDadosAutorizacaoIdentificados.style.border = "1px solid black";
                headerTabelaDadosIdentificados.append(headerDadosAutorizacaoIdentificados);

                const headerDadosCorIdentificados = document.createElement('th');
                headerDadosCorIdentificados.innerHTML = "Cor";
                headerDadosCorIdentificados.style.border = "1px solid black";
                headerTabelaDadosIdentificados.append(headerDadosCorIdentificados);


                // Auxiliares para verificar se algum dos dados foram modificados.
                const auxNomeUtilizadorInput = []
                const auxValenciaUtilizadorInput = []
                const auxSalaUtilizadorInput = []
                const auxAutorizacaoInput = []
                for(let i = 0 ; i < utentesVerificados.length; i++)
                    {
                        // Cria nova linha
                        const linha = document.createElement('tr');
                        linha.setAttribute('id', 'linhaNum' + (i + 1));
                        tabelaDadosIdentificados.append(linha);

                        // Cria célula para nome de utilizador
                        const nomeTd = document.createElement('td');
                        linha.append(nomeTd);
                        const nomeUtilizadorInput = document.createElement('input');
                        nomeUtilizadorInput.setAttribute('id', 'nomeUtenteIdentificado' + (i + 1));
                        nomeUtilizadorInput.placeholder = "Nome utilizador " + (i + 1);
                        if(utentesVerificados[i].nome != null && i < utentesVerificados.length)
                            nomeUtilizadorInput.value = utentesVerificados[i].nome
                        nomeTd.append(nomeUtilizadorInput);

                        // Cria célula para valência
                        const valenciaTd = document.createElement('td');
                        linha.append(valenciaTd);
                        const valenciaUtilizadorInput = document.createElement('input');
                        valenciaUtilizadorInput.setAttribute('id', 'valenciaIdentificado' + (i + 1));
                        valenciaUtilizadorInput.placeholder = "Valencia";
                        if(utentesVerificados[i].valencia != null && i < utentesVerificados.length)
                            valenciaUtilizadorInput.value = utentesVerificados[i].valencia
                        valenciaTd.append(valenciaUtilizadorInput);

                        // Cria célula para sala
                        const salaTd = document.createElement('td');
                        linha.append(salaTd);
                        const salaUtilizadorInput = document.createElement('input');
                        salaUtilizadorInput.setAttribute('id', 'SalaIdentificado' + (i + 1));
                        salaUtilizadorInput.placeholder = "Sala";
                        if(utentesVerificados[i].sala != null && i < utentesVerificados.length)
                            salaUtilizadorInput.value = utentesVerificados[i].sala
                        salaTd.append(salaUtilizadorInput);

                        // Cria célula para autorização
                        const autTd = document.createElement('td');
                        linha.append(autTd);
                        const autUtilizadorInput = document.createElement('input');
                        autUtilizadorInput.setAttribute('id', 'autIdentificado' + (i + 1));
                        autUtilizadorInput.placeholder = "Autorizacao";
                        if(utentesVerificados[i].autorizacao!=null && i < utentesVerificados.length)
                            autUtilizadorInput.value = utentesVerificados[i].autorizacao
                        autTd.append(autUtilizadorInput);

                        // Cria célula para cor do utilizador
                        const corTd = document.createElement('td');
                        linha.append(corTd);
                        const corUtilizador = document.createElement('span');
                        var primeiraCor = 0
                        var segundaCor = 0
                        var terceiraCor = 0
                        primeiraCor = utentesVerificados[i].primeiraCor;   
                        segundaCor = utentesVerificados[i].segundaCor;   
                        terceiraCor = utentesVerificados[i].terceiraCor;   
                        corUtilizador.setAttribute('id', 'corIdentificado' + (i + 1));
                        corUtilizador.style.backgroundColor = `rgb(${primeiraCor}, ${segundaCor}, ${terceiraCor})`;
                        corUtilizador.innerHTML='.'
                        corTd.append(corUtilizador);
                        auxNomeUtilizadorInput.push(nomeUtilizadorInput.value);
                        auxValenciaUtilizadorInput.push(valenciaUtilizadorInput.value);
                        auxSalaUtilizadorInput.push(salaUtilizadorInput.value);
                        auxAutorizacaoInput.push(autUtilizadorInput.value);
                    }
                    const sendDataToEdit = document.createElement('button');
                    sendDataToEdit.textContent = 'Editar Pessoa(s)';
                    sendDataToEdit.addEventListener('click',async function(){
                        for (let i = 0; i < utentesVerificados.length; i++)
                        {
                            const nomeUtilizadorId = document.getElementById('nomeUtenteIdentificado'+(i+1));
                            const valenciaUtilizadorId = document.getElementById('valenciaIdentificado'+(i+1));
                            const salaUtilizadorId = document.getElementById('SalaIdentificado'+(i+1));
                            const autUtilizadorId = document.getElementById('autIdentificado'+(i+1));
                            const corUtilizadorId = document.getElementById('corIdentificado'+(i+1));
                            const idUtilizadorBd = utentesVerificados[i].id;
                            const valorCor = window.getComputedStyle(corUtilizadorId);
                            const valorCorAux = valorCor.backgroundColor;
                            if(auxNomeUtilizadorInput[i] == nomeUtilizadorId.value && 
                               auxValenciaUtilizadorInput[i] == valenciaUtilizadorId.value &&
                               auxSalaUtilizadorInput[i] == salaUtilizadorId.value &&
                               auxAutorizacaoInput[i] == autUtilizadorId.value )
                               continue;
                            nomeUtilizador = nomeUtilizadorId.value;
                            valenciaUtilizador = valenciaUtilizadorId.value;
                            salaUtilizador = salaUtilizadorId.value;
                            autUtilizador = autUtilizadorId.value;
                            const rgbValues = valorCorAux.match(/\d+/g); 
                            var primeiraCor = 0
                            var segundaCor = 0
                            var terceiraCor = 0
                            if (rgbValues && rgbValues.length === 3) {
                                primeiraCor = parseInt(rgbValues[0]);
                                segundaCor = parseInt(rgbValues[1]);
                                terceiraCor = parseInt(rgbValues[2]); 
                            }
                            await editarDadosPessoa(idUtilizadorBd,nomeUtilizador, valenciaUtilizador, salaUtilizador,
                                autUtilizador, utentesVerificados, primeiraCor, segundaCor,
                                    terceiraCor);        
                        }
                    })
                bodyTag.appendChild(sendDataToEdit)
                const headerTabelaNaoReconhecidos = document.createElement('h1')
                headerTabelaNaoReconhecidos.innerHTML = "Pessoas Não Reconhecidas: " + utentesVerificar.length
                bodyTag.appendChild(headerTabelaNaoReconhecidos)
                    
                //Criação de tabela para os utentes não verificados.
                const tabelaDadosNaoIdentificados = document.createElement('table');
                tabelaDadosNaoIdentificados.style.border = "1px solid black";
                bodyTag.appendChild(tabelaDadosNaoIdentificados);
                const headerTabelaDadosNaoIdentificados = document.createElement('tr');
                tabelaDadosNaoIdentificados.append(headerTabelaDadosNaoIdentificados);
                //Criação de headers.
                const headerDadosNomeNaoIdentificados = document.createElement('th');
                headerDadosNomeNaoIdentificados.innerHTML = "Nome";
                headerDadosNomeNaoIdentificados.style.border = "1px solid black";
                headerTabelaDadosNaoIdentificados.append(headerDadosNomeNaoIdentificados);

                const headerDadosValenciaNaoIdentificados = document.createElement('th');
                headerDadosValenciaNaoIdentificados.innerHTML = "Valencia";
                headerDadosValenciaNaoIdentificados.style.border = "1px solid black";
                headerTabelaDadosNaoIdentificados.append(headerDadosValenciaNaoIdentificados);

                const headerDadosSalaNaoIdentificados = document.createElement('th');
                headerDadosSalaNaoIdentificados.innerHTML = "Sala";
                headerDadosSalaNaoIdentificados.style.border = "1px solid black";
                headerTabelaDadosNaoIdentificados.append(headerDadosSalaNaoIdentificados);
 
                const headerDadosAutorizacaoNaoIdentificados = document.createElement('th');
                headerDadosAutorizacaoNaoIdentificados.innerHTML = "Autorizacao";
                headerDadosAutorizacaoNaoIdentificados.style.border = "1px solid black";
                headerTabelaDadosNaoIdentificados.append(headerDadosAutorizacaoNaoIdentificados);

                const headerDadosCorNaoIdentificados = document.createElement('th');
                headerDadosCorNaoIdentificados.innerHTML = "Cor";
                headerDadosCorNaoIdentificados.style.border = "1px solid black";
                headerTabelaDadosNaoIdentificados.append(headerDadosCorNaoIdentificados);

                
                for(let i = 0; i < utentesVerificar.length; i++)
                    {
                        // Cria nova linha
                        const linha = document.createElement('tr');
                        linha.setAttribute('id', 'linhaNum' + (i + 1));
                        tabelaDadosNaoIdentificados.append(linha);

                        // Cria célula para nome de utilizador
                        const nomeTd = document.createElement('td');
                        linha.append(nomeTd);
                        const nomeUtilizadorInput = document.createElement('input');
                        nomeUtilizadorInput.setAttribute('id', 'nomeUtenteNaoIdentificado' + (i + 1));
                        nomeUtilizadorInput.placeholder = "Nome utilizador " + (i + 1);
                        nomeTd.append(nomeUtilizadorInput);

                        // Cria célula para valência
                        const valenciaTd = document.createElement('td');
                        linha.append(valenciaTd);
                        const valenciaUtilizadorInput = document.createElement('input');
                        valenciaUtilizadorInput.setAttribute('id', 'valenciaNaoIdentificado' + (i + 1));
                        valenciaUtilizadorInput.placeholder = "Valencia";
                        valenciaTd.append(valenciaUtilizadorInput);

                        // Cria célula para sala
                        const salaTd = document.createElement('td');
                        linha.append(salaTd);
                        const salaUtilizadorInput = document.createElement('input');
                        salaUtilizadorInput.setAttribute('id', 'SalaNaoIdentificado' + (i + 1));
                        salaUtilizadorInput.placeholder = "Sala";
                        salaTd.append(salaUtilizadorInput);

                        // Cria célula para autorização
                        const autTd = document.createElement('td');
                        linha.append(autTd);
                        const autUtilizadorInput = document.createElement('input');
                        autUtilizadorInput.setAttribute('id', 'autNaoIdentificado' + (i + 1));
                        autUtilizadorInput.placeholder = "Autorizacao";
                        autTd.append(autUtilizadorInput);

                        // Cria célula para cor do utilizador
                        const corTd = document.createElement('td');
                        linha.append(corTd);
                        const corUtilizador = document.createElement('span');
                        var primeiraCor = 0
                        var segundaCor = 0
                        var terceiraCor = 0
                        primeiraCor = utentesVerificar[i].primeiraCor;   
                        segundaCor = utentesVerificar[i].segundaCor;   
                        terceiraCor = utentesVerificar[i].terceiraCor;   
                        corUtilizador.setAttribute('id', 'corNaoIdentificado' + (i + 1));
                        corUtilizador.style.backgroundColor = `rgb(${terceiraCor}, ${segundaCor}, ${primeiraCor})`;
                        corUtilizador.innerHTML='.'
                        corUtilizador.style.color = `rgb(${terceiraCor}, ${segundaCor}, ${primeiraCor})`;
                        corTd.append(corUtilizador);
                    }
                //Para adicionar o nome do utilizador, valencia, sala, aut, criar todas as caixas de text para o mesmo.

                const sendCoordButtonAdd = document.createElement('button');
                sendCoordButtonAdd.textContent = 'Adicionar Pessoa';
                sendCoordButtonAdd.addEventListener('click',async function(){
                    for (let i = 0; i < utentesVerificar.length; i++)
                    {
                        const nomeUtilizadorId = document.getElementById('nomeUtenteNaoIdentificado'+(i+1));
                        const valenciaUtilizadorId = document.getElementById('valenciaNaoIdentificado'+(i+1));
                        const salaUtilizadorId = document.getElementById('SalaNaoIdentificado'+(i+1));
                        const autUtilizadorId = document.getElementById('autNaoIdentificado'+(i+1));
                        const corUtilizadorId = document.getElementById('corNaoIdentificado'+(i+1));
                        const valorCor = window.getComputedStyle(corUtilizadorId);
                        const valorCorAux = valorCor.backgroundColor;
                        if (nomeUtilizadorId.value == "" && valenciaUtilizadorId.value == "" && salaUtilizadorId.value == "" 
                                && autUtilizadorId.value == "")
                                    continue;
                        nomeUtilizador = nomeUtilizadorId.value;
                        valenciaUtilizador = valenciaUtilizadorId.value;
                        salaUtilizador = salaUtilizadorId.value;
                        autUtilizador = autUtilizadorId.value;
                        const rgbValues = valorCorAux.match(/\d+/g); 
                        var primeiraCor = 0
                        var segundaCor = 0
                        var terceiraCor = 0
                        if (rgbValues && rgbValues.length === 3) {
                            primeiraCor = parseInt(rgbValues[0]);
                            segundaCor = parseInt(rgbValues[1]);
                            terceiraCor = parseInt(rgbValues[2]); 
                        }
                        await enviarDadosPessoa(auxPosX, auxPosY, nomeUtilizador, valenciaUtilizador, salaUtilizador,
                            autUtilizador, fotoOriginal, nomeFotoFicheiro, utentesVerificar, primeiraCor, segundaCor,
                                terceiraCor);        
                    }

                
                });
                const sendPessoaDesfoque = document.createElement('button');
                sendPessoaDesfoque.textContent = 'Desfocar não reconhecidos';
                sendPessoaDesfoque.addEventListener('click', async function(){
                    var auxListaDesfocar = []
                    for(let i = 0; i < utentesVerificar.length; i++)
                        {
                            const nomeUtilizadorId = document.getElementById('nomeUtenteNaoIdentificado'+(i+1));
                            const valenciaUtilizadorId = document.getElementById('valenciaNaoIdentificado'+(i+1));
                            const salaUtilizadorId = document.getElementById('SalaNaoIdentificado'+(i+1));
                            const autUtilizadorId = document.getElementById('autNaoIdentificado'+(i+1));
                            if (nomeUtilizadorId.value == "" && valenciaUtilizadorId.value == "" && salaUtilizadorId.value == "" 
                                    && autUtilizadorId.value == "")
                            {
                                auxListaDesfocar.push(utentesVerificar[i])
                            }
                        }
                            await enviarDadosPessoaDesfoque(fotoOriginal, nomeFotoFicheiro, absolutePath, auxListaDesfocar);       
                    })
                bodyTag.appendChild(sendPessoaDesfoque)
                bodyTag.appendChild(sendCoordButtonAdd);
                // bodyTag.appendChild(sendCoordButton);
                buttonCreated = true; 
            }
        })
    } catch (error) {
        console.error(error);
        alert('An error occurred while uploading the photo. ' + error);
    }
});

function getRelativePath(absolutePath)
{
    /*     
    const projectPath = 'C:/Users/marco/source/repos/Projeto_PDS/ProjetoPDS/website/';
    const relativePath = '../' + absolutePath.substring(projectPath.length);
    return relativePath;*/
    const projectPath = 'C:/VisualStudioProjetos/Projeto_PDS/ProjetoPDS/website';
    const relativePath = '.' + absolutePath.substring(projectPath.length);
    const normalizedrelativePath = relativePath.replace(/\\/g, '/');
    return normalizedrelativePath;
}

async function enviarDadosPessoa(posX, posY, nome, valencia, sala, aut, nomeDiretorio, nomeFotoFicheiro,utentesVerificar, primeiraCor
                                , segundaCor, terceiraCor)
{
    alert("Estou a enviar dados para adicionar a bd.");
    try{
        const formDataPessoa = new FormData();
        formDataPessoa.append('imagemOriginal', nomeDiretorio);
        formDataPessoa.append('nomeFoto', nomeFotoFicheiro);
        formDataPessoa.append('nome', nome);
        formDataPessoa.append('val',valencia);
        formDataPessoa.append('sala',sala);
        formDataPessoa.append('aut', aut);
        formDataPessoa.append('posX', posX);
        formDataPessoa.append('posY', posY);
        formDataPessoa.append('utentesPorVerificar', JSON.stringify(utentesVerificar));
        formDataPessoa.append('corP', primeiraCor)
        formDataPessoa.append('corS', segundaCor)
        formDataPessoa.append('corT', terceiraCor)
        
        const apiURL = 'https://localhost:7248/Publicacao/RealizarRegisto';
        const requestOptionsPessoa = {
            method:'POST',
            body: formDataPessoa,
        };

        const response = await fetch(apiURL, requestOptionsPessoa);
        const data = await response.json();
        console.log(data)
    }
    catch (error){
        console.error(error);
        alert('Ocorreu um erro.');
    }
}

async function editarDadosPessoa(idUtilizadorBd, nome, valencia, sala, aut, utentesVerificados, primeiraCor, segundaCor, terceiraCor)
{
    alert('Estou a enviar dados para alterar uma pessoa.')
    try
    {
        const formDataEditPessoa = new FormData();
        formDataEditPessoa.append('idUtente', idUtilizadorBd);
        formDataEditPessoa.append('nome', nome);
        formDataEditPessoa.append('val',valencia);
        formDataEditPessoa.append('sala',sala);
        formDataEditPessoa.append('aut', aut);
        formDataEditPessoa.append('utentesVerificados', JSON.stringify(utentesVerificados));
        formDataEditPessoa.append('corP', primeiraCor)
        formDataEditPessoa.append('corS', segundaCor)
        formDataEditPessoa.append('corT', terceiraCor)
        const apiURL = 'https://localhost:7248/Publicacao/EditarRegisto';
        const requestOptionsPessoa = {
            method:'PUT',
            body: formDataEditPessoa,
        };

        const response = await fetch(apiURL, requestOptionsPessoa);
        const data = await response.json();
        console.log(data)
    }
    catch (error){
        console.error(error);
        alert('Ocorreu um erro.');
    }       
}

async function enviarDadosPessoaDesfoque(fotoOriginal, nomeFotoFicheiro, absolutePath, utentesVerificar)
{
    // esVerificar[i].bottom;
    //                             await enviarDadosPessoaDesfoque(fotoOriginal, nomeFotoFicheiro, absolutePath, faceLeft,faceTop,faceRight
    //                                 ,faceBottom);       
    alert('Estou a desfocar pessoas.')
    try
    {
        const formDataDesfoquePessoa = new FormData();
        formDataDesfoquePessoa.append('fotoOriginal', fotoOriginal);
        formDataDesfoquePessoa.append('nomeFotoFicheiro', nomeFotoFicheiro);
        formDataDesfoquePessoa.append('absolutePath', absolutePath);
        formDataDesfoquePessoa.append('utentesPorVerificar', JSON.stringify(utentesVerificar))
        const apiURL = 'https://localhost:7248/Publicacao/RealizarDesfoque';
        const requestOptions = {
            method:'POST',
            body: formDataDesfoquePessoa,
        };

        const response = await fetch(apiURL, requestOptions);
        const data = await response.json();
        
        const fotoDesfocada = document.getElementById('fotoDesfocada');
        fotoDesfocada.src = data.value.pathFotoDesfocada
        // for(let i = 0; i < 4 ; i++)
        //     {
        //         const newBreak = document.createElement('br')
        //         bodyTag.appendChild(newBreak);
        //     }
        // const novaImagem = document.createElement('img');
        // novaImagem.src = data.value.pathFotoDesfocada
        // bodyTag.appendChild(novaImagem)
        console.log(data)
    }
    catch (error){
        console.error(error);
        alert('Ocorreu um erro.');
    }
}