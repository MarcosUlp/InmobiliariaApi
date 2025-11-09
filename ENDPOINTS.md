**AUTENTICACION**
# 🔸1) Register
- Método: **POST**
- Ruta: **/api/auth/registrar**
- Tipo de envio: **(JSON)**

- Body: {
  "nombre": "Lucía Fernández",
  "email": "lucia@example.com",
  "telefono": "1123456789",
  "clave": "123"
}

- Respuesta 200: {
  "mensaje": "Propietario registrado correctamente."
}

# 🔸2) Login

- Método: **POST**
- Ruta: **/api/auth/login**
- Tipo de envío: **(JSON)**

- Body: { 
    "Email": "lucia@example.com"
    "Clave": "123"
}

- Respuesta 200: JSON con el token JWT y el propietario : {
    "token": "..." 
    "propietario": "..."
}
------------------------------------------------------------------------------------------------------------
# 1)🔸Get Propietario

- Método: **GET**
- Ruta: **/api/propietario/perfil**
- Tipo de envio: **(JSON)**
- headers: Authorization: Barer <token>
- Respuesta: **propietario autenticado**:{
    "propietarioId": 1,
    "nombre": "Lucía Giraudi",
    "email": "lucia@example.com",
    "telefono": "123456789"
}

# 2)🔸Editar Perfil

- Método: **PUT**
- Ruta: **api/propietario/editar**
- Tipo de envio: **(JSON)**
- Headers: Authorization: Barer <token>
- Respuesta: void(boolean)

- Body: {
    "nombre": "Lucía F. Giraudi",
    "telefono": "1133344455"
}

- Respuesta: {
    "mensaje": "Perfil actualizado correctamente."
}

# 3)🔸Cambiar Clave

- Método: **PUT**
- Ruta: **api/propietario/cambiar-clave**
- Tipo de envio: **(JSON)**
- Headers: Authorization: Barer <token>

- Body: {
    "claveActual": "123",
    "nuevaClave": "456"
}

- Respuesta: {
    "mensaje": "Clave cambiada correctamente."
}
------------------------------------------------------------------------------------------------------------

# 🔸1) Agregar nuevo inmueble (por defecto deshabilitado)

- **Método:** POST  
- **Ruta:** `/api/Inmuebles`  
- **Tipo de envío:** form-data
- **Headres:** authorization: Barer <token>

- **Body:** 
direccion       Belgrano123
ambientes       3   
precio          450000
imagen          foto-alquiler.jpeg

- **Respuesta**:{
    "mensaje": "Inmueble agregado correctamente y deshabilitado por defecto."   
}

# 🔸2) Traer Inmueble por id
- Metodo: **GET**
- Ruta: `api/Inmuebles/{id}`
- Tipo de envio: sin envio
- Headers: Authorization barer <token>
- Respuesta: Inmueble{

}

# 🔸2) Listar Inmuebles

- Metodo: **GET**
- Ruta: `api/Inmuebles`
- Tipo de envio: sin envio
- Headers: Authorization barer <token>
- Respuesta: List<Inmueble>{
}


# 🔸3) Cambiar estado (habilitar/deshabilitar inmueble)

- Metodo: **PUT**
- Ruta: **/api/Inmuebles/{id}/estado**
- tipo de envio: **ninguno**

**no se envia nada el controlador reconoce el estado actual del inmueble y solo lo invierte**{
}
------------------------------------------------------------------------------------------------------------

***ENPOINTS PARA CONTRATO***
# 🔸1) Crear Contrato

- Método: **POST**
- Ruta: **api/Contratos**
- Tipo de envio: **(JSON)**
- Body {
    "inmuebleId": 1,
    "fechaInicio": "2025-11-01",
    "fechaFin": "2026-11-01",
    "monto": 250000
}
- Headers: Authorization: Barer <token>
- Respuesta: {
    "mensaje": "Contrato creado correctamente.",
    "contrato"{
    }
}

# 🔸2) Listar contratos por inmueble

- Método: **GET**
- Ruta: **/api/Contratos/inmueble/{id}**
- Tipo de envio: **ninguno**
- Headers: Authorization: Barer <token>

- Respuesta: Lista de contratos asociados al inmueble

- devuelve un contrato con todos sus respectivos pagos{

}
------------------------------------------------------------------------------------------------------------
# 🔸1)Registrar pago
- Metodo: **POST**
- Ruta: **/api/pagos**
- Tipo de envio: (JSON)
- Headers: Authorization: Barer <token>

- Body: json { 
    "contratoId": 1,
    "fechaPago":
    "2025-12-01", 
    "importe": 120000 
    }

- Respuesta: 

# 🔸2) Listar pagos por contrato
- Metodo: **GET**
- Ruta: **/api/pagos/Contrato/{contratoId}**
- Tipo de envio: **sin envio**
- Headers: Authorization: Barer <token>

- Respuesta: List<Pagos>: json{
    pagoId:,
    contratoId:,
    importe... etc
}
