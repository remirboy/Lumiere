// Динамическое заполнение дат в зависимости от фильма.
$('#film').change(function () {
  var filmId = $(this).val();

  $.ajax({
    type: 'GET',
    url: '/Booking/DatesList?filmId=' + filmId,
    success: function (data) {
      $('#date').replaceWith(data);
    }
  });
});

