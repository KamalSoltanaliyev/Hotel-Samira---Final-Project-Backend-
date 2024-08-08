const imgs = document.querySelectorAll(".img-select a");
const imgBtns = [...imgs];
let imgId = 1;

imgBtns.forEach((imgItem) => {
    imgItem.addEventListener("click", (event) => {
        event.preventDefault();
        imgId = imgItem.dataset.id;
        slideImage();
    });
});

function slideImage() {
    const displayWidth = document.querySelector(
        ".img-showcase img:first-child"
    ).clientWidth;

    document.querySelector(".img-showcase").style.transform = `translateX(${-(imgId - 1) * displayWidth
        }px)`;
}

window.addEventListener("resize", slideImage);

document.addEventListener("DOMContentLoaded", function () {
    const submitButton = document.getElementById("submit-comment");
    const commentList = document.getElementById("comment-list");
    const ratingInputs = document.querySelectorAll('input[name="rating"]');
    const commentTextarea = document.querySelector(".comment-form textarea");
    const averageRatingDisplay = document.createElement("div");


    commentList.parentElement.insertBefore(averageRatingDisplay, commentList);

    submitButton.addEventListener("click", function () {
        let selectedRating;
        for (const input of ratingInputs) {
            if (input.checked) {
                selectedRating = input.value;
                break;
            }
        }

        const commentText = commentTextarea.value.trim();

        if (!selectedRating || !commentText) {
            alert("Please select a rating and write a comment.");
            return;
        }

        const commentItem = document.createElement("div");
        commentItem.className = "comment-item";

        const commentAuthor = document.createElement("h3");
        commentAuthor.className = "comment-author";
        commentAuthor.textContent = "Anonymous"; // Здесь можно добавить функциональность для ввода имени

        const commentRating = document.createElement("div");
        commentRating.className = "comment-rating";
        for (let i = 1; i <= 5; i++) {
            const star = document.createElement("i");
            star.className =
                i <= selectedRating ? "fas fa-star" : "far fa-star";
            commentRating.appendChild(star);
        }

        const commentTextElement = document.createElement("p");
        commentTextElement.className = "comment-text";
        commentTextElement.textContent = commentText;

        const commentActions = document.createElement("div");
        commentActions.className = "comment-actions";
        const editButton = document.createElement("button");
        editButton.className = "edit-comment btn";
        editButton.textContent = "Edit";
        const deleteButton = document.createElement("button");
        deleteButton.className = "delete-comment btn";
        deleteButton.textContent = "Delete";

        commentActions.appendChild(editButton);
        commentActions.appendChild(deleteButton);

        commentItem.appendChild(commentAuthor);
        commentItem.appendChild(commentRating);
        commentItem.appendChild(commentTextElement);
        commentItem.appendChild(commentActions);

        commentList.appendChild(commentItem);

        editButton.addEventListener("click", editComment);
        deleteButton.addEventListener("click", deleteComment);

        // Обновление среднего рейтинга
        updateAverageRating();

        // Очистка формы
        for (const input of ratingInputs) {
            input.checked = false;
        }
        commentTextarea.value = "";
    });






    function editComment(event) {
        const commentBox = event.target.closest(".comment-item");
        const commentText = commentBox.querySelector(".comment-text").innerText;
        const rating = commentBox
            .querySelector(".comment-rating")
            .querySelectorAll(".fa-star").length;

        commentTextarea.value = commentText;
        document.querySelector(
            `input[name="rating"][value="${rating}"]`
        ).checked = true;

        commentBox.remove();

        updateAverageRating();
    }

    function deleteComment(event) {
        const commentBox = event.target.closest(".comment-item");
        commentBox.remove();
        updateAverageRating();
    }

    document
        .querySelectorAll(".edit-comment")
        .forEach((button) => button.addEventListener("click", editComment));

    document
        .querySelectorAll(".delete-comment")
        .forEach((button) => button.addEventListener("click", deleteComment));
});

document.getElementById("reserveButton").addEventListener("click", function () {
    if (this.textContent === "Reserve") {
        this.textContent = "Reserved";
    } else {
        this.textContent = "Reserve";
    }
});
