# JobTracker API
 
API REST para gestionar búsquedas de empleo. Permite registrar postulaciones, hacer seguimiento de su estado y obtener estadísticas sobre el proceso de búsqueda.
 
> Proyecto construido como portfolio profesional, aplicando Clean Architecture, CQRS, JWT y buenas prácticas de desarrollo backend en .NET.
 
**Demo en vivo:** https://jobtracker-api-mydx.onrender.com/swagger/index.html
 
---
 
## ¿Por qué existe este proyecto?
 
Buscar trabajo es un proceso que puede volverse caótico rápidamente: postulaciones sin respuesta, entrevistas en distintas etapas, empresas que te contactan semanas después. Este proyecto nació de esa necesidad real: tener un sistema que centralice y mida el proceso.
 
---
 
## Stack tecnológico
 
- **Lenguaje:** C# / .NET 10
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Base de datos:** PostgreSQL
- **Autenticación:** JWT Bearer
- **Documentación:** Swagger / OpenAPI
- **Contenedores:** Docker / Docker Compose
- **Testing:** xUnit + FluentAssertions
---
 
## Arquitectura
 
El proyecto sigue los principios de **Clean Architecture**, separando responsabilidades en cuatro capas independientes:
 
```
JobTracker/
├── JobTracker.Domain/          → Entidades, interfaces, enums. Sin dependencias externas.
├── JobTracker.Application/     → Casos de uso (CQRS), DTOs, excepciones.
├── JobTracker.Infrastructure/  → EF Core, repositorios, JWT, BCrypt, PostgreSQL.
├── JobTracker.API/             → Controllers, Swagger, Program.cs.
└── JobTracker.Tests/           → Tests unitarios con xUnit y FluentAssertions.
```
 
Las dependencias siempre apuntan hacia el Domain. Infrastructure y API dependen de las capas internas, nunca al revés.
 
### Patrón CQRS
 
Los casos de uso están separados en Commands (modifican estado) y Queries (solo leen):
 
```
UseCases/
├── CreateJobApplication/   → Command + Handler
├── GetJobApplication/      → Query + Handler
├── GetAllJobApplications/  → Query + Handler
├── UpdateJobApplication/   → Command + Handler
├── DeleteJobApplication/   → Command + Handler
├── RegisterUser/           → Command + Handler
├── LoginUser/              → Command + Handler
└── GetAnalyticsSummary/    → Query + Handler
```
 
---
 
## Endpoints disponibles
 
### Autenticación
| Método | Ruta | Descripción |
|--------|------|-------------|
| `POST` | `/api/auth/register` | Registrar nuevo usuario |
| `POST` | `/api/auth/login` | Iniciar sesión y obtener token JWT |
 
### Postulaciones (requieren token)
| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/jobapplications` | Listar todas las postulaciones del usuario |
| `GET` | `/api/jobapplications/{id}` | Obtener una postulación por ID |
| `POST` | `/api/jobapplications` | Crear una nueva postulación |
| `PUT` | `/api/jobapplications/{id}` | Actualizar una postulación |
| `DELETE` | `/api/jobapplications/{id}` | Eliminar una postulación |
 
### Análisis (requieren token)
| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/analytics/summary` | Resumen estadístico de la búsqueda |
 
### Estados posibles de una postulación
 
| Estado | Descripción |
|--------|-------------|
| `Applied` | Postulación enviada, esperando respuesta |
| `InProcess` | En proceso (entrevistas, pruebas técnicas) |
| `Rejected` | Rechazado |
| `Offer` | Oferta recibida |
| `Accepted` | Oferta aceptada |
| `Withdrawn` | Retirado del proceso por el candidato |
 
---
 
## Cómo correr el proyecto
 
### Con Docker (recomendado)
 
Requisitos: tener Docker Desktop instalado.
 
```bash
git clone https://github.com/Francisco-Galea/JobTracker.git
cd JobTracker
cp .env.example .env
```
 
Editá .env con tus valores y ejecutá:
 
```bash
docker-compose up --build
```
 
La API queda disponible en http://localhost:8080/swagger
 
Para detener:
```bash
docker-compose down
```
 
### Sin Docker
 
Requisitos: .NET 10 SDK y PostgreSQL.
 
1. Clonar el repositorio
```bash
git clone https://github.com/Francisco-Galea/JobTracker.git
cd JobTracker
```
 
2. Configurar la base de datos en JobTracker.API/appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=jobtracker;Username=postgres;Password=TU_PASSWORD"
  },
  "Jwt": {
    "Secret": "clave-secreta-minimo-32-caracteres",
    "Issuer": "JobTracker.API",
    "Audience": "JobTracker.Client",
    "ExpirationHours": "24"
  }
}
```
 
3. Aplicar las migrations
```bash
dotnet ef database update --project JobTracker.Infrastructure --startup-project JobTracker.API
```
 
4. Correr la API
```bash
dotnet run --project JobTracker.API
```
 
---
 
## Tests
 
```bash
dotnet test
```
 
Resultado esperado:
```
Passed! - Failed: 0, Passed: 15, Skipped: 0
```
 
---
 
## Ejemplo de uso
 
### Registrarse
```http
POST /api/auth/register
Content-Type: application/json
 
{
  "email": "usuario@gmail.com",
  "password": "Password123!",
  "fullName": "Nombre Apellido"
}
```
 
### Crear una postulación
```http
POST /api/jobapplications
Authorization: Bearer {token}
Content-Type: application/json
 
{
  "company": "Mercado Libre",
  "position": "Backend Developer .NET",
  "jobUrl": "https://jobs.mercadolibre.com/ejemplo",
  "notes": "Requieren experiencia en microservicios"
}
```
 
### Ver estadísticas
```http
GET /api/analytics/summary
Authorization: Bearer {token}
```
 
---
 
## Decisiones técnicas destacadas
 
**Setters privados en las entidades:** los campos solo se modifican a través de métodos con nombre. Imposible dejar la entidad en estado inválido desde código externo.
 
**Interfaz del repositorio en Domain:** permite que Infrastructure dependa del núcleo y no al revés. También hace los handlers testeables sin base de datos.
 
**Enum guardado como string:** EF Core persiste el estado como texto, haciendo los datos legibles en la DB y robustos ante cambios en el enum.
 
**UserId extraído del token:** el ID del usuario nunca viene del body del request. Se extrae del JWT validado por ASP.NET.
 
**Multi-stage Docker build:** la imagen final solo contiene el runtime (~200MB) sin el SDK (~800MB).
 
---
 
## Roadmap
 
- [x] CRUD de postulaciones con Clean Architecture
- [x] Autenticación con JWT
- [x] Módulo de análisis y estadísticas
- [x] Docker y Docker Compose
- [x] Tests unitarios
- [x] Deploy en Render
- [ ] Middleware global de excepciones
- [ ] FluentValidation
- [ ] Tests de integración