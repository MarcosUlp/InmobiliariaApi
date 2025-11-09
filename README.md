# 🏠 Inmobiliaria API

API REST desarrollada en **.NET 8 Web API** para la gestión de inmuebles, contratos y pagos de una inmobiliaria.  
Esta API está pensada para ser consumida por una **aplicación móvil de propietarios**, permitiendo administrar sus propiedades y contratos de manera segura mediante **autenticación por JWT**.

---

## 🚀 Tecnologías utilizadas

- **ASP.NET Core 8.0**
- **Entity Framework Core**
- **SQL Server / SQLite**
- **JWT (JSON Web Token)** para autenticación
- **Swagger** para documentación de endpoints
- **Postman** para pruebas

---

## 🔐 Autenticación

Todos los endpoints (excepto *Login*) requieren un **token JWT** válido.  
El token se obtiene al iniciar sesión y debe enviarse en cada request con el encabezado:

-------------------------------------------------
| Inmobiliaria API                              |
-------------------------------------------------
| 🔑 AuthController                             |
|   POST /api/Auth/login                        |
-------------------------------------------------
| 🏘️ InmueblesController                        |
|   GET  /api/Inmuebles                         |
|   POST /api/Inmuebles                         |
|   PUT  /api/Inmuebles/{id}/estado             |
-------------------------------------------------
| 📜 ContratosController                        |
|   GET  /api/Contratos/inmueble/{id}           |
|   POST /api/Contratos                         |
-------------------------------------------------
| 💰 PagosController                            |
|   GET  /api/Pagos/contrato/{id}               |
|   POST /api/Pagos                             |
-------------------------------------------------

