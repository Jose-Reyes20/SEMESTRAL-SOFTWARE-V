document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("register-form");
  if (!form) return;

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    // 1. OBTENER DATOS (Ahora coinciden 1 a 1 con el Backend)
    const nombre    = document.getElementById("reg-nombre").value.trim();
    const apellido  = document.getElementById("reg-apellido").value.trim();
    const direccion = document.getElementById("reg-direccion").value.trim();
    const email     = document.getElementById("reg-email").value.trim();
    const telefono  = document.getElementById("reg-telefono").value.trim();
    const password  = document.getElementById("reg-pass").value.trim();

    // 2. CREAR OBJETO (DTO Exacto)
    const datosRegistro = {
      Nombre: nombre,
      Apellido: apellido,
      Direccion: direccion,
      Telefono: telefono,
      Correo: email,
      Contrasena: password
    };

    try {
      mostrarMensaje("Registrando...", false);

      // 3. ENVIAR A LA API
      const response = await apiFetch("/api/auth/register", {
        method: "POST",
        body: JSON.stringify(datosRegistro)
      });

      // 4. MANEJO DE ERRORES
      if (!response.ok) {
        const errorText = await response.text();
        try {
            // Intentar leer error JSON bonito
            const errJson = JSON.parse(errorText);
            // El backend suele devolver el error en 'title' o en un objeto de errores
            const mensaje = errJson.title || JSON.stringify(errJson.errors) || "Error en el registro";
            mostrarMensaje(`Error: ${mensaje}`, true);
        } catch {
            mostrarMensaje(`Error: ${errorText}`, true);
        }
        return;
      }

      // 5. ÉXITO
      mostrarMensaje("¡Cuenta creada! Redirigiendo...", false);
      setTimeout(() => window.location.href = "login.html", 1500);

    } catch (error) {
      console.error("Error crítico:", error);
      mostrarMensaje("Error de conexión con el servidor.", true);
    }
  });
});

function mostrarMensaje(texto, esError = false) {
  let msg = document.querySelector(".login-msg");
  if (!msg) {
    msg = document.createElement("p");
    msg.className = "login-msg";
    msg.style.marginTop = "10px";
    msg.style.fontWeight = "bold";
    msg.style.textAlign = "center";
    const btn = document.querySelector(".login-btn");
    if(btn) btn.parentNode.insertBefore(msg, btn.nextSibling);
  }
  msg.style.color = esError ? "#d9534f" : "#5E925C";
  msg.textContent = texto;
}