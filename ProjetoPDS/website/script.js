document.getElementById('uploadPhoto').addEventListener('submit', async function(event) {
    event.preventDefault();
    
    const formData = new FormData();
    const fileInput = document.querySelector('input[type="file"]');
    formData.append('photo', fileInput.files[0]);

    try {
        const apiURLPhoto = 'http://127.0.0.1:5000/sendPhoto'; 
        const requestOptions = {
            method: 'POST',
            body: formData,
        };
        const response = await fetch(apiURLPhoto, requestOptions);
        const responseData = await response.json();

        // Log the response data to the console.
        console.log(responseData);

        // Update the src attribute of the img element with the uploaded photo URL
        document.getElementById('uploadedPhoto').src = responseData.photo_path;
    } catch (error) {
        console.error(error);
        alert('An error occurred while uploading the photo.');
    }
});
document.getElementById('photoIdForm').addEventListener('submit', function(event) {
    event.preventDefault(); // Prevent form submission
    
    // Get the value of the photo ID input field
    const photoId = document.getElementById('photoIdInput').value.trim();
    
    // Fetch photo data based on the entered photo ID
    fetch(`http://127.0.0.1:5000/getData/${photoId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Response data:', data); // Log the response data to inspect it
            if (data[0].photo_url) {
                // Update the src attribute of the image with the fetched photo URL
                document.getElementById('uploadedPhoto').src = data[0].photo_url;
                document.getElementById('testeText').textContent = data[0].photo_url;
            } else {
                console.error('Photo not found:', data.error);
            }
        })
        .catch(error => {
            console.error('Error fetching photo:', error);
        });
});
document.getElementById('photoIdFormBlur').addEventListener('submit', function(event) {
    event.preventDefault(); // Prevent form submission
    
    // Get the value of the photo ID input field
    const photoId = document.getElementById('photoIdBlur').value.trim();
    
    // Fetch photo data based on the entered photo ID
    fetch(`http://127.0.0.1:5000/processImageFromDB`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Response data:', data); // Log the response data to inspect it
            if (data[0].photo_url) {
                // Update the src attribute of the image with the fetched photo URL
                document.getElementById('uploadedPhoto').src = data[0].photo_url;
                document.getElementById('testeText').textContent = data[0].photo_url;
            } else {
                console.error('Photo not found:', data.error);
            }
        })
        .catch(error => {
            console.error('Error fetching photo:', error);
        });
});