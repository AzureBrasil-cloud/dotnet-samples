# Books Web API - OpenAPI Export Guide

A modern ASP.NET 10 Web API that demonstrates how to **export OpenAPI (Swagger) documentation** from a development environment endpoint.

## Overview

This is a RESTful Web API built with:
- **Framework**: ASP.NET Core 10 (.NET 10)
- **Language**: C#
- **Documentation**: OpenAPI 3.1.1 (auto-generated)
- **Authentication**: API Key-based (x-api-key header)
- **Features**: In-memory book collection management

## Key Features

- Automatic OpenAPI Schema Generation - Exports to `/openapi/v1.json` endpoint
- Dynamic API documentation (no manual YAML/JSON maintenance)
- OpenAPI only available in Development environment
- Add books to an in-memory collection
- List all books or filter by name
- API Key authentication
- CORS enabled for cross-origin requests
- Comprehensive error handling and logging

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later
- Any REST client (Rider, VS Code REST Client, Postman, cURL, etc.)
- [jq](https://stedolan.github.io/jq/) (optional, for JSON formatting)

### Installation & Setup

1. **Clone or navigate to the project directory**
   ```bash
   cd webapi-openapi
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run --project OpenApi.WebApi
   ```

The API will start on `http://localhost:5115`

### Running in Development with Hot Reload

```bash
dotnet watch --project OpenApi.WebApi
```

---

## Exporting OpenAPI Schema (Main Focus)

The OpenAPI schema is **automatically generated** and available at the `/openapi/v1.json` endpoint in the **Development environment only**.

### Quick Export Methods

#### 1. Using cURL (Save to file)

```bash
curl http://localhost:5115/openapi/v1.json > openapi-schema.json
```

#### 2. Using cURL with jq (Pretty-print & save)

```bash
curl http://localhost:5115/openapi/v1.json | jq > openapi-schema.json
```

#### 3. Using PowerShell

```powershell
Invoke-WebRequest -Uri "http://localhost:5115/openapi/v1.json" -OutFile "openapi-schema.json"
```

#### 4. Using wget

```bash
wget http://localhost:5115/openapi/v1.json -O openapi-schema.json
```

### View Schema in Terminal (Without Saving)

**Pretty-printed JSON:**
```bash
curl http://localhost:5115/openapi/v1.json | jq .
```

**View API paths section:**
```bash
curl http://localhost:5115/openapi/v1.json | jq '.paths'
```

**View data models (schemas):**
```bash
curl http://localhost:5115/openapi/v1.json | jq '.components.schemas'
```

**View security schemes:**
```bash
curl http://localhost:5115/openapi/v1.json | jq '.components.securitySchemes'
```

### Using REST Client in Rider/VS Code

The project includes `OpenApi.WebApi.http` file. Open it and use the request:

```http
### Get OpenAPI Schema
GET http://localhost:5115/openapi/v1.json
Accept: application/json
```

Click **"Send Request"** and view the full schema in the response panel.

---

## Understanding the Auto-Generated OpenAPI Schema

The OpenAPI schema is **automatically generated** from your ASP.NET Core code:

### How It Works

1. **Endpoint Metadata** - Attributes like `[EndpointDescription]`, `[EndpointName]`, `[ProducesResponseType]`
2. **Model Classes** - Your C# models are automatically converted to JSON Schema
3. **Route Attributes** - `[HttpPost]`, `[HttpGet]` annotations define API paths
4. **Middleware Configuration** - OpenAPI service setup in `OpenApiServiceExtensions.cs`

**No manual YAML or JSON files needed!**

### Configuration

The OpenAPI generation is configured in `OpenApi.WebApi/Extensions/OpenApiServiceExtensions.cs`:

```csharp
public static IHostApplicationBuilder AddCustomOpenApiWithApiKey(
    this IHostApplicationBuilder builder,
    string apiKeyHeaderName = "x-api-key")
{
    if (builder.Environment.IsProduction())
        return builder;  // Only available in Development!

    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            // Automatically add security scheme for API key
            document.Components.SecuritySchemes["ApiKey"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = apiKeyHeaderName,
                In = ParameterLocation.Header
            };
            
            document.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("ApiKey")] = []
            });

            return Task.CompletedTask;
        });
    });
    return builder;
}
```

**Key Points:**
- Only available in **Development** environment
- Automatically updated when you modify endpoints
- Includes API Key security scheme documentation
- Real-time reflection of all endpoints and models
- No production performance impact

---

## Complete OpenAPI Schema Export Example

### Run the Application

```bash
dotnet run --project OpenApi.WebApi
```

### Export the Schema

```bash
curl http://localhost:5115/openapi/v1.json | jq > books-api.json
```

### Generated Schema Output

Here's the complete auto-generated OpenAPI schema for the Books API:

```json
{
  "openapi": "3.1.1",
  "info": {
    "title": "OpenApi.WebApi | v1",
    "version": "1.0.0"
  },
  "servers": [
    {
      "url": "http://localhost:5115/"
    }
  ],
  "paths": {
    "/api/books/add": {
      "post": {
        "tags": ["Books"],
        "description": "Add a new book to the list",
        "operationId": "AddBook",
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Book"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/Book"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/Book"
              }
            }
          },
          "required": true
        },
        "responses": {
          "201": {
            "description": "Created",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Book"
                }
              }
            }
          }
        }
      }
    },
    "/api/books/list": {
      "get": {
        "tags": ["Books"],
        "description": "Get all books, optionally filtered by name",
        "operationId": "GetBooks",
        "parameters": [
          {
            "name": "name",
            "in": "query",
            "description": "Optional name filter. If provided, only books containing this string in their name will be returned.",
            "schema": {
              "type": "string"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/Book"
                  }
                }
              }
            }
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "Book": {
        "required": ["name", "pages"],
        "type": "object",
        "properties": {
          "name": {
            "type": "string",
            "description": "The name of the book"
          },
          "pages": {
            "maximum": 2147483647,
            "minimum": 1,
            "type": "integer",
            "format": "int32",
            "description": "Number of pages in the book"
          }
        }
      }
    },
    "securitySchemes": {
      "ApiKey": {
        "type": "apiKey",
        "name": "x-api-key",
        "in": "header",
        "description": "API Key authentication required for most endpoints"
      }
    }
  },
  "security": [
    {
      "ApiKey": []
    }
  ],
  "tags": [
    {
      "name": "Books",
      "description": "Book management endpoints"
    }
  ]
}
```

---

## Authentication

All API requests (except `/openapi/v1.json`) require the `x-api-key: 1234` header. Configure the key in `appsettings.json`.

---

## API Endpoints

### 1. Get OpenAPI Schema

**No authentication required.**

```bash
curl http://localhost:5115/openapi/v1.json | jq
```

---

### 2. Add a Book

```bash
curl -X POST http://localhost:5115/api/books/add \
  -H "Content-Type: application/json" \
  -H "x-api-key: 1234" \
  -d '{"name":"The Great Gatsby","pages":180}'
```

Response: `201 Created`

---

### 3. List Books

```bash
# Get all books
curl http://localhost:5115/api/books/list -H "x-api-key: 1234" | jq

# Filter by name (case-insensitive contains)
curl "http://localhost:5115/api/books/list?name=Gatsby" -H "x-api-key: 1234" | jq
```

Response: `200 OK` with array of books

---

## Using the REST Client File

Open `OpenApi.WebApi.http` in Rider or VS Code REST Client and click "Send Request" on any endpoint to test the API.

---

## Project Structure

```
OpenApi.WebApi/
├── Controllers/
│   ├── BooksController.cs          # API endpoints for book operations
│   └── Models/
│       └── Book.cs                 # Book model with validation
├── Services/
│   └── BookService.cs              # In-memory book collection management
├── Extensions/
│   ├── ApplicationBuilderExtensions.cs   # Middleware configuration
│   ├── ApiKeyMiddlewareExtensions.cs     # API key validation
│   ├── ServiceCollectionExtensions.cs    # Dependency injection setup
│   ├── OpenApiServiceExtensions.cs       # OpenAPI configuration
│   └── OpenApiMappingExtensions.cs       # OpenAPI route mapping
├── Program.cs                      # Application startup
├── appsettings.json                # Configuration (API Key)
├── launchSettings.json             # Launch profiles
└── OpenApi.WebApi.http             # REST client test file
```

---

## Configuration

The API key is configured in `appsettings.json` (default: `1234`):

```json
{
  "ApiKey": "1234"
}
```

To change the API key, edit `appsettings.json` and update the `x-api-key` header in requests accordingly.

---

## Development

```bash
dotnet build                    # Build
dotnet test                     # Run tests
dotnet publish -c Release       # Publish for production
```

---

## Security Notes

Important for Production:
- Use a strong API key (current "1234" is for development only)
- Enable HTTPS
- Restrict CORS policy (currently allows all origins)
- Use a database for persistence (data is lost on restart)
- Implement rate limiting

---

## Troubleshooting

**"Unauthorized" errors**: Include the `x-api-key: 1234` header in requests

**Port already in use**: Change the port in `launchSettings.json`

---

## Dependencies

- **Microsoft.AspNetCore.OpenApi**: OpenAPI documentation support
- **Microsoft.AspNetCore.Cors**: CORS middleware
- **.NET 10 Runtime**: Latest .NET runtime