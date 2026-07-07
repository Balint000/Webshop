**WebShop**

Projekt Specifikáció

v1.0 · Magas szintű specifikáció · Portfolió projekt

| **Frontend**<br><br>Vue 3 + TypeScript + Pinia | **Admin app**<br><br>Avalonia UI + .NET + MVVM                      |
| ---------------------------------------------- | ------------------------------------------------------------------- |
| **Backend API**<br><br>ASP.NET Core Web API    | **Adatbázis & Infrastruktúra**<br><br>PostgreSQL + Redis + Hangfire |

# Tartalomjegyzék

Tartalom

[Tartalomjegyzék 2](#_Toc233190122)

[1\. Projekt áttekintő 3](#_Toc233190123)

[1.1 Célkitűzések 3](#_Toc233190124)

[1.2 Technológiai stack 3](#_Toc233190125)

[2\. Szerepkörök és jogosultságok 4](#_Toc233190126)

[2.1 Szerepkörök 4](#_Toc233190127)

[Guest (vendég) 4](#_Toc233190128)

[Customer (vásárló) 4](#_Toc233190129)

[Admin 4](#_Toc233190130)

[2.2 Jogosultság mátrix 4](#_Toc233190131)

[3\. Adatmodell 5](#_Toc233190132)

[3.1 User 5](#_Toc233190133)

[3.2 Category 5](#_Toc233190134)

[3.3 Product 5](#_Toc233190135)

[3.4 Cart / CartItem 6](#_Toc233190136)

[3.5 Order / OrderItem 6](#_Toc233190137)

[3.6 Invoice 6](#_Toc233190138)

[3.7 AuditLog 6](#_Toc233190139)

[4\. Backend API végpontok 8](#_Toc233190140)

[4.1 Autentikáció 8](#_Toc233190141)

[4.2 Termékek 8](#_Toc233190142)

[4.3 Kategóriák 8](#_Toc233190143)

[4.4 Kosár 8](#_Toc233190144)

[4.5 Rendelések 9](#_Toc233190145)

[4.6 Számlák 9](#_Toc233190146)

[4.7 Felhasználók (admin) 9](#_Toc233190147)

[4.8 Riportok és audit log (admin) 9](#_Toc233190148)

[5\. Webshop frontend (Vue 3) 10](#_Toc233190149)

[5.1 Oldalak és képernyők 10](#_Toc233190150)

[5.2 Pinia store-ok 10](#_Toc233190151)

[5.3 Route guard-ok 10](#_Toc233190152)

[6\. Admin desktop alkalmazás (Avalonia UI) 11](#_Toc233190153)

[6.1 Projekt struktúra 11](#_Toc233190154)

[6.2 Képernyők 11](#_Toc233190155)

[6.3 MVVM felépítés 11](#_Toc233190156)

[7\. Számlázás (Számlázz.hu integráció) 12](#_Toc233190157)

[7.1 Folyamat 12](#_Toc233190158)

[7.2 Implementációs részletek 12](#_Toc233190159)

[8\. Solution struktúra 13](#_Toc233190160)

[9\. Fejlesztési ütemterv 14](#_Toc233190161)

[10\. Fejlesztési ticketek 15](#_Toc233190162)

[Infrastruktúra (INFRA) 15](#_Toc233190163)

[Adatbázis (DB) 15](#_Toc233190164)

[Autentikáció (AUTH) 15](#_Toc233190165)

[API végpontok (API) 15](#_Toc233190166)

[Webshop frontend (FE) 15](#_Toc233190167)

[Admin desktop app (ADMIN) 16](#_Toc233190168)

[Számlázás (INV) 16](#_Toc233190169)

[Minőség (TEST) 16](#_Toc233190170)

[11\. Jövőbeli fejlesztések 17](#_Toc233190171)

# 1\. Projekt áttekintő

A WebShop egy portfolió szintű e-kereskedelmi alkalmazás, amely modern .NET és JavaScript technológiákat demonstrál. A rendszer három különálló kliensből áll: egy böngésző alapú webshopból (Vue 3), egy különálló asztali adminisztrációs alkalmazásból (Avalonia UI), és egy megosztott ASP.NET Core REST API-ból.

## 1.1 Célkitűzések

- Valósághű webshop funkcionalitás (böngészés, kosár, rendelés, számla)
- Különálló admin desktop alkalmazás (Avalonia UI, MVVM pattern)
- Biztonságos JWT alapú autentikáció role-alapú hozzáféréssel
- Aszinkron számlázás Hangfire background job-okon keresztül
- Számlázz.hu integráció automatikus PDF számlákhoz
- Portfolió és technikai interjú szempontjából bemutatható kódbázis

## 1.2 Technológiai stack

| **Réteg**             | **Technológia**                                | **Megjegyzés**                          |
| --------------------- | ---------------------------------------------- | --------------------------------------- |
| **Webshop frontend**  | Vue 3 + TypeScript + Pinia + Vue Router        | SPA, Composition API, Vite build        |
| **Admin desktop app** | Avalonia UI + .NET 8 + MVVM (CommunityToolkit) | Cross-platform, Windows/macOS/Linux     |
| **Backend API**       | ASP.NET Core 8 Web API                         | REST, Minimal API vagy Controller alapú |
| **ORM**               | Entity Framework Core 8                        | Code-First, Migrations                  |
| **Adatbázis**         | PostgreSQL 16                                  | Npgsql provider                         |
| **Cache**             | Redis (StackExchange.Redis)                    | Session cache, query cache              |
| **Background jobs**   | Hangfire                                       | PostgreSQL storage, Dashboard UI        |
| **Auth**              | JWT Bearer + Refresh token                     | ASP.NET Core Identity nélkül            |
| **Validáció**         | FluentValidation                               | API és DTO szintű validáció             |
| **Számlázás**         | Számlázz.hu REST API                           | Aszinkron job-on keresztül              |
| **Fizetés**           | Placeholder (TBD)                              | Stripe vagy SimplePay, később           |

# 2\. Szerepkörök és jogosultságok

## 2.1 Szerepkörök

### Guest (vendég)

Nem autentikált látogató. Böngészhet és kereshet termékek között, megtekintheti a kategóriákat, regisztrálhat és bejelentkezhet. Kosár és rendelés funkciókhoz nincs hozzáférése.

### Customer (vásárló)

Regisztrált és bejelentkezett felhasználó. Hozzáfér a kosár, rendelés, számla és profil funkciókhoz. A szerepkör regisztrációkor automatikusan kiosztásra kerül.

### Admin

Kizárólag az Avalonia desktop alkalmazáson keresztül tud bejelentkezni. Teljes hozzáféréssel rendelkezik a termék-, kategória-, rendelés- és felhasználókezeléshez, valamint a riportokhoz és az audit log megtekintéséhez. Admin role-t manuálisan vagy seed adattal kap.

## 2.2 Jogosultság mátrix

| **Jogosultság**               | **Guest** | **Customer** | **Admin** |
| ----------------------------- | --------- | ------------ | --------- |
| Termékek böngészése           | **✓**     | **✓**        | **✓**     |
| Keresés                       | **✓**     | **✓**        | **✓**     |
| Regisztráció / Bejelentkezés  | **✓**     | **✓**        | **✓**     |
| Kosár kezelése                | **✗**     | **✓**        | **✓**     |
| Megrendelés leadása           | **✗**     | **✓**        | **✓**     |
| Saját rendelések megtekintése | **✗**     | **✓**        | **✓**     |
| Számla letöltése              | **✗**     | **✓**        | **✓**     |
| Profil kezelése               | **✗**     | **✓**        | **✓**     |
| Termékek kezelése (admin)     | **✗**     | **✗**        | **✓**     |
| Kategóriák kezelése (admin)   | **✗**     | **✗**        | **✓**     |
| Rendelések kezelése (admin)   | **✗**     | **✗**        | **✓**     |
| Felhasználók kezelése (admin) | **✗**     | **✗**        | **✓**     |
| Riportok (admin)              | **✗**     | **✗**        | **✓**     |
| Audit log (admin)             | **✗**     | **✗**        | **✓**     |

# 3\. Adatmodell

Az adatbázis PostgreSQL, Entity Framework Core Code-First megközelítéssel. Az entitások Guid alapú primary key-eket használnak.

## 3.1 User

| **Mező**     | **Típus** | **Leírás**                       |
| ------------ | --------- | -------------------------------- |
| Id           | Guid PK   | Egyedi azonosító                 |
| Email        | string    | Egyedi, bejelentkezési azonosító |
| PasswordHash | string    | BCrypt hash                      |
| Role         | enum      | Guest \| Customer \| Admin       |
| FirstName    | string    | Keresztnév                       |
| LastName     | string    | Vezetéknév                       |
| CreatedAt    | DateTime  | Regisztráció időpontja           |
| IsActive     | bool      | Fiók aktív-e (soft delete)       |

## 3.2 Category

| **Mező** | **Típus** | **Leírás**                |
| -------- | --------- | ------------------------- |
| Id       | Guid PK   | Egyedi azonosító          |
| Name     | string    | Kategória neve (egyedi)   |
| Slug     | string    | URL-barát azonosító       |
| ParentId | Guid? FK  | Alkategóriához (nullable) |
| IsActive | bool      | Aktív-e                   |

## 3.3 Product

| **Mező**    | **Típus** | **Leírás**           |
| ----------- | --------- | -------------------- |
| Id          | Guid PK   | Egyedi azonosító     |
| CategoryId  | Guid FK   | Kategória hivatkozás |
| Name        | string    | Termék neve          |
| Description | string    | Leírás               |
| Price       | decimal   | Nettó ár (HUF)       |
| Stock       | int       | Raktárkészlet        |
| ImageUrl    | string?   | Kép URL              |
| IsActive    | bool      | Eladható-e           |
| CreatedAt   | DateTime  | Létrehozás ideje     |

## 3.4 Cart / CartItem

| **Mező**           | **Típus** | **Leírás**                  |
| ------------------ | --------- | --------------------------- |
| Cart.Id            | Guid PK   | Kosár azonosítója           |
| Cart.UserId        | Guid FK   | Felhasználóhoz kötött (1:1) |
| CartItem.CartId    | Guid FK   | Kosár hivatkozás            |
| CartItem.ProductId | Guid FK   | Termék hivatkozás           |
| CartItem.Quantity  | int       | Mennyiség (min: 1)          |

## 3.5 Order / OrderItem

| **Mező**              | **Típus** | **Leírás**                                                 |
| --------------------- | --------- | ---------------------------------------------------------- |
| Order.Id              | Guid PK   | Rendelés azonosítója                                       |
| Order.UserId          | Guid FK   | Felhasználó hivatkozás                                     |
| Order.Status          | enum      | Pending \| Processing \| Shipped \| Completed \| Cancelled |
| Order.TotalAmount     | decimal   | Végösszeg (HUF)                                            |
| Order.ShippingAddress | string    | Szállítási cím (JSON)                                      |
| Order.CreatedAt       | DateTime  | Rendelés ideje                                             |
| OrderItem.OrderId     | Guid FK   | Rendelés hivatkozás                                        |
| OrderItem.ProductId   | Guid FK   | Termék hivatkozás                                          |
| OrderItem.Quantity    | int       | Rendelt mennyiség                                          |
| OrderItem.UnitPrice   | decimal   | Egységár a rendeléskor                                     |

## 3.6 Invoice

| **Mező**      | **Típus** | **Leírás**                  |
| ------------- | --------- | --------------------------- |
| Id            | Guid PK   | Számla azonosítója          |
| OrderId       | Guid FK   | Rendelés hivatkozás (1:1)   |
| InvoiceNumber | string    | Számlázz.hu sorszám         |
| PdfUrl        | string    | PDF letöltési URL           |
| IssuedAt      | DateTime  | Kiállítás időpontja         |
| Status        | enum      | Pending \| Issued \| Failed |

## 3.7 AuditLog

| **Mező**   | **Típus** | **Leírás**                      |
| ---------- | --------- | ------------------------------- |
| Id         | Guid PK   | Esemény azonosítója             |
| UserId     | Guid FK   | Ki végezte a műveletet          |
| Action     | string    | Create \| Update \| Delete stb. |
| EntityType | string    | Érintett entitás neve           |
| EntityId   | string    | Érintett entitás ID-ja          |
| OldValues  | string?   | JSON - korábbi érték            |
| NewValues  | string?   | JSON - új érték                 |
| Timestamp  | DateTime  | Esemény időpontja               |

# 4\. Backend API végpontok

Az API ASP.NET Core 8 Web API, JWT Bearer autentikációval. Minden válasz application/json formátumban érkezik. Hibák RFC 7807 (ProblemDetails) formátumban.

_Az admin végpontok kizárólag Admin role-lal rendelkező felhasználók számára elérhetők, és jellemzően az Avalonia desktop alkalmazás hívja őket._

## 4.1 Autentikáció

| **Metódus** | **Útvonal**        | **Szerepkör** | **Leírás**                              |
| ----------- | ------------------ | ------------- | --------------------------------------- |
| **POST**    | /api/auth/register | Publikus      | Regisztráció, Customer role automatikus |
| **POST**    | /api/auth/login    | Publikus      | JWT + Refresh token visszaadása         |
| **POST**    | /api/auth/refresh  | Publikus      | Új access token refresh token alapján   |
| **POST**    | /api/auth/logout   | Customer+     | Refresh token invalidálása              |

## 4.2 Termékek

| **Metódus** | **Útvonal**        | **Szerepkör** | **Leírás**                         |
| ----------- | ------------------ | ------------- | ---------------------------------- |
| **GET**     | /api/products      | Publikus      | Listázás, lapozás, szűrés, keresés |
| **GET**     | /api/products/{id} | Publikus      | Egy termék részletei               |
| **POST**    | /api/products      | Admin         | Új termék létrehozása              |
| **PUT**     | /api/products/{id} | Admin         | Termék szerkesztése                |
| **DELETE**  | /api/products/{id} | Admin         | Soft delete                        |

## 4.3 Kategóriák

| **Metódus** | **Útvonal**          | **Szerepkör** | **Leírás**               |
| ----------- | -------------------- | ------------- | ------------------------ |
| **GET**     | /api/categories      | Publikus      | Kategória fa lekérdezése |
| **POST**    | /api/categories      | Admin         | Új kategória             |
| **PUT**     | /api/categories/{id} | Admin         | Kategória szerkesztése   |
| **DELETE**  | /api/categories/{id} | Admin         | Soft delete              |

## 4.4 Kosár

| **Metódus** | **Útvonal**                 | **Szerepkör** | **Leírás**              |
| ----------- | --------------------------- | ------------- | ----------------------- |
| **GET**     | /api/cart                   | Customer+     | Aktuális kosár tartalma |
| **POST**    | /api/cart/items             | Customer+     | Termék hozzáadása       |
| **PUT**     | /api/cart/items/{productId} | Customer+     | Mennyiség módosítása    |
| **DELETE**  | /api/cart/items/{productId} | Customer+     | Tétel eltávolítása      |
| **DELETE**  | /api/cart                   | Customer+     | Kosár ürítése           |

## 4.5 Rendelések

| **Metódus** | **Útvonal**                   | **Szerepkör** | **Leírás**                  |
| ----------- | ----------------------------- | ------------- | --------------------------- |
| **POST**    | /api/orders                   | Customer+     | Rendelés leadása (checkout) |
| **GET**     | /api/orders                   | Customer+     | Saját rendelések listája    |
| **GET**     | /api/orders/{id}              | Customer+     | Egy rendelés részletei      |
| **GET**     | /api/admin/orders             | Admin         | Összes rendelés (admin)     |
| **PUT**     | /api/admin/orders/{id}/status | Admin         | Rendelés státusz módosítása |

## 4.6 Számlák

| **Metódus** | **Útvonal**         | **Szerepkör** | **Leírás**                 |
| ----------- | ------------------- | ------------- | -------------------------- |
| **GET**     | /api/invoices/{id}  | Customer+     | Saját számla PDF letöltése |
| **GET**     | /api/admin/invoices | Admin         | Összes számla listája      |

## 4.7 Felhasználók (admin)

| **Metódus** | **Útvonal**           | **Szerepkör** | **Leírás**               |
| ----------- | --------------------- | ------------- | ------------------------ |
| **GET**     | /api/admin/users      | Admin         | Felhasználók listája     |
| **GET**     | /api/admin/users/{id} | Admin         | Felhasználó részletei    |
| **PUT**     | /api/admin/users/{id} | Admin         | Felhasználó szerkesztése |
| **DELETE**  | /api/admin/users/{id} | Admin         | Fiók deaktiválása (soft) |

## 4.8 Riportok és audit log (admin)

| **Metódus** | **Útvonal**                     | **Szerepkör** | **Leírás**                 |
| ----------- | ------------------------------- | ------------- | -------------------------- |
| **GET**     | /api/admin/reports/sales        | Admin         | Eladási statisztikák       |
| **GET**     | /api/admin/reports/top-products | Admin         | Legtöbbet rendelt termékek |
| **GET**     | /api/admin/audit-log            | Admin         | Audit log bejegyzések      |

# 5\. Webshop frontend (Vue 3)

Single Page Application, Vite builddel, TypeScript-tel. Az állapotot Pinia store-ok kezelik, a navigációt Vue Router.

## 5.1 Oldalak és képernyők

| **Oldal**              | **Útvonal**         | **Leírás**                                  |
| ---------------------- | ------------------- | ------------------------------------------- |
| **Főoldal**            | /                   | Kiemelt termékek, kategória navigáció       |
| **Termék lista**       | /products           | Szűrés, keresés, lapozás, kártya nézet      |
| **Terméklap**          | /products/:id       | Részletes leírás, ár, készlet, kosárba gomb |
| **Kosár**              | /cart               | Tételek, mennyiség módosítás, végösszeg     |
| **Checkout**           | /checkout           | Szállítási cím, fizetési mód (placeholder)  |
| **Visszaigazolás**     | /orders/:id/success | Rendelés összegzés, sorszám                 |
| **Rendelések**         | /account/orders     | Rendelés előzmények, státuszok              |
| **Rendelés részletek** | /account/orders/:id | Tételek, számla letöltés gomb               |
| **Profil**             | /account/profile    | Személyes adatok szerkesztése               |
| **Bejelentkezés**      | /login              | Email + jelszó form, JWT tárolás            |
| **Regisztráció**       | /register           | Regisztrációs form, validáció               |

## 5.2 Pinia store-ok

- authStore - token, felhasználói adatok, bejelentkezés/kijelentkezés
- cartStore - kosár tartalom, szinkronizáció az API-val
- productStore - termékek, kategóriák, keresési állapot
- orderStore - rendelés előzmények

## 5.3 Route guard-ok

- Nem bejelentkezett user → /login redirect (kosár, checkout, profil)
- Admin route → 403 oldal (admin végpontok webes oldalon nem elérhetők)

# 6\. Admin desktop alkalmazás (Avalonia UI)

Különálló .NET 8 desktop alkalmazás Avalonia UI keretrendszerrel, MVVM (CommunityToolkit.Mvvm) architektúrával. Az app az ASP.NET Core API Admin végpontjain keresztül kommunikál, ugyanazt a JWT rendszert használja. Cross-platform: Windows, macOS és Linux alatt is futtatható.

_Az admin app teljesen különálló a webes frontend-től - nem megosztott kód, saját UI, saját state management. Az egyetlen közös pont az API és a JWT token formátum._

## 6.1 Projekt struktúra

- WebShop.Admin - Avalonia UI projekt (Views, ViewModels)
- WebShop.Admin.Core - Services, Models, HTTP kliensek
- WebShop.Admin.Core az API-val HttpClient + typed clients-on keresztül kommunikál

## 6.2 Képernyők

| **Képernyő**            | **Funkciók**                                                                 |
| ----------------------- | ---------------------------------------------------------------------------- |
| **Bejelentkezés**       | Email + jelszó form, JWT token tárolás SecureStorage-ben, hibaüzenetek       |
| **Dashboard (főoldal)** | Összesített statisztikák: napi rendelések, bevétel, készlet figyelmeztetések |
| **Termékkezelő**        | DataGrid lista, szűrés, keresés, Létrehozás/Szerkesztés dialog, Soft delete  |
| **Kategóriakezelő**     | Fa nézet alkategóriákkal, CRUD műveletek                                     |
| **Rendeléskezelő**      | Szűrés státusz szerint, részletek panel, státusz módosítás dropdown          |
| **Felhasználókezelő**   | Lista, keresés, részletek megtekintés, fiók deaktiválás                      |
| **Számlák**             | Lista, PDF megnyitás / letöltés, újragenerálás trigger                       |
| **Riportok**            | Időszaki eladási grafikon, top termékek, export (CSV)                        |
| **Audit log**           | Szűrhető esemény napló, részletek panel                                      |
| **Beállítások**         | API base URL, token lejárat megjelenítése, kijelentkezés                     |

## 6.3 MVVM felépítés

- Minden képernyőhöz saját ViewModel (pl. ProductsViewModel, OrdersViewModel)
- CommunityToolkit.Mvvm: \[ObservableProperty\], \[RelayCommand\] source generátorok
- IApiService interfész → mock-olható, tesztelhető
- HttpClient Typed Client pattern (IProductsClient, IOrdersClient stb.)
- Token kezelés: HttpMessageHandler interceptorban automatikus Authorization header
- Refresh token logika: 401 response esetén automatikus token megújítás

# 7\. Számlázás (Számlázz.hu integráció)

A számlagenerálás aszinkron módon történik, Hangfire background job-on keresztül, hogy a checkout response ne függjön az API hívástól.

## 7.1 Folyamat

- 1\. Vásárló leadja a rendelést (POST /api/orders)
- 2\. Az API létrehozza az Order rekordot, Invoice rekordot (Status: Pending)
- 3\. Az API Hangfire job-ot ütemez: GenerateInvoiceJob(orderId)
- 4\. A checkout response azonnal visszatér (201 Created + Order ID)
- 5\. Háttérben a Hangfire job meghívja a Számlázz.hu API-t
- 6\. Sikeres generálás esetén az Invoice rekord frissül (InvoiceNumber, PdfUrl, Status: Issued)
- 7\. Sikertelen esetén: Hangfire automatikusan újrapróbálja (3x, exponenciális backoff)

## 7.2 Implementációs részletek

- ISzamlazzHuClient interfész → könnyű mock / csere
- Hangfire Dashboard elérhető: /hangfire (Admin auth-val védve)
- Job retry policy: 3 kísérlet, 1 / 5 / 15 perces késleltetéssel
- Failed job esetén az Invoice.Status = Failed, admin app-ban látható és újraindítható

# 8\. Solution struktúra

Clean Architecture elvek mentén szervezett projektek, a közös logika külön osztálykönyvtárakban.

| **Projekt**                  | **Tartalom**                                                         |
| ---------------------------- | -------------------------------------------------------------------- |
| **WebShop.Api**              | ASP.NET Core Web API - Controllers/Endpoints, Middleware, Program.cs |
| **WebShop.Core**             | Domain entitások, Interfaces, DTOs, Exceptions (nincs függőség)      |
| **WebShop.Infrastructure**   | EF Core DbContext, Repositories, Hangfire jobs, Számlázz.hu kliens   |
| **WebShop.Application**      | Business logic Services, Validation (FluentValidation)               |
| **WebShop.Admin**            | Avalonia UI - Views (.axaml), App.axaml, Assets                      |
| **WebShop.Admin.Core**       | ViewModels, Services, HttpClients, Models (Avalonia-mentes)          |
| **WebShop.Tests**            | xUnit unit tesztek (Service réteg, Validators)                       |
| **WebShop.IntegrationTests** | API integrációs tesztek (WebApplicationFactory)                      |

# 9\. Fejlesztési ütemterv

8 sprint, mindegyik kb. 1-2 hetes iteráció. A sprintek sorrendje függőségek szerint alakul - az API végpontok megelőzik a frontend és admin fejlesztést.

| **#** | **Sprint**       | **Ticketek**            | **Tartalom**                                                                            |
| ----- | ---------------- | ----------------------- | --------------------------------------------------------------------------------------- |
| **1** | **Alapok**       | INFRA-1..4, DB-1..4     | Solution struktúra, EF Core, PostgreSQL, Redis, Hangfire konfiguráció és alap migrációk |
| **2** | **Autentikáció** | AUTH-1..3, API-1..3     | JWT auth, regisztráció, bejelentkezés, refresh token, role-alapú policy-k               |
| **3** | **Katalógus**    | API-4..5, FE-1..4       | Termék és kategória API, Vue termékoldal, keresés, szűrés                               |
| **4** | **Kosár**        | API-6, FE-5             | Cart API, Vue kosár oldal, mennyiség módosítás, törlés                                  |
| **5** | **Checkout**     | API-7..8, DB-5, FE-6..7 | Rendelés leadás, készlet csökkentés, visszaigazolás oldal, audit log                    |
| **6** | **Admin app**    | ADMIN-1..6              | Avalonia MVVM alap, bejelentkezés, termék- és rendeléskezelő képernyők                  |
| **7** | **Számlázás**    | INV-1..4, FE-8          | Számlázz.hu integráció, Hangfire job, PDF letöltés                                      |
| **8** | **Minőség**      | TEST-1..4, CI-1         | Unit tesztek, integrációs tesztek, CI pipeline, dokumentáció                            |

_Az admin desktop app (6. sprint) párhuzamosan is fejleszthető az 5. sprinttől, ha az API végpontok rendelkezésre állnak._

# 10\. Fejlesztési ticketek

A diagramokból és specifikációból levezetett GitHub issue-k. Minden ticket önállóan zárható.

## Infrastruktúra (INFRA)

- INFRA-1: Solution struktúra létrehozása (8 projekt, NuGet referenciák)
- INFRA-2: EF Core + PostgreSQL konfiguráció, DbContext, connection string
- INFRA-3: Redis konfiguráció (StackExchange.Redis, IDistributedCache)
- INFRA-4: Hangfire konfiguráció (PostgreSQL storage, Dashboard, retry policy)
- INFRA-5: FluentValidation pipeline integráció az API-ba
- INFRA-6: Global exception handler middleware (ProblemDetails)

## Adatbázis (DB)

- DB-1: User, Category, Product entitások és kezdeti migration
- DB-2: Cart, CartItem entitások és migration
- DB-3: Order, OrderItem, Invoice entitások és migration
- DB-4: AuditLog entitás és migration
- DB-5: Seed data (admin user, alap kategóriák, teszt termékek)

## Autentikáció (AUTH)

- AUTH-1: User entitás Role mezővel, BCrypt jelszó hash
- AUTH-2: JWT generálás, validáció, Refresh token mechanizmus
- AUTH-3: Role-alapú Authorization policy-k (RequireRole)
- AUTH-4: Vue route guard implementáció
- AUTH-5: Avalonia token tárolás + automatikus Authorization header

## API végpontok (API)

- API-1..3: Auth endpoints (register, login, refresh, logout)
- API-4..5: Products és Categories CRUD
- API-6: Cart endpoints
- API-7..8: Orders endpoints (checkout, lista, részletek)
- API-9..10: Admin Orders és Users endpoints
- API-11: Invoices endpoints
- API-12: Reports és Audit log endpoints

## Webshop frontend (FE)

- FE-1: Vue Router setup, route guard-ok
- FE-2: Pinia store-ok (auth, cart, product, order)
- FE-3: Termék lista oldal (szűrés, keresés, lapozás)
- FE-4: Terméklap (részletek, kosárba gomb)
- FE-5: Kosár oldal (mennyiség, törlés, végösszeg)
- FE-6: Checkout form (szállítási cím, fizetési placeholder)
- FE-7: Visszaigazolás oldal
- FE-8: Rendelés előzmények + számla letöltés
- FE-9: Bejelentkezés / Regisztráció oldalak

## Admin desktop app (ADMIN)

- ADMIN-1: Avalonia projekt setup, MVVM alap, navigáció
- ADMIN-2: Bejelentkezés képernyő + token kezelés
- ADMIN-3: Dashboard képernyő (statisztikák)
- ADMIN-4: Termékkezelő képernyő (DataGrid, CRUD dialog)
- ADMIN-5: Kategóriakezelő képernyő
- ADMIN-6: Rendeléskezelő képernyő (státusz módosítás)
- ADMIN-7: Felhasználókezelő képernyő
- ADMIN-8: Riportok képernyő (grafikon, CSV export)
- ADMIN-9: Audit log képernyő

## Számlázás (INV)

- INV-1: ISzamlazzHuClient interfész + implementáció
- INV-2: GenerateInvoiceJob Hangfire job
- INV-3: Invoice rekord frissítés (PdfUrl, Status)
- INV-4: PDF letöltés endpoint (proxy vagy redirect)

## Minőség (TEST)

- TEST-1: Service réteg unit tesztek (xUnit + Moq)
- TEST-2: FluentValidation tesztek
- TEST-3: API integrációs tesztek (WebApplicationFactory)
- TEST-4: CI pipeline (GitHub Actions - build + test)

# 11\. Jövőbeli fejlesztések

Az alábbi funkciók nincsenek benne az alaptervben, de az architektúra támogatja őket:

- Fizetési integráció: Stripe vagy SimplePay (OTP) - a placeholder cserélhető
- Email értesítések: rendelés visszaigazolás, státusz változás (SendGrid / SMTP)
- Kedvezmény kuponok: Coupon entitás, checkout kedvezmény logika
- Termék vélemények: Review entitás, értékelés megjelenítés
- Kívánságlista: Wishlist entitás, user-hez kötve
- Többnyelvűség: i18n Vue-ban, lokalizált termékleírások
- Kubernetes deployment: ha a skálázás szükségessé válik
- Monitoring: OpenTelemetry + Grafana dashboard