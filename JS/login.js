// =====================================
//   LOGIN REAL CONTRA LA API AUTH
// =====================================

document.getElementById("login-form")?.addEventListener("submit", async function (e) {
  e.preventDefault();

  const correo = document.querySelector("#login-form input[type=email]").value.trim();
  const pass   = document.querySelector("#login-form input[type=password]").value.trim();

  // Lo que tu API debe recibir según tu tabla "usuario"
  const credenciales = {
    usuario: correo,        // en tu BD es campo 'usuario'
    contrasena: pass        // en tu BD es campo 'contrasena'
  };

  try {
    const res = await apiFetch("/api/Auth/login", {
      method: "POST",
      body: JSON.stringify(credenciales)
    });

    if (!res.ok) {
      const errorText = await res.text();
      mostrarError(errorText || "Correo o contraseña incorrectos.");
      console.error("Login error:", errorText);
      return;
    }

    // API devuelve la información del usuario ya autenticado
    const data = await res.json();

    // Normalizar estructura de sesión según tu BD
    const normalizedUser = {
      id: data.id || data.Id || data.idUsuario || null,
      cliente_id: data.cliente_id || data.ClienteId || null,
      nombre: data.nombre || data.Nombre || "Usuario",
      rol: (data.rol || data.Rol || "cliente").toLowerCase()
    };

    // Guardar sesión del usuario
    localStorage.setItem("usuarioConectado", JSON.stringify(normalizedUser));

    // Redirección según rol
    if (normalizedUser.rol === "admin") {
      window.location.href = "admin.html";
    } else {
      window.location.href = "index.html";
    }

  } catch (error) {
    mostrarError("No se pudo conectar con la API. Verifica que el backend esté en ejecución.");
    console.error("Connection error:", error);
  }
});

function mostrarError(mensaje) {
  let errorBox = document.querySelector(".login-error");
  if (!errorBox) {
    errorBox = document.createElement("p");
    errorBox.className = "login-error";
    errorBox.style.color = "#d9534f";
    errorBox.style.fontWeight = "700";
    errorBox.style.marginTop = "10px";
    document.getElementById("login-form").appendChild(errorBox);
  }
  errorBox.textContent = mensaje;
}

