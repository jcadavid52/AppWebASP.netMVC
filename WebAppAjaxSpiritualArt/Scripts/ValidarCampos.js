let email = document.getElementById("email").value;

if (email.lenght == 0) {
    document.getElementById("validacion2").innerHTML = '<h6 class="validacion" id="validacion">*</h6>';
}

if (email.lenght != 0) {
    document.getElementById("validacion").innerHTML = '<h6 class="validacion" id="validacion"></h6>';
}