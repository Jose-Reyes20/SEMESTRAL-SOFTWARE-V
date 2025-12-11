
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
}

function cargarCarritoEnPagina() {
    const contenedor = document.getElementById("cart-items");
    if (!contenedor) return;

    contenedor.innerHTML = "";

    for (const nombre in carrito) {
        const item = carrito[nombre];

        const div = document.createElement("div");
        div.classList.add("cart-item");

        div.innerHTML = `
            <div class="item-info">
                <h3>${nombre}</h3>
                <p>$${item.precio.toFixed(2)}</p>
            </div>

            <div class="item-controls">
                <button class="btn-restar" data-nombre="${nombre}">-</button>
                <span>${item.cantidad}</span>
                <button class="btn-sumar" data-nombre="${nombre}">+</button>
            </div>

            <button class="btn-eliminar" data-nombre="${nombre}">Eliminar</button>
        `;

        contenedor.appendChild(div);
    }

    agregarEventos();
    actualizarContador();
    calcularTotal();
}


//      EVENTOS DEL CARRITO EN LA PÁGINA

function agregarEventos() {

    // SUMAR en carrito
    document.querySelectorAll(".btn-sumar").forEach(btn => {
        btn.addEventListener("click", () => {
            const nombre = btn.dataset.nombre;
            if (!carrito[nombre]) return;
            carrito[nombre].cantidad++;
            guardarCarrito();
            cargarCarritoEnPagina();
        });
    });

    // RESTAR en carrito
    document.querySelectorAll(".btn-restar").forEach(btn => {
        btn.addEventListener("click", () => {
            const nombre = btn.dataset.nombre;
            if (!carrito[nombre]) return;

            if (carrito[nombre].cantidad > 1) {
                carrito[nombre].cantidad--;
            } else {
                delete carrito[nombre];
            }

            guardarCarrito();
            cargarCarritoEnPagina();
        });
    });

    // ELIMINAR
    document.querySelectorAll(".btn-eliminar").forEach(btn => {
        btn.addEventListener("click", () => {
            const nombre = btn.dataset.nombre;
            delete carrito[nombre];
            guardarCarrito();
            cargarCarritoEnPagina();
        });
    });

    // VACIAR CARRITO
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


//   BOTONES + Y - DE LA PÁGINA DE PRODUCTOS

document.querySelectorAll(".qty-btn.plus").forEach(btn => {
    btn.addEventListener("click", () => {
        const nombre = btn.dataset.nombre;
        const precio = parseFloat(btn.dataset.precio);
        const idProducto = parseInt(btn.dataset.id);

        if (!idProducto || idProducto < 1) {
            console.error(" ERROR: El botón no tiene un ID válido:", btn);
            return;
        }

        if (!carrito[nombre]) {
            carrito[nombre] = { 
                precio: precio, 
                cantidad: 0,
                id: idProducto
            };
        }

        carrito[nombre].cantidad++;

        const display = document.querySelector(`.qty-display[data-id="${idProducto}"]`);
        if (display) display.textContent = carrito[nombre].cantidad;

        guardarCarrito();
        actualizarContador();
    });
});

document.querySelectorAll(".qty-btn.minus").forEach(btn => {
    btn.addEventListener("click", () => {
        const nombre = btn.dataset.nombre;
        const idProducto = parseInt(btn.dataset.id);

        if (!carrito[nombre]) return;

        carrito[nombre].cantidad--;

        if (carrito[nombre].cantidad <= 0) {
            delete carrito[nombre];
        }

        const display = document.querySelector(`.qty-display[data-id="${idProducto}"]`);
        if (display) display.textContent = carrito[nombre] ? carrito[nombre].cantidad : 0;

        guardarCarrito();
        actualizarContador();
    });
});


//              CONFIRMAR COMPRA

document.getElementById("confirmar-compra")?.addEventListener("click", async () => {

    console.log(" Carrito actual:", carrito);

    if (Object.keys(carrito).length === 0) {
        alert("Tu carrito está vacío.");
        return;
    }

    const detalles = Object.entries(carrito)
        .map(([nombre, item]) => ({
            idProducto: Number(item.id || item.idProducto || 0),
            cantidad: Number(item.cantidad || 0)
        }))
        .filter(det => det.idProducto > 0 && det.cantidad > 0);

    console.log(" Detalles construidos:", detalles);

    if (detalles.length === 0) {
        alert(" Error: el carrito no contiene productos válidos.");
        return;
    }

    const ventaData = {
        idCliente: 1,
        detalles: detalles
    };

    console.log(" JSON ENVIADO A LA API:", ventaData);

    try {
        const response = await fetch("https://localhost:7024/api/Ventas/registrar", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(ventaData)
        });

        const data = await response.json().catch(() => null);

        console.log(" Respuesta API:", response.status, data);

        if (!response.ok) {
            alert(" Error API: " + (data?.mensaje || `HTTP ${response.status}`));
            return;
        }

        alert("Compra registrada con éxito");

        carrito = {};
        guardarCarrito();
        cargarCarritoEnPagina();
        actualizarContador();

    } catch (error) {
        console.error(" Error fetch:", error);
        alert("Error de conexión con la API.");
    }
});

// =========================================
//              INICIALIZAR
// =========================================
cargarCarritoEnPagina();
actualizarContador();
 