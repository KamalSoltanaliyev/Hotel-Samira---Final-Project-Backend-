if (document.getElementById('addImage')) {
    document.getElementById('addImage').addEventListener('click', function () {
        var imageInputs = document.getElementById('imageInputs');
        if (imageInputs.children.length < 7) {
            var input = imageInputs.lastElementChild;
            if (!input || input.files.length > 0) {
                var clonedInput = input.cloneNode(true);
                clonedInput.value = '';
                imageInputs.appendChild(clonedInput);
            } else {
                alert('Select a file before adding another image');
            }
        } else {
            alert('You can only upload up to 4 images');
        }
    });
}

if (document.getElementById('addImageUpdate')) {
    document.getElementById('addImageUpdate').addEventListener('click', function () {
        var imageCount = document.querySelectorAll(".p-image").length;
        var productImageInputCount = document.querySelectorAll(".product-image-input").length;
        var imageInputs = document.getElementById("imageInputs");

        if (imageCount + productImageInputCount < 5) {
            var input = imageInputs.lastElementChild;
            if (!input || input.files.length > 0) {
                var clonedInput = input.cloneNode(true);
                clonedInput.value = '';
                imageInputs.appendChild(clonedInput);
            } else {
                alert('Select a file before adding another image');
            }
        } else {
            alert('You can only upload up to 3 images');
        }
    });
}