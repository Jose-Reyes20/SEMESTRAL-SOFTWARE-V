// =====================================
//   LOGIN REAL CONTRA LA API AUTH
// =====================================

document.getElementById("login-form")?.addEventListener("submit", async function (e) {
  e.preventDefault();

  // 1. Obtener datos por ID (Más seguro)
  const correo = document.getElementById("login-email").value.trim();
  const pass   = document.getElementById("login-pass").value.trim();

  // 2. Preparar credenciales
  // NOTA: Aunque el usuario escriba su correo, la API espera la propiedad "usuario"
  const credenciales = {
    usuario: correo, 
    contrasena: pass
  };

  try {
    mostrarError(""); // Limpiar errores previos

    // 3. Enviar a la API
    const res = await apiFetch("/api/auth/login", {
      method: "POST",
      body: JSON.stringify(credenciales)
    });

    // 4. Manejar error de credenciales
    if (!res.ok) {
      const errorText = await res.text();
      try {
          const errJson = JSON.parse(errorText);
          mostrarError(errJson.message || "Credenciales incorrectas.");
      } catch {
          mostrarError("Correo o contraseña incorrectos.");
      }
      return;
    }

    // 5. Login Exitoso
    const data = await res.json();
    console.log("Login OK:", data);

    // 6. Guardar sesión
    const sessionData = {
      id: data.usuarioId,
      cliente_id: data.clienteId,
      nombre: data.rol === 'admin' ? 'Administrador' : 'Cliente',
      rol: data.rol,
      token: data.token
    };

    localStorage.setItem("usuarioConectado", JSON.stringify(sessionData));

    // 7. Redirigir
    if (sessionData.rol && sessionData.rol.toLowerCase() === "admin") {
      window.location.href = "admin.html";
    } else {
      window.location.href = "index.html";
    }

  } catch (error) {
    mostrarError("Error de conexión con el servidor.");
    console.error(error);
  }
});

function mostrarError(mensaje) {
  let errorBox = document.querySelector(".login-error");
  
  if (!mensaje) {
      if (errorBox) errorBox.remove();
      return;
  }

  if (!errorBox) {
    errorBox = document.createElement("p");
    errorBox.className = "login-error";
    errorBox.style.color = "#d9534f";
    errorBox.style.fontWeight = "bold";
    errorBox.style.marginTop = "10px";
    errorBox.style.textAlign = "center";
    
    const btn = document.querySelector(".login-btn");
    if(btn) btn.parentNode.insertBefore(errorBox, btn.nextSibling);
  }
  errorBox.textContent = mensaje;
}