# פרויקט מכירה סינית – Chinese Sale API

מדריך הגשה ל**אלישבע**

---

## אודות הפרויקט

**Chinese Sale** היא מערכת לניהול מכירה סינית (הגרלת מתנות), הכוללת:

- **Backend:** ASP.NET Core 8 Web API (`SaleApi`)
- **Frontend:** Angular SPA
- **מסד נתונים:** SQL Server (Entity Framework Core)
- **תשתית:** Docker + Redis

הארכיטקטורה מבוססת שכבות: **Controller → Service → Repository → DbContext**.

---

## מה מומש בפרויקט

### תיעוד ארכיטקטורה ומיקרו-סרביסים

| קובץ | תיאור |
|------|--------|
| `.github/copilot-instructions.md` | הנחיות ל-Controllers ו-Repositories |
| `.github/api-architect.agent.md` | סוכן AI מותאם לפרויקט |
| `microservices-planning.md` | תכנון חלוקה תיאורטית למיקרו-סרביסים |
| `mongodb-sample-data/` | קבצי JSON לדוגמה למעבר ל-MongoDB |

### אבטחת JWT עם HttpOnly Cookies

- הגדרות JWT ב-`appsettings.json` (`SecretKey`, `Issuer`, `Audience`, `ExpiryMinutes`)
- יצירת Token ב-`TokenService` / `UserService.AuthenticateAsync`
- שמירת Token ב-Cookie `access_token` עם: `HttpOnly`, `Secure`, `SameSite=Strict`
- `AddAuthentication` + `AddJwtBearer` ב-`Program.cs` עם חילוץ Token מה-Cookie
- `[Authorize(Roles = "Admin")]` על פעולות מנהל
- תמיכת Swagger – כפתור **Authorize** (מנעול)

### חסימת הצפות – Rate Limiting

- מדיניות `"SpecificPolicy"`: **10 בקשות / 10 שניות**
- החזרת **429 Too Many Requests** בחריגה
- מיושם על `GiftController` באמצעות `[EnableRateLimiting]`

### שיפור ביצועים – Redis Cache

- `AddStackExchangeRedisCache` ב-`Program.cs`
- Cache למפתח `"all_gifts"` ב-`GET /api/Gift` (תוקף: 10 דקות)
- Invalidation אוטומטי ביצירה, עדכון ומחיקה של מתנה

### Docker

- `Dockerfile` – build multi-stage על .NET 8
- `docker-compose.yml` – שירותי `webapi` + `redis`
- Redis מוגדר ל-`redis:6379` ברשת הפנימית של Docker

---

## מדריך הפעלה – צעד אחר צעד

### דרישות מקדימות

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) מותקן ופועל

### הרצת המערכת

1. פתחו **Terminal** (PowerShell / CMD).
2. נווטו לתיקיית ה-API:

   ```bash
   cd "ChineseSale-Api-main (1)/ChineseSale-Api-main/SaleApi"
   ```

3. הריצו:

   ```bash
   docker compose up --build
   ```

4. המתינו עד ששני השירותים (`webapi`, `redis`) במצב **Running**.

### גישה ל-Swagger

פתחו בדפדפן:

**http://localhost:8080/swagger**

---

## בדיקת JWT ב-Swagger

### שלב 1 – התחברות (Login)

1. ב-Swagger, מצאו: **`POST /api/Auth/login`**
2. לחצו **Try it out**
3. הזינו JSON לדוגמה:

   ```json
   {
     "email": "admin@chinesesale.com",
     "password": "your-password"
   }
   ```

4. לחצו **Execute**
5. בתשובה (200) – העתיקו את ערך **`token`** מה-JSON

> בנוסף, ה-Token נשמר אוטומטית ב-Cookie `access_token` (HttpOnly).

### שלב 2 – Authorize

1. לחצu על כפתור **Authorize** (מנעול) בראש דף Swagger
2. בשדה הזינu:

   ```
   Bearer {הטוקן שהעתקתם}
   ```

   (כולל המילה `Bearer` ורווח לפני הטוקן)

3. לחצu **Authorize** → **Close**

### שלב 3 – בדיקת Endpoint מוגן

1. נסu endpoint שדורש Admin, למשל: **`POST /api/Gift`**
2. אם ה-Token תקף ויש הרשאת Admin – הבקשה תצליח
3. ללא Token – תקבlu **401 Unauthorized**

---

## Endpoints עיקריים

| תחום | Route | תיאור |
|------|-------|--------|
| Auth | `/api/Auth/login`, `/register` | התחברות והרשמה |
| Gifts | `/api/Gift` | מתנות + Cache + Rate Limit |
| Bag | `/api/Bag` | עגלת קניות + Checkout |
| Orders | `/api/Order` | הזמנות והיסטוריה |
| Random | `/api/Random` | הגרלות וזוכים |

---

## מבנה תיקיות (עיקרי)

```
final-api-angular/
├── README.md                          ← קובץ זה
├── microservices-planning.md
├── mongodb-sample-data/
├── .github/
│   ├── copilot-instructions.md
│   └── api-architect.agent.md
├── ChineseSale-Api-main (1)/
│   └── ChineseSale-Api-main/
│       └── SaleApi/                   ← Web API + Docker
│           ├── Dockerfile
│           ├── docker-compose.yml
│           ├── Program.cs
│           └── Controllers/
└── ChineseSale-angular-main/          ← Frontend Angular
```

---

## הערות

- **SQL Server** אינו כלול ב-Docker Compose (לפי דרישות המורה). לפיתוח מקומי – SQL Server / LocalDB.
- **Redis** ב-Docker: שירות `redis` על פורט 6379.
- **Angular Frontend** – רץ בנפרד (`ng serve`, פורט 4200).

---

**מגישה:** _______________  
**תאריך:** _______________
