$(function () {
    loadDates($('#film'));
    setInterval(SetReservedSeats, 1000);
});

function SetReservedSeats() {
    var filmSeance = {
        RoomNumber: $('#roomNumber').val(),
        Date: $('#date').val(),
        Price: $('#price').text(),
        Time: $('#time').val(),
        FilmId: $('#film').val()
    }

    $.ajax({
        method: "POST",
        url: "/Booking/GetReservedSeats/",
        data: filmSeance,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                blocked = [];
                for (var i = 0; i < data.length; i++) {
                    blocked.push(data[i] - 1);
                    seats[data[i] - 1].disabled = 1;
                    block()
                }
            }
        }
    });
}

// Динамическое заполнение дат в зависимости от фильма.
function loadDates(select_film) {
    var filmId = select_film.val();

    $.ajax({
        type: 'GET',
        url: '/Booking/DatesList?filmId=' + filmId,
        success: function (data) {
            $('#date').replaceWith(data);
            loadTimes($('#date'));
        }
    });
}

// Динамическое заполнение времени сеансов в зависимости от даты.
function loadTimes(select_date) {
    var filmId = $('#film').val();
    var date = select_date.val();

    $.ajax({
        type: 'GET',
        url: '/Booking/TimesList?filmId=' + filmId + '&date=' + date,
        success: function (data) {
            $('#time').replaceWith(data);
            loadRoom($('#time'));
        }
    });
}

// Динамическое заполнение номера зала в зависимости от времени сеанса. 
function loadRoom(select_time) {
    var filmId = $('#film').val();
    var date = $('#date').val();
    var time = select_time.val();

    $.ajax({
        type: 'GET',
        url: '/Booking/RoomNumbersList?filmId=' + filmId + '&date=' + date + '&time=' + time,
        success: function (data) {
            $('#roomNumber').replaceWith(data);
            loadPrice($('#roomNumber'));
        }
    });
}

// Динамическое заполнение цены билеты в зависимости от номера зала. 
function loadPrice(select_room) {
    var filmId = $('#film').val();
    var date = $('#date').val();
    var time = $('#time').val();
    var room = select_room.val();

    var seance = {
        FilmId: filmId,
        Date: date,
        Time: time,
        RoomNumber: room
    }

    $.ajax({
        type: 'POST',
        url: '/Booking/LoadPrice',
        data: seance,
        dataType: "json",
        success: function (data) {
            $('#price').text(data);
            price = data;
            SetReservedSeats();
        }
    });
}
