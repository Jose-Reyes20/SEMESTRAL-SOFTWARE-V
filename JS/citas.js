document.addEventListener("DOMContentLoaded", async () => {
    const usuarioData = localStorage.getItem("usuarioConectado");
    if (!usuarioData) {
        alert("Debes iniciar sesión para agendar una cita.");
        window.location.href = "login.html";
        return;
    }

    let usuario;
    try {
        usuario = JSON.parse(usuarioData);
    } catch (e) {
        console.error("Error parsing user session:", e);
        alert("Error de sesión. Por favor inicia sesión nuevamente.");
        window.location.href = "login.html";
        return;
    }

    const apiBaseCitas = getApiUrl("/api/Citas");

    const listaCitas = document.getElementById("listaCitas");
    const form = document.getElementById("formCita");
    const mensajeConfirmacion = document.getElementById("mensajeConfirmacion");

    const selectServicio = document.getElementById("servicio");
    const selectEstilista = document.getElementById("estilista");

    // Cargar citas del usuario
    async function cargarCitas() {
        listaCitas.innerHTML = "<li>Cargando citas...</li>";
        try {
            const res = await fetch(apiBaseCitas);
            
            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }
            
            const citas = await res.json();
            
            // Robust filtering - check multiple ID fields and coerce to Number
            const userId = Number(usuario.id);
            const citasCliente = citas.filter(c => {
                const citaClienteId = Number(c.IdCliente || c.idCliente || c.clienteId);
                return citaClienteId === userId;
            });

            listaCitas.innerHTML = "";
            if (citasCliente.length === 0) {
                listaCitas.innerHTML = "<li>No tienes citas agendadas.</li>";
                return;
            }

            citasCliente.forEach(c => {
                const li = document.createElement("li");
                li.innerHTML = `
                    ${c.Servicio?.NombreServicio || "Servicio"} con ${c.Estilista?.Nombre || "Estilista"} el ${new Date(c.Fecha).toLocaleString()}
                    <button data-id="${c.IdCita || c.idCita}" class="btn-cancelar">Cancelar</button>
                `;
                listaCitas.appendChild(li);
            });

            document.querySelectorAll(".btn-cancelar").forEach(btn => {
                btn.addEventListener("click", async () => {
                    const id = btn.dataset.id;
                    if (confirm("¿Deseas cancelar esta cita?")) {
                        try {
                            const res = await fetch(`${apiBaseCitas}/${id}`, { method: "DELETE" });
                            if (res.ok) {
                                alert("Cita cancelada exitosamente.");
                                cargarCitas();
                            } else {
                                const errorText = await res.text();
                                alert("No se pudo cancelar la cita: " + errorText);
                            }
                        } catch (err) {
                            console.error("Error canceling appointment:", err);
                            alert("Error de conexión al cancelar la cita.");
                        }
                    }
                });
            });

        } catch (error) {
            console.error("Error al cargar citas:", error);
            listaCitas.innerHTML = "<li style='color:#d9534f;'>Error al cargar citas. Verifica tu conexión con la API.</li>";
        }
    }

    await cargarCitas();

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const servicio = Number(selectServicio.value);
    const fecha = document.getElementById("fecha").value;
    const hora = document.getElementById("hora").value;
    const estilista = Number(selectEstilista.value);

    if (!servicio || !fecha || !hora || !estilista) {
        mensajeConfirmacion.textContent = "Completa todos los campos.";
        return;
    }

    // Validar horario 09:00 - 18:00
    const [hh, mm] = hora.split(":").map(Number);
    if (hh < 9 || hh >= 18) {
        mensajeConfirmacion.textContent = "La cita debe estar dentro del horario del salón (09:00 a 18:00).";
        mensajeConfirmacion.style.color = "#d9534f";
        return;
    }

    // 🔥 ENVIAR LA FECHA TAL CUAL (sin convertir, sin ISO)
    let fechaHora = `${fecha}T${hora}`;

    const nuevaCita = {
        IdCliente: Number(usuario.id),
        IdEstilista: estilista,
        IdServicio: servicio,
        Fecha: fechaHora, // <-- AQUÍ SE ENVÍA TAL CUAL
        Estado: "Confirmado"
    };

    console.log("Cita a enviar:", nuevaCita);

    try {
        const res = await apiFetch("/api/Citas", {
            method: "POST",
            body: JSON.stringify(nuevaCita)
        });

        if (res.ok) {
            mensajeConfirmacion.textContent = "✓ Cita agendada correctamente.";
            mensajeConfirmacion.style.color = "#5E925C";
            form.reset();
            setTimeout(() => {
                mensajeConfirmacion.textContent = "";
            }, 3000);
            cargarCitas();
        } else {
            const errorText = await res.text();
            let errorMsg = "Error al agendar cita.";
            try {
                const errorJson = JSON.parse(errorText);
                errorMsg = errorJson.mensaje || errorJson.message || errorText;
            } catch (e) {
                errorMsg = errorText || errorMsg;
            }
            mensajeConfirmacion.textContent = errorMsg;
            mensajeConfirmacion.style.color = "#d9534f";
            console.error("Appointment error:", errorText);
        }
    } catch (err) {
        console.error("Connection error:", err);
        mensajeConfirmacion.textContent = "Error de conexión con la API.";
        mensajeConfirmacion.style.color = "#d9534f";
    }
});

});
