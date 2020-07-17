var count = 0;
var price = 0;
var total_price = 0;
var booked = [];
var blocked = [];
var list = document.getElementsByClassName('check');

function takePlace(input){
	if (input.checked==1&&count<7){
		count+=1;
		total_price+=price;
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
		total_price-=price;
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
	document.getElementById('number').innerHTML = count;
}

function setMessage(){
	document.getElementById('place_help').innerHTML = exception;
}

function setPrice(){
	document.getElementById('total_price').innerHTML = total_price;
}

function block(){
	for (var i = 0; i < blocked.length; i++) {
		list[blocked[i]].style="background-color : #818292";
		if (list[blocked[i]].checked == 1) {
			list[blocked[i]].checked = 0;
			count -= 1;
			total_price -= price;
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
	var filmSeance = {
		RoomNumber: $('#roomNumber').val(),
		Date: $('#date').val(),
		Price: $('#price').text(),
		Time: $('#time').val(),
		FilmId: $('#film').val() 
	}

	$.ajax({
		method: "post",
		url: "/Booking/GetReservedSeats",
		dataType: "json",
		data: filmSeance,
		success: function (data) {
			console.log(data);

			if (msg.objects.length > 0) {
				blocked=[];
				for (var i = 0; i < msg.objects.length; i++) {
					blocked.push(msg.objects[i].number);
					list[msg.objects[i].number].disabled=1;
					block()
				}
			}
		}
	});
}

//setInterval( f, 1000, list);