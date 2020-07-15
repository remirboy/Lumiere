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
    exception="Формат email: example@gmail.com";
    document.getElementById('email_help').innerHTML = exception;
  }
  else{
    document.getElementById('email_help').innerHTML = '';
  }
   if (validatePassword(pass)) {
        exception="Поле должно быть заполненным";
        document.getElementById('password_help').innerHTML = exception;
    }
    else{
      document.getElementById('password_help').innerHTML = '';
    }
}

function validRegistration(){
  var email = document.getElementById('emailEnter').value;
  var pass = document.getElementById('passwordEnter').value;
  var repass = document.getElementById('passwordConfirm').value;
  var name = document.getElementById('firstName').value;
  var surname = document.getElementById('lastName').value;
  var birth = document.getElementById('birthDate').value;
  var exception;
  if (!validateEmail(email)) {
    exception="Формат email: example@gmail.com";
    document.getElementById('email_helper').innerHTML = exception;
  }
  else{
    document.getElementById('email_helper').innerHTML = '';
  }
  if (!validateName(name)) {
    exception="Имя начинается с большой русской буквы";
    document.getElementById('name_help').innerHTML = exception;
  }
  else{
    document.getElementById('name_help').innerHTML = '';
  }
  if (!validateName(surname)) {
    exception="Фамилия начинается с большой русской буквы";
    document.getElementById('surname_help').innerHTML = exception;
  }
  else{
    document.getElementById('surname_help').innerHTML = '';
  }
  if (!validatePass(pass)) {
    exception="Пароль должен содержать как минимум одну цифру и одну большую букву";
    document.getElementById('password_helper').innerHTML = exception;
  }
  else{
    document.getElementById('password_helper').innerHTML = '';
  }
  if (!validateRePass(repass,pass)) {
    exception="Пароли не совпадают";
    document.getElementById('repassword_helper').innerHTML = exception;
  }
  else{
    document.getElementById('repassword_helper').innerHTML = '';
  }
  if (!validateBirth(birth)) {
    exception="Формат ввода: дд.мм.гггг";
    document.getElementById('birth_help').innerHTML = exception;
  }
  else{
    document.getElementById('birth_help').innerHTML = '';
  }
}


function validateName(name){
  var re = /^[а-яА-Я'][а-яА-Я-' ]+[а-яА-Я']?$/;
  return re.test(name);
}

function validatePassword(pass){
  if (pass=='') 
    return true;
}

function validatePass(pass){
  return validateNumber(pass)&&validateLiteral(pass);
}

function validateRePass(repass,pass){
  if (pass==repass) 
    return true;
}

function validateBirth(birth) {
  var reg = /^\d{2}([.])\d{2}\1\d{4}$/;
  return reg.test(birth);
}


function validateNumber(pass){
  var reg = /[0-9]/;
  return reg.test(pass);
}

function validateLiteral(pass){
  var reg = /[A-Z]/;
  return reg.test(pass);
}


function validateEmail(email) {
  var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  return re.test(email);
}
