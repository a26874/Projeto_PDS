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