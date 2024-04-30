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

                //Para adicionar o nome do utilizador, valencia, sala, aut, criar todas as caixas de text para o mesmo.
                const nomeUtilizadorInput = document.createElement('input');
                nomeUtilizadorInput.placeholder = "Nome utilizador";
                bodyTag.appendChild(nomeUtilizadorInput);

                nomeUtilizador = nomeUtilizadorInput.value;

                const valenciaUtilizadorInput = document.createElement('input');
                valenciaUtilizadorInput.placeholder = "Valencia";
                bodyTag.appendChild(valenciaUtilizadorInput);
                valenciaUtilizador = valenciaUtilizadorInput.value;

                const salaUtilizadorInput = document.createElement('input');
                salaUtilizadorInput.placeholder = "Sala";
                bodyTag.appendChild(salaUtilizadorInput);
                salaUtilizador = salaUtilizadorInput.value;

                
                const sendCoordButtonAdd = document.createElement('button');
                sendCoordButtonAdd.textContent = 'Adicionar Pessoa';
                sendCoordButtonAdd.addEventListener('click',function(){
                    enviarDadosPessoa(auxPosX, auxPosY, nomeUtilizador, valenciaUtilizador, salaUtilizador,
                    autUtilizador, fotoOriginal, nomeFotoFicheiro, utentesVerificar);
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
    const path = 'C:/Users/marco/source/repos/Projeto_PDS/ProjetoPDS/';
    const relativePath = '../'+absolutePath.substring(path.length);
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

async function enviarDadosPessoa(posX, posY, nome, valencia, sala, aut, nomeDiretorio, nomeFotoFicheiro,utentesVerificar)
{
    alert("Estou a enviar dados para adicionar a bd.");
    try{
        const formDataPessoa = new FormData();
        formDataPessoa.append('posX', posX);
        formDataPessoa.append('posY', posY);
        formDataPessoa.append('imagemOriginal', nomeDiretorio);
        formDataPessoa.append('nomeFoto', nomeFotoFicheiro);
        formDataPessoa.append('nome', nome);
        formDataPessoa.append('val',valencia);
        formDataPessoa.append('sala',sala);
        formDataPessoa.append('aut', aut);
        formDataPessoa.append('utentesPorVerificar', JSON.stringify(utentesVerificar));

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