$(function(){
  let authButton = document.getElementById("auth--button")
  authButton.onclick = function(){
    $('#authModal').modal('show')
  }
});

function valid(form){
  var email = form.email.value;
  var pass = form.password.value;
  var exception;
  if (!validateEmail(email)) {
    exception="Заполните это поле";
    document.getElementById('email_help').innerHTML = exception;
  }
  else{
    document.getElementById('email_help').innerHTML = '';
  }
  if (!validatePassword(pass)) {
    exception="Пароль должен содержать буквы латинского алфавита, как минимум одну цифру и одну заглавную букву";
    document.getElementById('password_help').innerHTML = exception;
  }
  else{
    document.getElementById('password_help').innerHTML = '';
  }

  login();
}

function validateEmail(email) {
  var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  return re.test(email);
}

function validatePassword(pass){
  return validateNumber(pass)&&validateLiteral(pass);
}

function validateNumber(pass){
  var regNumber = /[0-9]/;
  return regNumber.test(pass);
}

function validateLiteral(pass){
  var reg = /[A-Z]/;
  return reg.test(pass);
}