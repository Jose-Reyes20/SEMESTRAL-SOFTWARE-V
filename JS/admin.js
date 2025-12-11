document.addEventListener("DOMContentLoaded", () => {
    cargarDashboard();
});

async function cargarDashboard() {
    try {
        // 1. OBTENER PRODUCTOS
        const resProductos = await apiFetch("/api/articulos");
        if (resProductos.ok) {
            const productos = await resProductos.json();
            document.getElementById("total-productos").textContent = productos.length;
        }

        // 2. OBTENER CLIENTES
        const resClientes = await apiFetch("/api/clientes"); // Asegúrate que tu Controller tenga ruta 'clientes' o 'cliente'
        if (resClientes.ok) {
            const clientes = await resClientes.json();
            document.getElementById("total-clientes").textContent = clientes.length;
        }

        // 3. OBTENER ÓRDENES
        const resOrdenes = await apiFetch("/api/ordenes");
        if (resOrdenes.ok) {
            const ordenes = await resOrdenes.json();
            
            // Actualizar tarjeta de cantidad
            document.getElementById("total-ordenes").textContent = ordenes.length;

            // Calcular ingresos totales (Suma del total de todas las órdenes)
            const ingresos = ordenes.reduce((sum, ord) => sum + (ord.total || 0), 0);
            document.getElementById("total-ingresos").textContent = "$" + ingresos.toFixed(2);

            // Llenar tabla (Mostrar solo las últimas 5)
            llenarTablaOrdenes(ordenes.slice(-5).reverse());
        }

    } catch (error) {
        console.error("Error cargando dashboard:", error);
    }
}

function llenarTablaOrdenes(ordenes) {
    const tbody = document.getElementById("tabla-ordenes");
    tbody.innerHTML = "";

    if (ordenes.length === 0) {
        tbody.innerHTML = `<tr><td colspan="5" style="text-align:center;">No hay órdenes registradas.</td></tr>`;
        return;
    }

    ordenes.forEach(orden => {
        const tr = document.createElement("tr");
        
        // Formatear fecha
        const fecha = new Date(orden.fecha).toLocaleDateString();
        
        // Clase para el color del estado
        const estadoClass = orden.estado ? orden.estado.toLowerCase() : "pendiente";

        tr.innerHTML = `
            <td>#${orden.id}</td>
            <td>${fecha}</td>
            <td>${orden.usuarioId || "Usuario"}</td>
            <td>$${orden.total.toFixed(2)}</td>
            <td><span class="estado ${estadoClass}">${orden.estado || "Pendiente"}</span></td>
        `;
        tbody.appendChild(tr);
    });
}