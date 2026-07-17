# DMAS_EXAM
# BATTLEGAME - Developing Microsoft Azure Solutions - Set01 Practical Exam

## 1. Cấu trúc project
DMAS_EXAM/
├── Controllers/
│   ├── PlayerController.cs
│   ├── AssetController.cs
│   └── PlayerAssetController.cs
├── DTOs/
├── Models/
├── Data/
├── Migrations/
├── wwwroot/client/
│   ├── index.html
│   ├── styles.css
│   └── app.js
├── Program.cs
├── appsettings.json
└── README.md
text## 2. Yêu cầu cài đặt

- **.NET 8 SDK**
- **MySQL Server** (local hoặc XAMPP / Docker)
- **JetBrains Rider** (hoặc Visual Studio / VS Code)

---

## 3. Hướng dẫn cài đặt & chạy project

### Bước 1: Cấu hình Connection String

Mở file `appsettings.json` và chỉnh lại connection string cho phù hợp với MySQL của bạn:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=battlegame;user=root;password="
  }
}
```
Bước 2: Restore packages
```Bash
dotnet restore
```
Bước 3: Tạo database (Migration)
```Bash
dotnet ef database update
```

Bước 4: Chạy project
Chạy project (https/http). Project sẽ chạy tại:
```
https://localhost:7285 hoặc http://localhost:5291
```
Bước 5: Mở Client
Truy cập:
```
http://localhost:5291/client/index.html
```
