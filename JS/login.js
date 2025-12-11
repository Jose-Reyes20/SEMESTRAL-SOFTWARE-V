// =====================================
//   LOGIN REAL CONTRA LA API AUTH
// =====================================

document.getElementById("login-form")?.addEventListener("submit", async function (e) {
  e.preventDefault();

  const email = document.querySelector("#login-form input[type=email]").value.trim();
  const pass  = document.querySelector("#login-form input[type=password]").value.trim();

  // Datos que la API espera
  const credenciales = {
    correo: email,
    password: pass
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

    // API devuelve el usuario ya validado
    const data = await res.json();

    // Normalize user session data to consistent shape
    // API may return: { IdCliente, Nombre, Rol } or { id, nombre, rol }
    const normalizedUser = {
      id: data.IdCliente || data.idCliente || data.id || null,
      nombre: data.Nombre || data.nombre || data.name || "Usuario",
      rol: (data.Rol || data.rol || "cliente").toLowerCase()
    };

    // Guardar sesión normalizada
    localStorage.setItem("usuarioConectado", JSON.stringify(normalizedUser));

    // Redirección
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
