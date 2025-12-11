document.addEventListener("DOMContentLoaded", () => {
    cargarProductos();
    actualizarBadge();
});

// 1. Cargar productos desde la Base de Datos
async function cargarProductos() {
    const contenedor = document.querySelector(".service-list");
    if (!contenedor) return;

    try {
        // Pedimos los datos a la API (Puerto 5224)
        const response = await apiFetch("/api/articulos");
        
        if (!response.ok) {
            contenedor.innerHTML = "<p>Error al cargar productos.</p>";
            return;
        }

        const productos = await response.json();

        // Limpiamos el contenido hardcoded del HTML
        contenedor.innerHTML = "";

        if (productos.length === 0) {
            contenedor.innerHTML = "<p>No hay productos disponibles.</p>";
            return;
        }

        // Generar HTML por cada producto
        productos.forEach(prod => {
            const card = document.createElement("div");
            card.classList.add("service-card");
            
            // Usamos una imagen por defecto si no hay URL o usamos un placeholder
            const imagen = "IMG/producto_default.png"; 

            card.innerHTML = `
                <img src="${imagen}" alt="${prod.nombre}" style="max-height: 150px; object-fit: contain;">
                <h3>${prod.nombre}</h3>
                <p>${prod.descripcion || "Sin descripción"}</p>
                <span class="service-price">$${prod.precio.toFixed(2)}</span>

                <div class="service-controls">
                    <button class="qty-btn minus" onclick="agregarAlCarrito(${prod.id}, '${prod.nombre}', ${prod.precio}, -1)">−</button>
                    <span class="qty-display" id="qty-${prod.id}">0</span>
                    <button class="qty-btn plus" onclick="agregarAlCarrito(${prod.id}, '${prod.nombre}', ${prod.precio}, 1)">+</button>
                </div>
            `;
            contenedor.appendChild(card);
        });

        // Restaurar cantidades si ya había cosas en el carrito
        actualizarDisplays();

    } catch (error) {
        console.error("Error cargando productos:", error);
        contenedor.innerHTML = "<p>Error de conexión con el servidor.</p>";
    }
}

// 2. Lógica del Carrito (Unificada)
function agregarAlCarrito(id, nombre, precio, cantidad) {
    let carrito = JSON.parse(localStorage.getItem("carrito")) || {};
    
    // Clave única para el producto
    const key = `prod_${id}`;

    if (!carrito[key]) {
        carrito[key] = { id, nombre, precio, cantidad: 0 };
    }

    carrito[key].cantidad += cantidad;

    if (carrito[key].cantidad <= 0) {
        delete carrito[key];
    }

    localStorage.setItem("carrito", JSON.stringify(carrito));
    
    // Actualizar vista
    const display = document.getElementById(`qty-${id}`);
    if (display) display.textContent = carrito[key] ? carrito[key].cantidad : 0;
    
    actualizarBadge();
}

function actualizarDisplays() {
    let carrito = JSON.parse(localStorage.getItem("carrito")) || {};
    for (const key in carrito) {
        const item = carrito[key];
        const display = document.getElementById(`qty-${item.id}`);
        if (display) display.textContent = item.cantidad;
    }
}

function actualizarBadge() {
    let carrito = JSON.parse(localStorage.getItem("carrito")) || {};
    const total = Object.values(carrito).reduce((acc, item) => acc + item.cantidad, 0);
    const badge = document.getElementById("cart-count");
    if (badge) badge.textContent = total;
}

// Hacer funciones globales para que el HTML onclick las vea
window.agregarAlCarrito = agregarAlCarrito;