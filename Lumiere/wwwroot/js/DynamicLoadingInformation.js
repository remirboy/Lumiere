// Динамическое заполнение дат в зависимости от фильма.
function loadDates(select_film) {
  var filmId = select_film.value;

  $.ajax({
    type: 'GET',
    url: '/Booking/DatesList?filmId=' + filmId,
    success: function (data) {
      $('#date').replaceWith(data);
    }
  });
}

// Динамическое заполнение времени сеансов в зависимости от даты.
function loadTimes(select_date) {
  var filmId = $('#film').val();
  var date = select_date.value

  $.ajax({
    type: 'GET',
    url: '/Booking/TimesList?filmId=' + filmId + '&date=' + date,
    success: function (data) {
      $('#time').replaceWith(data);
    }
  });
}

// Динамическое заполнение номера зала в зависимости от времени сеанса. 
function loadRoom(select_time) {
  var filmId = $('#film').val();
  var date = $('#date').val();
  var time = select_time.value;

  $.ajax({
    type: 'GET',
    url: '/Booking/RoomNumbersList?filmId=' + filmId + '&date=' + date + '&time=' + time,
    success: function (data) {
      $('#roomNumber').replaceWith(data);
    }
  });
}