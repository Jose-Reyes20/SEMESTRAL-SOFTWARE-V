 let slideIndex = 1; // Empezamos desde el slide 1
showSlides(slideIndex);

function plusSlides(n) {
  showSlides(slideIndex += n);
}

function currentSlide(n) {
  showSlides(slideIndex = n);
}

function showSlides() {
    let i;
    let slides = document.getElementsByClassName("slides");
    
    // Si slideIndex es mayor que el número de slides, vuelve al primero
    if (slideIndex > slides.length) {slideIndex = 1}    
    // Si slideIndex es menor que 1, ve al último
    if (slideIndex < 1) {slideIndex = slides.length}

    // Oculta todos los slides
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";  
    }
    
    // Muestra el slide actual
    slides[slideIndex-1].style.display = "block";  
    
    // Incrementa el índice para el siguiente ciclo
    slideIndex++;
    
    // Llama a la función de nuevo después de 4 segundos
    setTimeout(showSlides, 4000); 
}