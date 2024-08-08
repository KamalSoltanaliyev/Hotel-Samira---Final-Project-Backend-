let commentId = 2;

document
  .getElementById("submit-comment")
  .addEventListener("click", function () {
    const rating = document.querySelector('input[name="rating"]:checked');
    const commentText = document.getElementById("comment-text").value;
    const commentList = document.getElementById("comment-list");

    if (rating && commentText) {
      const newComment = document.createElement("div");
      newComment.classList.add("testimonial-box");
      newComment.setAttribute("data-id", commentId++);

      newComment.innerHTML = `
      <div class="box-top">
        <div class="profile">
          <div class="name-user">
            <strong>Guest User</strong>
            <span>guest@example.com</span>
          </div>
        </div>
        <div class="reviews">
          ${'<i class="fas fa-star"></i>'.repeat(
            rating.value
          )}${'<i class="far fa-star"></i>'.repeat(5 - rating.value)}
        </div>
      </div>
      <div class="client-comment">
        <p>${commentText}</p>
        <div class="comment-actions">
          <button class="edit-comment btn">Edit</button>
          <button class="delete-comment btn">Delete</button>
        </div>
      </div>
    `;
      commentList.appendChild(newComment);

      newComment
        .querySelector(".edit-comment")
        .addEventListener("click", editComment);
      newComment
        .querySelector(".delete-comment")
        .addEventListener("click", deleteComment);

      document.querySelector('input[name="rating"]:checked').checked = false;
      document.getElementById("comment-text").value = "";
    } else {
      alert("Please select a rating and write a comment.");
    }
  });

function editComment(event) {
  const commentBox = event.target.closest(".testimonial-box");
  const commentText = commentBox.querySelector(".client-comment p").innerText;
  const rating = commentBox
    .querySelector(".reviews")
    .querySelectorAll(".fa-star").length;

  document.getElementById("comment-text").value = commentText;
  document.querySelector(
    `input[name="rating"][value="${rating}"]`
  ).checked = true;

  commentBox.remove();

  document.querySelector(".comments").scrollIntoView({ behavior: "smooth" });
}

function deleteComment(event) {
  const commentBox = event.target.closest(".testimonial-box");
  commentBox.remove();
}

document.querySelectorAll(".edit-comment").forEach((button) => {
  button.addEventListener("click", editComment);
});

document.querySelectorAll(".delete-comment").forEach((button) => {
  button.addEventListener("click", deleteComment);
});
