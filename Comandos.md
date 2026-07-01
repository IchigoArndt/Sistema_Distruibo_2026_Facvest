# Comandos — Execução Local

## Docker (stack completa)

**Requisito:** Docker com pelo menos **4 GB de RAM** (SQL Server usa ~2 GB).

```bash
docker compose up -d --build
```

```powershell
docker compose up -d --build
```

| Serviço | URL |
|---------|-----|
| Portal | http://localhost:4200 |
| Auth | http://localhost:5159/scalar |
| Server | http://localhost:5252/scalar |

```bash
docker compose logs -f
docker compose down
docker compose down -v   # remove volumes (reset do banco)
```

---

## Execução manual (sem Docker)

**Ordem:** Bancos → Server → Auth → Client (cada serviço em um terminal separado).

| Serviço | URL |
|---------|-----|
| Auth | http://localhost:5159 |
| Server | http://localhost:5252 |
| Client | http://localhost:4200 |

---

## Bancos de Dados (Docker)

### Linux / macOS (Bash)

**SQL Server**

```bash
docker run -d \
  --name sd-sqlserver \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=p4ssw0rd!" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

**MongoDB**

```bash
docker run -d \
  --name sd-mongodb \
  -p 27017:27017 \
  mongo:7
```

**Parar e remover**

```bash
docker stop sd-sqlserver sd-mongodb
docker rm sd-sqlserver sd-mongodb
```

---

### Windows (PowerShell)

**SQL Server**

```powershell
docker run -d `
  --name sd-sqlserver `
  -e "ACCEPT_EULA=Y" `
  -e "MSSQL_SA_PASSWORD=p4ssw0rd!" `
  -p 1433:1433 `
  mcr.microsoft.com/mssql/server:2022-latest
```

**MongoDB**

```powershell
docker run -d `
  --name sd-mongodb `
  -p 27017:27017 `
  mongo:7
```

**Parar e remover**

```powershell
docker stop sd-sqlserver sd-mongodb
docker rm sd-sqlserver sd-mongodb
```

---

### Windows (CMD)

**SQL Server**

```cmd
docker run -d --name sd-sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=p4ssw0rd!" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
```

**MongoDB**

```cmd
docker run -d --name sd-mongodb -p 27017:27017 mongo:7
```

**Parar e remover**

```cmd
docker stop sd-sqlserver sd-mongodb
docker rm sd-sqlserver sd-mongodb
```

---

## Execução do Projeto

### Linux / macOS (Bash)

**1. Server** (aplica migrations automaticamente)

```bash
cd Server/SD_Server
dotnet restore
dotnet run --launch-profile http
```

**2. Auth**

```bash
cd Server/SD_Server.Auth
dotnet restore
dotnet run --launch-profile http
```

**3. Client**

```bash
cd Client/portal
npm install
npm start
```

---

### Windows (PowerShell)

**1. Server** (aplica migrations automaticamente)

```powershell
cd Server\SD_Server
dotnet restore
dotnet run --launch-profile http
```

**2. Auth**

```powershell
cd Server\SD_Server.Auth
dotnet restore
dotnet run --launch-profile http
```

**3. Client**

```powershell
cd Client\portal
npm install
npm start
```

---

### Windows (CMD)

**1. Server** (aplica migrations automaticamente)

```cmd
cd Server\SD_Server
dotnet restore
dotnet run --launch-profile http
```

**2. Auth**

```cmd
cd Server\SD_Server.Auth
dotnet restore
dotnet run --launch-profile http
```

**3. Client**

```cmd
cd Client\portal
npm install
npm start
```
