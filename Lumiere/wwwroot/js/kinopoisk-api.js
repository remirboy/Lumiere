document.addEventListener("DOMContentLoaded", () => {
    getRaiting(326);
});


function getRaiting(id) {
    var request = new XMLHttpRequest();

    request.open("GET", "https://kinopoiskapiunofficial.tech/api/v1/reviews?filmId=" + id + "&page=1", false);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.setRequestHeader("X-API-KEY", "e961fbe1-05d8-4830-81b6-8f6eed118564");


    request.send();

    if (request.status == 200) {
        data = JSON.parse(request.responseText);
        document.getElementById("raiting").innerHTML = data.reviewAllPositiveRatio;
    }
}