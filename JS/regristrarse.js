document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("register-form");
  if (!form) return;

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const nombre   = document.getElementById("reg-nombre").value.trim();
    const apellido = document.getElementById("reg-apellido").value.trim();
    const direccion = document.getElementById("reg-direccion").value.trim();
    const telefono = document.getElementById("reg-telefono").value.trim();
    const correo   = document.getElementById("reg-email").value.trim();
    const pass     = document.getElementById("reg-pass").value.trim();

    //  Crear objeto según estructura real de la tabla CLIENTE
    const nuevoCliente = {
      nombre,
      apellido,
      direccion,
      telefono,
      correo
    };

    try {
      // PRIMERA PETICIÓN: registrar al cliente
      const resCliente = await apiFetch("/api/cliente", {
        method: "POST",
        body: JSON.stringify(nuevoCliente)
      });

      if (!resCliente.ok) {
        const errorText = await resCliente.text();
        mostrarMensaje(`Error al registrar cliente: ${errorText}`, true);
        return;
      }

      const clienteData = await resCliente.json();
      const clienteId = clienteData.id || clienteData.Id || clienteData.idCliente;

      // Validar
      if (!clienteId) {
        mostrarMensaje("Error: la API no retornó el ID del cliente.", true);
        return;
      }

      // SEGUNDA PETICIÓN: registrar el usuario enlazado al cliente
      const nuevoUsuario = {
        usuario: correo,
        contrasena: pass,
        rol: "cliente",
        cliente_id: clienteId
      };

      const resUsuario = await apiFetch("/api/usuario", {
        method: "POST",
        body: JSON.stringify(nuevoUsuario)
      });

      if (!resUsuario.ok) {
        const errorText = await resUsuario.text();
        mostrarMensaje(`Error al crear usuario: ${errorText}`, true);
        return;
      }

      const usuarioData = await resUsuario.json();

      // Normalizar sesión
      const normalizedUser = {
        id: usuarioData.id || usuarioData.Id || null,
        nombre,
        rol: "cliente",
        cliente_id: clienteId
      };

      localStorage.setItem("usuarioConectado", JSON.stringify(normalizedUser));

      mostrarMensaje("Cuenta creada con éxito. Redirigiendo...");

      setTimeout(() => (location.href = "index.html"), 1200);

    } catch (error) {
      console.error("Error de conexión:", error);
      mostrarMensaje("No se pudo conectar con la API. ¿Está el backend corriendo?", true);
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
