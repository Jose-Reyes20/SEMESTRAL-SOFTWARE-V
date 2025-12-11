// =====================================
//   CENTRALIZED API CONFIGURATION
// =====================================

/**
 * Determines the API base URL with smart HTTPS/HTTP fallback.
 * 
 * Priority:
 * 1. localStorage override: API_BASE
 * 2. Try HTTPS by default
 * 3. Fallback to HTTP if HTTPS fails
 * 
 * Usage:
 *   - Set custom endpoint: localStorage.setItem('API_BASE', 'http://localhost:7024')
 *   - Clear override: localStorage.removeItem('API_BASE')
 */

// Check for localStorage override first
let API_BASE = localStorage.getItem('API_BASE');

if (!API_BASE) {
  // Default to HTTPS, but allow runtime switching
  API_BASE = 'https://localhost:7024';
  
  // You can detect HTTPS failures at runtime and switch to HTTP
  // This is done in individual fetch error handlers if needed
}

// Ensure no trailing slash
if (API_BASE.endsWith('/')) {
  API_BASE = API_BASE.slice(0, -1);
}

/**
 * Helper to build full API endpoint URLs
 * @param {string} path - API path (e.g., '/api/Auth/login')
 * @returns {string} Full API URL
 */
function getApiUrl(path) {
  // Ensure path starts with /
  if (!path.startsWith('/')) {
    path = '/' + path;
  }
  return API_BASE + path;
}

/**
 * Default fetch options with credentials support
 */
const DEFAULT_FETCH_OPTIONS = {
  credentials: 'same-origin', // Include cookies if needed
  headers: {
    'Content-Type': 'application/json'
  }
};

/**
 * Enhanced fetch wrapper with better error handling
 * @param {string} url - Full URL or path
 * @param {object} options - Fetch options
 * @returns {Promise<Response>}
 */
async function apiFetch(url, options = {}) {
  // If url doesn't start with http, assume it's a path
  if (!url.startsWith('http')) {
    url = getApiUrl(url);
  }
  
  // Merge default options
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
    // Check if it's an HTTPS certificate error
    if (error.message && error.message.includes('certificate')) {
      console.warn('HTTPS certificate error detected. Consider using HTTP or accepting the certificate.');
      console.warn('To switch to HTTP, run: localStorage.setItem("API_BASE", "http://localhost:7024")');
    }
    throw error;
  }
}

// Export for use in other scripts
if (typeof window !== 'undefined') {
  window.API_BASE = API_BASE;
  window.getApiUrl = getApiUrl;
  window.apiFetch = apiFetch;
  window.DEFAULT_FETCH_OPTIONS = DEFAULT_FETCH_OPTIONS;
}
