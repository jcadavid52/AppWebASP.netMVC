const $seleccionArchivos = document.querySelector("#archivoProducto2"),
    $imagenPrevisualizacion = document.querySelector("#imagenPrueba");

// Escuchar cuando cambie
$seleccionArchivos.addEventListener("change", () => {
    // Los archivos seleccionados, pueden ser muchos o uno
    const archivos = $seleccionArchivos.files;
    // Si no hay archivos salimos de la función y quitamos la imagen
    if (!archivos || !archivos.length) {
        $imagenPrevisualizacion.src = "";
        return;
    }
    // Ahora tomamos el primer archivo, el cual vamos a previsualizar
    const primerArchivo = archivos[0];
    // Lo convertimos a un objeto de tipo objectURL
    const objectURL = URL.createObjectURL(primerArchivo);
    // Y a la fuente de la imagen le ponemos el objectURL
    $imagenPrevisualizacion.src = objectURL;

    document.getElementById("imagenPrueba").style.height = "250px";
    document.getElementById("imagenPrueba").style.width = "250px";

});


const $seleccionArchivos2 = document.querySelector("#archivoProducto"),
    $imagenPrevisualizacion2 = document.querySelector("#imagenPrueba2");

// Escuchar cuando cambie
$seleccionArchivos2.addEventListener("change", () => {
    // Los archivos seleccionados, pueden ser muchos o uno
    const archivos = $seleccionArchivos2.files;
    // Si no hay archivos salimos de la función y quitamos la imagen
    if (!archivos || !archivos.length) {
        $imagenPrevisualizacion2.src = "";
        return;
    }
    // Ahora tomamos el primer archivo, el cual vamos a previsualizar
    const primerArchivo = archivos[0];
    // Lo convertimos a un objeto de tipo objectURL
    const objectURL = URL.createObjectURL(primerArchivo);
    // Y a la fuente de la imagen le ponemos el objectURL
    $imagenPrevisualizacion2.src = objectURL;

    document.getElementById("imagenPrueba2").style.height = "250px";
    document.getElementById("imagenPrueba2").style.width = "250px";

});