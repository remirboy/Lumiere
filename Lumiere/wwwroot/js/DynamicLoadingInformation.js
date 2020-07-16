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

