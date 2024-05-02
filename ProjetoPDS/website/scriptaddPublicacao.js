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
        //Caso existam.
        // const utentesVerificados = 
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
                //Criar também para adicionar baseado no click
                const sendCoordButton = document.createElement('button');
                sendCoordButton.textContent = 'Desfocar';
                sendCoordButton.addEventListener('click', function() {
                    enviarCoords(auxPosX, auxPosY, absolutePath, fotoOriginal, utentesVerificar,nomeFotoFicheiro);
                });

                //Criação de tabela com dados
                const tabelaDados = document.createElement('table');
                tabelaDados.style.border = "1px solid black";
                bodyTag.appendChild(tabelaDados);
                const headerTabelaDados = document.createElement('tr');
                tabelaDados.append(headerTabelaDados);
                //Criação de headers.
                const headerDadosNome = document.createElement('th');
                headerDadosNome.innerHTML = "Nome";
                headerDadosNome.style.border = "1px solid black";
                headerTabelaDados.append(headerDadosNome);

                const headerDadosValencia = document.createElement('th');
                headerDadosValencia.innerHTML = "Valencia";
                headerDadosValencia.style.border = "1px solid black";
                headerTabelaDados.append(headerDadosValencia);

                const headerDadosSala = document.createElement('th');
                headerDadosSala.innerHTML = "Sala";
                headerDadosSala.style.border = "1px solid black";
                headerTabelaDados.append(headerDadosSala);
 
                const headerDadosAutorizacao = document.createElement('th');
                headerDadosAutorizacao.innerHTML = "Autorizacao";
                headerDadosAutorizacao.style.border = "1px solid black";
                headerTabelaDados.append(headerDadosAutorizacao);

                const headerDadosCor = document.createElement('th');
                headerDadosCor.innerHTML = "Cor";
                headerDadosCor.style.border = "1px solid black";
                headerTabelaDados.append(headerDadosCor);

                for(let i = 0 ; i < utentesVerificar.length; i++)
                    {
                        // Cria nova linha
                        const linha = document.createElement('tr');
                        linha.setAttribute('id', 'linhaNum' + (i + 1));
                        tabelaDados.append(linha);

                        // Cria célula para nome de utilizador
                        const nomeTd = document.createElement('td');
                        linha.append(nomeTd);
                        const nomeUtilizadorInput = document.createElement('input');
                        nomeUtilizadorInput.setAttribute('id', 'nomeUtente' + (i + 1));
                        nomeUtilizadorInput.placeholder = "Nome utilizador " + (i + 1);
                        nomeTd.append(nomeUtilizadorInput);

                        // Cria célula para valência
                        const valenciaTd = document.createElement('td');
                        linha.append(valenciaTd);
                        const valenciaUtilizadorInput = document.createElement('input');
                        valenciaUtilizadorInput.setAttribute('id', 'valencia' + (i + 1));
                        valenciaUtilizadorInput.placeholder = "Valencia";
                        valenciaTd.append(valenciaUtilizadorInput);

                        // Cria célula para sala
                        const salaTd = document.createElement('td');
                        linha.append(salaTd);
                        const salaUtilizadorInput = document.createElement('input');
                        salaUtilizadorInput.setAttribute('id', 'Sala' + (i + 1));
                        salaUtilizadorInput.placeholder = "Sala";
                        salaTd.append(salaUtilizadorInput);

                        // Cria célula para autorização
                        const autTd = document.createElement('td');
                        linha.append(autTd);
                        const autUtilizadorInput = document.createElement('input');
                        autUtilizadorInput.setAttribute('id', 'aut' + (i + 1));
                        autUtilizadorInput.placeholder = "Autorizacao";
                        autTd.append(autUtilizadorInput);

                        // Cria célula para cor do utilizador
                        const corTd = document.createElement('td');
                        linha.append(corTd);
                        const corUtilizador = document.createElement('span');
                        const primeiraCor = utentesVerificar[i].primeiraCor;
                        const segundaCor = utentesVerificar[i].segundaCor;
                        const terceiraCor = utentesVerificar[i].terceiraCor;
                        corUtilizador.setAttribute('id', 'cor' + (i + 1));
                        corUtilizador.style.backgroundColor = `rgb(${primeiraCor}, ${segundaCor}, ${terceiraCor})`;
                        corUtilizador.innerHTML='.'
                        corTd.append(corUtilizador);
                    }

                //Para adicionar o nome do utilizador, valencia, sala, aut, criar todas as caixas de text para o mesmo.

                const sendCoordButtonAdd = document.createElement('button');
                sendCoordButtonAdd.textContent = 'Adicionar Pessoa';
                sendCoordButtonAdd.addEventListener('click',function(){
                    for (let i = 0; i < utentesVerificar.length; i++)
                    {
                        const nomeUtilizadorId = document.getElementById('nomeUtente'+(i+1));
                        const valenciaUtilizadorId = document.getElementById('valencia'+(i+1));
                        const salaUtilizadorId = document.getElementById('Sala'+(i+1));
                        const autUtilizadorId = document.getElementById('aut'+(i+1));
                        const corUtilizadorId = document.getElementById('cor'+(i+1));
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
                        enviarDadosPessoa(auxPosX, auxPosY, nomeUtilizador, valenciaUtilizador, salaUtilizador,
                            autUtilizador, fotoOriginal, nomeFotoFicheiro, utentesVerificar, primeiraCor, segundaCor,
                                terceiraCor);        
                    }
                });
                bodyTag.appendChild(sendCoordButtonAdd);
                bodyTag.appendChild(sendCoordButton);
                buttonCreated = true; 
            }
        })
    } catch (error) {
        console.error(error);
        alert('An error occurred while uploading the photo.');
    }
});

function getRelativePath(absolutePath)
{
    const projectPath = 'C:/Users/marco/source/repos/Projeto_PDS/ProjetoPDS/';
    const relativePath = '../' + absolutePath.substring(projectPath.length);
    return relativePath;
}

async function enviarCoords(posX, posY,absolutePath, nomeDiretorio, utentesVerificar, nomeFotoFicheiro)
{
    alert("Estou a enviar as coordenadas.");
    try{
        const formdataPos = new FormData();
        formdataPos.append('posX',posX);
        formdataPos.append('posY',posY);
        formdataPos.append('urlFoto', absolutePath)
        formdataPos.append('imagemOriginal', nomeDiretorio)
        formdataPos.append('utentesPorVerificar', JSON.stringify(utentesVerificar))
        formdataPos.append('nomeFoto', nomeFotoFicheiro)
        const apiURL = 'https://localhost:7248/Publicacao/RealizarDesfoque';
        const requestOptionsPos = {
            method: 'POST',
            body: formdataPos,
        };
        
        const response = await fetch(apiURL, requestOptionsPos);
        const data = await response.json();
        console.log(data)
    }
    catch (error){
        console.error(error);
        alert('Ocorreu um erro.');
    }
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