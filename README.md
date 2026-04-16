# JobTracker API

API REST para gestionar búsquedas de empleo. Permite registrar postulaciones, hacer seguimiento de su estado y obtener estadísticas sobre el proceso de búsqueda.

> Proyecto construido como portfolio profesional, aplicando Clean Architecture, CQRS y buenas prácticas de desarrollo backend en .NET.

---

## ¿Por qué existe este proyecto?

Buscar trabajo es un proceso que puede volverse caótico rápidamente: postulaciones sin respuesta, entrevistas en distintas etapas, una cantidad en incremento de medios el cual buscar trabajo (Linkedin, Email, Google Forms, etc),empresas que te contactan semanas después. Este proyecto nació de esa necesidad real: tener un sistema que centralice y mida el proceso.

---

## Stack tecnológico

- **Lenguaje:** C# / .NET 10
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Base de datos:** PostgreSQL
- **Documentación:** Swagger / OpenAPI

---

## Arquitectura

El proyecto sigue los principios de **Clean Architecture**, separando responsabilidades en cuatro capas independientes:

```
JobTracker/
├── JobTracker.Domain/          → Entidades, interfaces, enums. Sin dependencias externas.
├── JobTracker.Application/     → Casos de uso (CQRS), DTOs, excepciones.
├── JobTracker.Infrastructure/  → EF Core, repositorios, configuración de PostgreSQL.
└── JobTracker.API/             → Controllers, Swagger, Program.cs.
```

Las dependencias siempre apuntan hacia el Domain. Infrastructure y API dependen de las capas internas, nunca al revés.

### Patrón CQRS

Los casos de uso están separados en **Commands** (modifican estado) y **Queries** (solo leen), siguiendo el principio de responsabilidad única:

```
UseCases/
├── CreateJobApplication/   → Command + Handler
├── GetJobApplication/      → Query + Handler
├── GetAllJobApplications/  → Query + Handler
├── UpdateJobApplication/   → Command + Handler
└── DeleteJobApplication/   → Command + Handler
```

---

## Endpoints disponibles

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/jobapplications` | Listar todas las postulaciones |
| `GET` | `/api/jobapplications/{id}` | Obtener una postulación por ID |
| `POST` | `/api/jobapplications` | Crear una nueva postulación |
| `PUT` | `/api/jobapplications/{id}` | Actualizar una postulación |
| `DELETE` | `/api/jobapplications/{id}` | Eliminar una postulación |

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

## Cómo correr el proyecto localmente

### Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

### Pasos

**1. Clonar el repositorio**

```bash
git clone https://github.com/Francisco-Galea/JobTracker.git
cd JobTracker
```

**2. Configurar la base de datos**

Creá una base de datos en PostgreSQL llamada `jobtracker` y actualizá la connection string en `JobTracker.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=jobtracker;Username=postgres;Password=TU_PASSWORD"
  }
}
```

**3. Aplicar las migrations**

```bash
dotnet ef database update --project JobTracker.Infrastructure --startup-project JobTracker.API
```

**4. Correr la API**

```bash
dotnet run --project JobTracker.API
```

**5. Abrir Swagger**

```
https://localhost:{puerto}/swagger
```

---

## Ejemplo de uso

### Crear una postulación

```http
POST /api/jobapplications
Content-Type: application/json

{
  "company": "Mercado Libre",
  "position": "Backend Developer .NET",
  "jobUrl": "https://jobs.mercadolibre.com/ejemplo",
  "notes": "Requieren experiencia en microservicios"
}
```

### Respuesta

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "company": "Mercado Libre",
  "position": "Backend Developer .NET",
  "jobUrl": "https://jobs.mercadolibre.com/ejemplo",
  "notes": "Requieren experiencia en microservicios",
  "status": 0,
  "statusDisplay": "Applied",
  "appliedAt": "2026-04-07T00:00:00Z",
  "lastUpdatedAt": null
}
```

---

## Decisiones técnicas destacadas

**Setters privados en las entidades:** Los campos de `JobApplication` solo se modifican a través de métodos con nombre (`ChangeStatus`, `Update`). Esto hace imposible dejar la entidad en un estado inválido desde código externo.

**Factory method `Create`:** En lugar de un constructor público, la entidad se crea a través de un método estático que valida los datos antes de instanciar el objeto.

**Enum guardado como string:** EF Core está configurado para persistir el estado como texto (`"Applied"`, `"Rejected"`) en lugar de número entero, haciendo los datos legibles directamente en la base de datos y robustos ante cambios de orden en el enum.

**Interfaz del repositorio en Domain:** `IJobApplicationRepository` vive en la capa Domain para que Infrastructure dependa del núcleo, y no al revés. Esto permite reemplazar la implementación de acceso a datos sin tocar la lógica de negocio.

---

## Roadmap

- [x] CRUD de postulaciones con Clean Architecture
- [x] Autenticación con JWT
- [x] Módulo de análisis y estadísticas
- [x] Deploy en Render
- [x] Tests unitarios




