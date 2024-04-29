document.getElementById('addUserPhoto').addEventListener('submit', async function(event) {
    event.preventDefault();
    
    const formData = new FormData();
    const fileInput = document.querySelector('input[type="file"]');
    formData.append('foto', fileInput.files[0]);

    const nomeUtente = document.getElementById('Nome').value;
    const valencia = document.getElementById('Valencia').value;
    const sala = document.getElementById('Sala').value;
    const autorizacao = document.getElementById('Autorizacao').value;
    formData.append('utente.Nome', nomeUtente);
    formData.append('utente.Valencia', valencia);
    formData.append('utente.Sala', sala);
    formData.append('utente.Autorizacao', autorizacao);
    
    try {
        const apiURLPhoto = 'https://localhost:7248/Utente/addSingleUtente';
        const requestOptions = {
            method: 'POST',
            body: formData,
        };
        const response = await fetch(apiURLPhoto, requestOptions);
        const responseData = await response.json();

        // Log the response data to the console
        console.log(responseData);

        // Update the src attribute of the img element with the uploaded photo URL
        document.getElementById('uploadedPhoto').src = responseData.photo_path;
    } catch (error) {
        console.error(error);
        alert('An error occurred while uploading the photo.');
    }
});
