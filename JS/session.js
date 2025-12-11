// ===============================
//   MANEJO ESPECIAL PARA ADMIN.HTML
// ===============================
if (window.location.pathname.includes("admin.html")) {
    document.addEventListener("DOMContentLoaded", () => {

        const usuarioData = localStorage.getItem("usuarioConectado");
        const userArea = document.getElementById("user-area");

        if (!usuarioData || !userArea) return;

        let usuario;
        try {
            usuario = JSON.parse(usuarioData);
        } catch (e) {
            console.error("Error parsing user session:", e);
            return;
        }

        // Normalize rol to lowercase for consistent checking
        const rol = (usuario.rol || "cliente").toLowerCase();

        // Si existe sesión y es admin → mostrar info
        if (usuario && rol === "admin") {
            userArea.innerHTML = `
                <span style="color:#5E925C; font-weight:700;">Admin</span>
                <button id="logout-btn"
                        style="margin-left:10px;background:#ff5b5b;border:none;color:white;padding:6px 10px;border-radius:8px;cursor:pointer;">
                    Salir
                </button>`;
        }

        // Botón salir
        const logoutBtn = document.getElementById("logout-btn");
        if (logoutBtn) {
            logoutBtn.addEventListener("click", () => {
                localStorage.removeItem("usuarioConectado");
                window.location.href = "login.html";
            });
        }
    });

    // IMPORTANTE: NO HACEMOS return;
    // Ahora admin.html también puede manejar user-area normalmente
}


// ===============================
//  MOSTRAR NOMBRE DEL USUARIO EN TODAS LAS DEMÁS PÁGINAS
// ===============================
document.addEventListener("DOMContentLoaded", () => {

    const userArea = document.getElementById("user-area");
    if (!userArea) return;

    const usuarioData = localStorage.getItem("usuarioConectado");
    if (!usuarioData) return;

    let usuario;
    try {
        usuario = JSON.parse(usuarioData);
    } catch (e) {
        console.error("Error parsing user session:", e);
        return;
    }

    if (!usuario) return;

    // Normalize rol to lowercase for consistent checking
    const rol = (usuario.rol || "cliente").toLowerCase();

    if (rol === "admin") {

        userArea.innerHTML = `
            <span style="color:#5E925C; font-weight:700;">Admin</span>

            <a href="admin.html" 
                style="margin-left:10px; background:#8dce88; padding:6px 10px; border-radius:8px; color:white; font-weight:700;">
                Panel
            </a>

            <button id="logout-btn" style="
                margin-left:10px;
                background:#ff5b5b;
                border:none;
                color:white;
                padding:6px 10px;
                border-radius:8px;
                cursor:pointer;
            ">Salir</button>
        `;

    } else {

        userArea.innerHTML = `
            <span style="color:#5E925C; font-weight:700;">Hola, ${usuario.nombre || "Usuario"}</span>

            <button id="logout-btn" style="
                margin-left:10px;
                background:#ff5b5b;
                border:none;
                color:white;
                padding:6px 10px;
                border-radius:8px;
                cursor:pointer;
            ">Salir</button>
        `;
    }

    // Evento salir
    const logoutBtn = document.getElementById("logout-btn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", () => {
            localStorage.removeItem("usuarioConectado");
            window.location.href = "login.html";
        });
    }
});
