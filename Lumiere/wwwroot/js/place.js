var selectedSeatsCount = 0;
var price = 0;
var total_price = 0;
var booked = [];
var blocked = [];
var seats = document.getElementsByClassName('check');

function takePlace(input){
	if (input.checked == 1 && selectedSeatsCount < 7){
		selectedSeatsCount += 1;
		total_price += price;
		setPlace()
		setPrice()
	}
	if (selectedSeatsCount == 7){
		exception="Максимальное кол-во билетов: 7";
		setMessage()
		placeBlocking();
	}
	if (selectedSeatsCount <= 7 && input.checked == 0) {
		selectedSeatsCount -= 1;
		total_price -= price;
		exception="";
		setMessage()
		setPlace();
		setPrice();
		placeOpening();
	}
}

function placeBlocking(){
	for (var index = 0; index < seats.length; ++index) 
		if (seats[index].checked==0)
			seats[index].disabled=1;
}

function placeOpening(){
	for (var index = 0; index < seats.length; ++index)
		if(!blocked.includes(index))
			seats[index].disabled=0;
}

function book(){
	for (var index = 0; index < seats.length; ++index) {
		if (seats[index].checked==1) {
			booked.push(index);
		}
	}
	sendJSON();
	booked = [];
}

function sendJSON() {
	var reservedSeats = {
		SeatNumbers: booked,
		RoomNumber: $('#roomNumber').val(),
		Date: $('#date').val(),
		Price: $('#price').text(),
		Time: $('#time').val(),
		FilmId: $('#film').val()
	};

	$.ajax({
		url: "/Booking/ReservedSeats/",
		type: "POST",
		data: reservedSeats,
		dataType: "json", 
		success: function (result) {

		}
	});
}


function setPlace(){
	document.getElementById('number').innerHTML = selectedSeatsCount;
}

function setMessage(){
	document.getElementById('place_help').innerHTML = exception;
}

function setPrice(){
	document.getElementById('total_price').innerHTML = total_price;
}

function block(){
	for (var i = 0; i < blocked.length; i++) {
		seats[blocked[i]].style="background-color : #818292";
		if (seats[blocked[i]].checked == 1) {
			seats[blocked[i]].checked = 0;
			selectedSeatsCount -= 1;
			total_price -= price;
			exception="";
			booked.splice(booked.indexOf(blocked[i]), 1);
			setPlace();
			setPrice();
			setMessage()
		}
	}
	for (var index = 0; index < seats.length; ++index) {
		if (!blocked.includes(index)) {
			seats[index].style = "";
			seats[index].disabled = 0;
		}
	}
	if (selectedSeatsCount == 7){
		placeBlocking();
	}
}
