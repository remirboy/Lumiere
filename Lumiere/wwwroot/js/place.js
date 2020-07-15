var count = 0;
var price = 0;
var booked = [];
var blocked = [];
var list = document.getElementsByClassName('check');

function takePlace(input){
	if (input.checked==1&&count<7){
		count+=1;
		price+=500;
		setPlace()
		setPrice()
		f(list)
	}
	if (count==7){
		exception="Макс. кол-во билетов:7";
		setMessage()
		placeBlocking(list);
	}
	if (count<=7&&input.checked==0) {
		f(list)
		count-=1;
		price-=500;
		exception="";
		setMessage()
		setPlace();
		setPrice();
		placeOpening(list);
	}
}

function placeBlocking(list){
	for (var index = 0; index < list.length; ++index) 
    	if(list[index].checked==0)
    		list[index].disabled=1;
}

function placeOpening(list){
	for (var index = 0; index < list.length; ++index)
		if(!blocked.includes(index))
			list[index].disabled=0;
}

function book(){
	for (var index = 0; index < list.length; ++index) {
    	if(list[index].checked==1) {
			booked.push(index);
		}
	}
	sendJSON();
	booked = [];
}

function sendJSON() {
	var reservedSeats = {
		SeatNumbers: booked,
		RoomNumber: $('#room_number').text(),
		Date: $('#date').val(),
		Time: $('#time').val(),
		FilmId: $('#film').val()
	};

	/*
	var xhr = new XMLHttpRequest();
	var url = "/Booking/ReservedSeats";
	xhr.open("POST", url, true);
	xhr.setRequestHeader("Content-Type", "application/json");

	var data = JSON.stringify(reservedSeat);
	console.log(data);
	xhr.send(data);
	*/

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
	document.getElementById('number').innerHTML = count;
}

function setMessage(){
	document.getElementById('place_help').innerHTML = exception;
}

function setPrice(){
	document.getElementById('price').innerHTML = price;
}

function block(){
	for (var i = 0; i < blocked.length; i++) {
		list[blocked[i]].style="background-color : #818292";
		if (list[blocked[i]].checked == 1) {
			list[blocked[i]].checked = 0;
			count -=1;
			price -=500;
			exception="";
			booked.splice(booked.indexOf(blocked[i]), 1);
			setPlace();
			setPrice();
			setMessage()
		}
	}
	for (var index = 0; index < list.length; ++index) {
		if (!blocked.includes(index)) {
			list[index].style = "";
			list[index].disabled = 0;
		}
	}
	if (count==7){
		placeBlocking(list);
	}
}

function f(list) {
	$.ajax({
		url: "/Booking/GetReservedSeats",
  		dataType: "json",
		success: function (msg) {
			if (msg.objects.length > 0) {
				blocked=[];
				for (var i = 0; i < msg.objects.length; i++) {
					blocked.push(msg.objects[i].number);
					list[msg.objects[i].number].disabled=1;
					block()
				}
			} else {
			}
		}
	});
}

//setInterval( f, 1000, list);