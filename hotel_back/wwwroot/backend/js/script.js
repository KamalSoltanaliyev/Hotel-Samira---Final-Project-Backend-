const loadMoreBtn = document.getElementById("loadMoreBtn");
const productBox = document.getElementById("productBox");
const addToBasketBtns = document.querySelectorAll(".add-to-basket");
const basketItems = document.getElementById("basket-items");

let skip = 6;
loadMoreBtn.addEventListener("click", function () {
    let url = `/Rooms/LoadMore?skip=${skip}`;

    fetch(url).then(response => response.text())
        .then(data => {
            const tempDiv = document.createElement('div');
            tempDiv.innerHTML = data;

            while (tempDiv.firstChild) {
                productBox.appendChild(tempDiv.firstChild);
            }

            skip += 6;

            if (!data.trim()) {
                loadMoreBtn.style.display = 'none';
            }
        });
});

addToBasketBtns.forEach(addToBasketBtn => {
    addToBasketBtn.addEventListener("click", function (e) {
        e.preventDefault();

        let url = this.getAttribute("href");
        fetch(url).then(response => response.text())
            .then(data => {
                basketItems.innerHTML = data;
            })
    }
})
