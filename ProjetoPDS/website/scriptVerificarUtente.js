document.getElementById('verifyPhoto').addEventListener('submit', async function(event) {
    event.preventDefault();
    
    const formData = new FormData();
    const fileInput = document.querySelector('input[type="file"]');
    formData.append('foto', fileInput.files[0]);
    
    try {
        const apiURLPhoto = 'https://localhost:7248/Utente/verificarUtente';
        const requestOptions = {
            method: 'POST',
            body: formData, // Include the FormData object as the request body
        };
        
        const response = await fetch(apiURLPhoto, requestOptions);
        const responseData = await response.json();

        // Log the response data to the console
        console.log(responseData);

        // Check the response data and handle it accordingly
        // For example, you can update the UI based on the response

    } catch (error) {
        console.error(error);
        alert('An error occurred while verifying the photo.');
    }
});
