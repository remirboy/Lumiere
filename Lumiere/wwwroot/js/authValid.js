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
  var exception;

  if (!validateEmail(email)) {
      exception="Формат email: example@gmail.com";
      $('#email_helper').text(exception);
  }
  else{
      $('#email_helper').text('')
  }

  if (!validatePass(pass)) {
      exception="Пароль должен содержать одну цифру и одну большую букву";
      $('#password_helper').text(exception);
  }
  else{
      $('#password_helper').text('');
  }

  if (!validateRePass(repass,pass)) {
      exception="Пароли не совпадают";
      $('#repassword_helper').text(exception);
  }
  else{
      $('#repassword_helper').text('');
  }
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
