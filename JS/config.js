// =====================================
//   CONFIGURACIÓN CENTRALIZADA DE API
// =====================================

// IMPORTANTE: Apuntamos al puerto 5224 (HTTP) que sale en tu pantalla negra.
let API_BASE = 'http://localhost:5224';

/**
 * Construye la URL completa para la API
 * Ejemplo: getApiUrl('/api/articulos') -> 'http://localhost:5224/api/articulos'
 */
function getApiUrl(path) {
  // Aseguramos que el path empiece con /
  if (!path.startsWith('/')) {
    path = '/' + path;
  }
  return API_BASE + path;
}

// Encabezados por defecto (siempre enviamos JSON)
const DEFAULT_FETCH_OPTIONS = {
  headers: {
    'Content-Type': 'application/json'
  }
};

/**
 * Función auxiliar para hacer peticiones (fetch)
 * Se encarga de poner la URL base automáticamente.
 */
async function apiFetch(url, options = {}) {
  // Si la url no empieza con http (ej: "/api/login"), le pegamos la base
  if (!url.startsWith('http')) {
    url = getApiUrl(url);
  }
  
  // Combinar las opciones por defecto con las que envíes
  const mergedOptions = {
    ...DEFAULT_FETCH_OPTIONS,
    ...options,
    headers: {
      ...DEFAULT_FETCH_OPTIONS.headers,
      ...(options.headers || {})
    }
  };
  
  try {
    const response = await fetch(url, mergedOptions);
    return response;
  } catch (error) {
    console.error("Error crítico de conexión con la API:", error);
    // Lanzamos el error para que lo maneje el script individual
    throw error;
  }
}

// Exportar variables al objeto window para que funcionen en otros archivos JS
window.API_BASE = API_BASE;
window.getApiUrl = getApiUrl;
window.apiFetch = apiFetch;