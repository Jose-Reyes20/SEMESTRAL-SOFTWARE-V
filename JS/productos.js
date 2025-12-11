// =============================
//   JS PARA PÁGINA PRODUCTOS
// =============================
(() => {
  // LocalStorage para carrito
  let carrito = {};
  try {
    carrito = JSON.parse(localStorage.getItem("carrito")) || {};
  } catch {
    carrito = {};
  }

  const cartCountEl = document.getElementById("cart-count");

  function guardarCarrito() {
    localStorage.setItem("carrito", JSON.stringify(carrito));
  }

  function totalItemsCarrito() {
    return Object.values(carrito).reduce((acc, it) => acc + (it.cantidad || 0), 0);
  }

  function actualizarBadge() {
    if (cartCountEl) {
      cartCountEl.textContent = totalItemsCarrito();
    }
  }

  function actualizarDisplayCantidad(id) {
    const display = document.querySelector(`.qty-display[data-id="${id}"]`);
    if (display && carrito[id]) {
      display.textContent = carrito[id].cantidad;
    } else if (display) {
      display.textContent = 0;
    }
  }

  function initDisplaysDesdeCarrito() {
    document.querySelectorAll(".qty-display").forEach(disp => {
      const id = disp.getAttribute("data-id");
      if (id && carrito[id]) {
        disp.textContent = carrito[id].cantidad;
      } else {
        disp.textContent = 0;
      }
    });
  }

  function onClickQty(e) {
    const btn = e.currentTarget;
    const id = btn.dataset.id;
    const nombre = btn.dataset.nombre || `Producto ${id}`;
    const precio = Number.parseFloat(btn.dataset.precio) || 0;

    if (!id) return;

    if (!carrito[id]) {
      carrito[id] = { nombre, precio, cantidad: 0 };
    }

    if (btn.classList.contains("plus")) {
      carrito[id].cantidad += 1;
    } else if (btn.classList.contains("minus") && carrito[id].cantidad > 0) {
      carrito[id].cantidad -= 1;
    }

    actualizarDisplayCantidad(id);
    actualizarBadge();
    guardarCarrito();
  }

  // Esperar a que esté el DOM para enganchar listeners
  document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".qty-btn").forEach(btn => {
      btn.addEventListener("click", onClickQty);
    });

    // Pinta cantidades guardadas y badge al cargar
    initDisplaysDesdeCarrito();
    actualizarBadge();
  });

  // Navbar compacta al hacer scroll (SIN <script> en archivos .js)
  window.addEventListener("scroll", () => {
    if (window.scrollY > 20) {
      document.body.classList.add("scrolled");
    } else {
      document.body.classList.remove("scrolled");
    }
  });
})();

