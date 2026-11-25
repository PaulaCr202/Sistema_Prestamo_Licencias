# API Gateway — Sistema de Préstamo de Licencias

Punto de entrada único del sistema. La UI solo conoce el gateway (puerto 5000), y el gateway reenvía cada petición al microservicio que le corresponde. Está hecho con YARP (Yarp.ReverseProxy), igual que el gateway de Inmoby.

El gateway **no valida el token**: lo reenvía tal cual y cada microservicio valida el JWT por su cuenta.

## Puertos

| Servicio | Puerto | Ruta en el gateway | Se reenvía a |
|---|---|---|---|
| API Gateway | 5000 | — | — |
| Identity | 5086 | `/identity/...` | `http://localhost:5086/api/...` |
| Catalog | 5080 | `/catalog/...` | `http://localhost:5080/api/...` |
| Loans | 5090 | `/loans/...` | `http://localhost:5090/api/...` |
| Vendors | 5100 | `/vendors/...` | `http://localhost:5100/api/...` |
| UI (Vite) | 5173 | — | — |

Ejemplo: la UI llama a `http://localhost:5000/loans/loans/mine`; el gateway quita `/loans` y Loans recibe `GET /api/loans/mine`.

## CORS

El navegador solo deja que la UI llame al gateway si el gateway lo autoriza. Los orígenes permitidos están en `ApiGateway.App/appsettings.json`, sección `Cors:AllowedOrigins`: `localhost:5173` (Vite) y `localhost:5500` (Live Server), cada uno también con `127.0.0.1`. Si la UI se abre desde otro puerto, hay que agregarlo ahí.

## JWT

Los cuatro microservicios usan la misma configuración de JWT (sección `JWT` de cada `appsettings.json`), así un token emitido por Identity sirve en todos:

- Issuer: `Licenses.Identity`
- Audience: `Licenses.Microservices`
- La misma `SignKey`

## Orden de arranque

Cada servicio en su propia terminal, desde la raíz del repositorio y en este orden:

```bash
dotnet run --project Identity/Identity.App --launch-profile http
dotnet run --project Catalog/Catalog.Api --launch-profile http
dotnet run --project Loans/Loans.Api --launch-profile http
dotnet run --project Vendors/Vendors.Api --launch-profile http
dotnet run --project ApiGateway/ApiGateway.App --launch-profile http
```

Vendors es opcional para el flujo de préstamos.

Antes del primer arranque hay que crear las bases de datos (ver el README de cada servicio). Si tu SQL Server es distinto al de las cadenas de conexión del repositorio, no cambies los `appsettings.json`: define la conexión como variable de entorno en cada terminal antes del `dotnet run`. Ejemplo con LocalDB en PowerShell:

```powershell
$env:ConnectionStrings__CatalogDb="Server=(localdb)\MSSQLLocalDB;Database=CatalogDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

(igual con `IdentityDb`, `LoansDb` y `VendorsDb`).

Por último, la UI, desde la carpeta `UI`:

```bash
npx vite
```

y abrir `http://localhost:5173`.

## Probar el flujo completo

El archivo `ApiGateway.App/ApiGateway.App.http` recorre el flujo pasando solo por el gateway: login, ver licencias, pedir un préstamo, ver la licencia ocupada en Catalog, devolver y verla libre otra vez. Se ejecuta con la extensión REST Client de VS Code, de arriba hacia abajo.

Si un servicio está apagado, el gateway responde **502 Bad Gateway** a las rutas de ese servicio.