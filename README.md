# Backend API - User Management System

Este proyecto es el backend para un sistema de gestión de usuarios que permite realizar operaciones CRUD sobre usuarios y autenticación mediante JWT. El backend está construido con .NET, y expone endpoints REST para manejar datos de usuarios como nombre, nombre de usuario, email, número de teléfono, cambio de contraseña, y eliminación de cuenta.

## Tabla de Contenidos
- [Requisitos](#requisitos)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Ejecución del Proyecto](#ejecución-del-proyecto)
- [Endpoints](#endpoints)
  - [Autenticación](#autenticación)
  - [Operaciones sobre usuarios](#operaciones-sobre-usuarios)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)

## Requisitos

- .NET 6.0 SDK o superior
- SQL Server (o cualquier otra base de datos relacional compatible)
- Postman (opcional, para pruebas de la API)

## Instalación

1. Clona este repositorio:
   ```bash
   git clone https://github.com/usuario/backend-user-management.git
   ```
2. Ve al directorio del proyecto:

  ```bash
  cd backendapp
  ```
3. Restaura las dependencias de NuGet:

  ```bash
  dotnet restore
  ```
4. Configura la base de datos en tu entorno local.

## Configuración
Configuración del archivo `appsettings.json`
Edita el archivo appsettings.json para que se ajuste a tu entorno de desarrollo, en particular la conexión a la base de datos:

 ```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=UserDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "Secret": "your-secret-key",
    "Issuer": "your-app-name",
    "Audience": "your-app-name",
    "ExpirationInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
 ```

Migraciones de la Base de Datos
Ejecuta las migraciones para configurar la base de datos:

```bash
dotnet ef database update
```

## Ejecución del Proyecto
1. Para ejecutar el servidor localmente, utiliza el siguiente comando:

```bash
dotnet run
```
2. La API estará disponible en http://localhost:5119.

## Endpoints
### Autenticación
`POST /api/auth/login`
Autentica un usuario y devuelve un token JWT.
- Request Body:

```json
{
  "username": "user123",
  "password": "password123"
}
```
- Response:

```json
{
  "token": "jwt-token"
}
```
### Operaciones sobre Usuarios
`GET /api/users/{userId}`
Obtiene los datos del usuario por su ID.

- Response:
```json
{
  "id": 1,
  "name": "Daniel Triveño",
  "userName": "danieltriv",
  "email": "daniel@example.com",
  "phoneNumber": "123456789"
}
```
`PATCH /api/users/{userId}/name`
Actualiza el nombre del usuario.

- Request Body:

```json
{
  "name": "Nuevo Nombre"
}
```
- Response:

```json
{
  "message": "Name updated successfully."
}
```
`PATCH /api/users/{userId}/change-password`
Cambia la contraseña del usuario.

- Request Body:

```json
{
  "currentPassword": "password123",
  "newPassword": "password456",
  "confirmPassword": "password456"
}
```
- Response:

```json
{
  "message": "Password updated successfully."
}
```
`DELETE /api/users/{userId}`
Elimina la cuenta del usuario.

- Response:
```json
{
  "message": "User account deleted successfully."
}
```

## Tecnologías Utilizadas
- .NET 6.0: Framework principal del backend.
- Entity Framework Core: Para la interacción con la base de datos.
- SQL Server: Base de datos utilizada.
- JWT: Autenticación basada en tokens.
- AutoMapper: Mapeo de objetos entre las entidades y los DTOs.
