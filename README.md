Udemy Benzeri Basit API (ASP.NET Core 8 + SQL Server)

Özet
- .NET 8 Minimal API, EF Core (SQL Server)
- Modeller: User, Course, Lesson, Category, Enrollment
- Özellikler: Kayıt/Giriş (JWT), Kurs listeleme/ekleme, Ders ekleme, Kayıt (enrollment)

Kurulum
1) .NET 8 SDK kurulu olmalı.
2) SQL Server veya LocalDB kullanılabilir. Varsayılan connection string: `(localdb)\\MSSQLLocalDB`.
3) `appsettings.json` içindeki `ConnectionStrings:DefaultConnection` ve `Jwt:Key` değerlerini güncelleyin.

Çalıştırma
- Komut: `dotnet run --project src/UdemyClone.Api/UdemyClone.Api.csproj`
- Swagger: `http://localhost:5255/swagger`

Başlangıç Verisi
- Uygulama ilk çalıştığında 3 kategori ekler: Development, Business, Design
- Veritabanı hızlı başlangıç için `EnsureCreated()` ile oluşur (migration yok).

Temel Uçlar
- POST `/auth/register` { email, password, role? }
- POST `/auth/login` { email, password } -> { token }
- GET  `/courses` (opsiyonel `categoryId`, `q`)
- GET  `/courses/{id}`
- POST `/courses` { title, description, price, instructorId, categoryId, published }
- POST `/courses/{id}/lessons` { title, contentUrl?, durationSeconds }
- POST `/enrollments` { userId, courseId }
- GET  `/users/{userId}/courses`
- GET  `/instructors/{instructorId}/courses`

Notlar
- Basitlik için yetkilendirme zorunlu değil. İsterseniz endpointlere `[Authorize]` eklenebilir ve rol bazlı koruma yapılabilir.
- Prod için EF Core Migrations ve gerçek dosya/video yönetimi eklenmelidir.

Proje Yolu
- API proje dosyası: `src/UdemyClone.Api/UdemyClone.Api.csproj`

