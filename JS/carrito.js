let carrito = JSON.parse(localStorage.getItem("carrito")) || {};

function guardarCarrito() {
    localStorage.setItem("carrito", JSON.stringify(carrito));
}

function actualizarContador() {
    const totalItems = Object.values(carrito).reduce((acc, item) => acc + item.cantidad, 0);
    const contador = document.getElementById("cart-count");
    if (contador) contador.textContent = totalItems;
}

function calcularTotal() {
    let total = 0;
    for (const item of Object.values(carrito)) {
        total += item.precio * item.cantidad;
    }
    const totalElemento = document.getElementById("cart-total");
    if (totalElemento) totalElemento.textContent = "$" + total.toFixed(2);
    return total;
}

function cargarCarritoEnPagina() {
    const contenedor = document.getElementById("cart-items");
    if (!contenedor) return;

    contenedor.innerHTML = "";

    if (Object.keys(carrito).length === 0) {
        contenedor.innerHTML = "<p style='text-align:center; padding:20px;'>Tu carrito está vacío.</p>";
        return;
    }

    for (const nombre in carrito) {
        const item = carrito[nombre];
        const div = document.createElement("div");
        div.classList.add("cart-item");
        div.innerHTML = `
            <div class="item-info">
                <h3>${item.nombre || nombre}</h3>
                <p>$${item.precio.toFixed(2)}</p>
            </div>
            <div class="item-controls">
                <button class="btn-restar" data-id="${item.id}">-</button>
                <span>${item.cantidad}</span>
                <button class="btn-sumar" data-id="${item.id}">+</button>
            </div>
            <button class="btn-eliminar" data-id="${item.id}">Eliminar</button>
        `;
        contenedor.appendChild(div);
    }
    agregarEventos();
    actualizarContador();
    calcularTotal();
}

function agregarEventos() {
    // Usamos ID en lugar de nombre para mayor precisión
    document.querySelectorAll(".btn-sumar").forEach(btn => {
        btn.addEventListener("click", () => modificarCantidad(btn.dataset.id, 1));
    });

    document.querySelectorAll(".btn-restar").forEach(btn => {
        btn.addEventListener("click", () => modificarCantidad(btn.dataset.id, -1));
    });

    document.querySelectorAll(".btn-eliminar").forEach(btn => {
        btn.addEventListener("click", () => {
            const id = btn.dataset.id;
            // Buscar por ID en el objeto carrito
            const key = Object.keys(carrito).find(k => carrito[k].id == id);
            if (key) {
                delete carrito[key];
                guardarCarrito();
                cargarCarritoEnPagina();
            }
        });
    });

    const clearBtn = document.getElementById("clear-cart");
    if (clearBtn) {
        clearBtn.addEventListener("click", () => {
            carrito = {};
            guardarCarrito();
            cargarCarritoEnPagina();
            actualizarContador();
        });
    }
}

function modificarCantidad(id, delta) {
    const key = Object.keys(carrito).find(k => carrito[k].id == id);
    if (!key) return;

    carrito[key].cantidad += delta;
    if (carrito[key].cantidad <= 0) delete carrito[key];
    
    guardarCarrito();
    cargarCarritoEnPagina();
}

// ==========================================
//   CONFIRMAR COMPRA (Conexión a API Ordenes)
// ==========================================
document.getElementById("confirmar-compra")?.addEventListener("click", async () => {
    // 1. Validar usuario conectado
    const usuarioStr = localStorage.getItem("usuarioConectado");
    if (!usuarioStr) {
        alert("Debes iniciar sesión para comprar.");
        window.location.href = "login.html";
        return;
    }
    const usuario = JSON.parse(usuarioStr);

    // 2. Validar carrito
    if (Object.keys(carrito).length === 0) {
        alert("Tu carrito está vacío.");
        return;
    }

    const montoTotal = calcularTotal();
    const impuesto = montoTotal * 0.07; // Ejemplo 7%
    const totalFinal = montoTotal + impuesto;

    // 3. Construir objeto ORDEN según tu Backend C#
    const nuevaOrden = {
        UsuarioId: usuario.id, // ID del usuario logueado
        Estado: "pendiente",
        Fecha: new Date().toISOString(),
        Subtotal: montoTotal,
        Itbms: impuesto,
        Total: totalFinal,
        Descuento: 0,
        // Opcional: Si tu API soporta recibir detalles aquí, los agregas. 
        // Si no, la orden se creará vacía de productos.
    };

    try {
        // Usar apiFetch con la configuración centralizada
        const response = await apiFetch("/api/ordenes", {
            method: "POST",
            body: JSON.stringify(nuevaOrden)
        });

        if (!response.ok) {
            alert("Error al procesar la orden.");
            return;
        }

        alert("¡Compra realizada con éxito!");
        carrito = {};
        guardarCarrito();
        cargarCarritoEnPagina();
        actualizarContador();

    } catch (error) {
        console.error(error);
        alert("Error de conexión con el servidor.");
    }
});

// Inicializar al cargar
cargarCarritoEnPagina();
actualizarContador();