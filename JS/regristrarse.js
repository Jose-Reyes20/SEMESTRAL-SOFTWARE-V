document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("register-form");
  if (!form) return;

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const nombre = document.getElementById("reg-nombre").value.trim();
    const email  = document.getElementById("reg-email").value.trim();
    const tel    = document.getElementById("reg-telefono").value.trim();
    const pass   = document.getElementById("reg-pass").value.trim();

    const nuevoCliente = {
      nombre,
      correo: email,
      telefono: tel,
      password: pass,
      fechaRegistro: new Date().toISOString()
    };

    try {
      const res = await apiFetch("/api/Clientes", {
        method: "POST",
        body: JSON.stringify(nuevoCliente)
      });

      if (!res.ok) {
        const errorText = await res.text();
        console.error("Error API:", errorText);
        mostrarMensaje(`Error al registrarse: ${errorText}`, true);
        return;
      }

      const data = await res.json();
      mostrarMensaje("Cuenta creada con éxito. Redirigiendo...");

      // Normalize user session data to consistent shape
      // API may return: { IdCliente, Nombre, Rol } or { id, nombre, rol }
      const normalizedUser = {
        id: data.IdCliente || data.idCliente || data.id || null,
        nombre: data.Nombre || data.nombre || data.name || nombre,
        rol: (data.Rol || data.rol || "cliente").toLowerCase()
      };

      // Guardar usuario en sesión local
      localStorage.setItem("usuarioConectado", JSON.stringify(normalizedUser));

      setTimeout(() => location.href = "index.html", 1200);

    } catch (error) {
      console.error("Error de conexión:", error);
      mostrarMensaje("No se pudo conectar con la API. Verifica que el backend esté en ejecución.", true);
    }
  });
});

function mostrarMensaje(texto, esError = false) {
  let msg = document.querySelector(".login-msg");
  if (!msg) {
    msg = document.createElement("p");
    msg.className = "login-msg";
    msg.style.marginTop = "12px";
    msg.style.fontWeight = "700";
    document.getElementById("register-form").appendChild(msg);
  }
  msg.style.color = esError ? "#d9534f" : "#5E925C";
  msg.textContent = texto;
}
