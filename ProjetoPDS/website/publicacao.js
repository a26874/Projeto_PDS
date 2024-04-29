document.getElementById('realizarPublicacao').addEventListener('submit',async function(event){
    event.preventDefault();

    const formData = new FormData();
    const fileInput = document.querySelector('input[type="file"]')
    formData.append('caminhoFoto', fileInput.files[0]);

    const idUtilizador = document.getElementById('idUtilizador').value;
    const tipoPublicacao = document.getElementById('tipoPublicacao').value;
    const timestampPub = new Date().toLocaleString()
    formData.append('publicacao.IdUtilizador', idUtilizador);
    formData.append('publicacao.dataPublicacao', timestampPub);
    
    try{
        const apiURLPhoto = 'https://localhost:7248/VerificarPublicacao';
        const requestOptions = {
            method: 'POST',
            body: formData,
        };

        const response = await fetch(apiURLPhoto, requestOptions);
        const responseData = await response.json();

        console.log(responseData);


    }
    catch(error)
    {
        console.error(error);
        alert('Ocorreu um erro.');
    }
});