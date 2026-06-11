# הנחיות Copilot – פרויקט Chinese Sale

> **חיסכון בטוקנים:** קרא רק את הסעיף הרלוונטי לשאלה. אל תטען את שני הסעיפים יחד אלא אם המשימה חוצה שכבות.

## הקשר כללי (קצר)

| פריט | ערך |
|------|-----|
| פרויקט | Chinese Sale – מכירה סינית / הגרלת מתנות |
| Backend | ASP.NET Core 8, `SaleApi` |
| ORM | Entity Framework Core 9 + SQL Server |
| Frontend | Angular (תיקייה נפרדת) |
| ארכיטקטורה | Controller → Service → Repository → `SaleContextDB` |
| מיקום API | `ChineseSale-Api-main (1)/ChineseSale-Api-main/SaleApi/` |

**ישויות:** `User`, `Gift` (≈ מוצר), `Order`, `Bag` (עגלה), `Category`, `Doner`, `Winner`

**DI:** כל ה-Repositories וה-Services רשומים ב-`Program.cs` כ-`AddScoped`.

---

<!-- ============================================================ -->
<!-- REPOSITORIES – טען סעיף זה בלבד בעבודה על שכבת הנתונים -->
<!-- ============================================================ -->

## § Repositories

### מיקום ומבנה

```
SaleApi/Repositories/
├── I{Entity}Repository.cs   ← ממשק
└── {Entity}Repository.cs    ← מימוש
```

**Repositories קיימים:** `User`, `Gift`, `Order`, `Bag`, `Category`, `Doner`, `Random`

### כללים מחייבים

1. **אחריות יחידה:** Repository מדבר רק עם EF Core ו-`SaleContextDB`. אין לוגיקה עסקית, אין מיפוי ל-DTO, אין שליחת מייל.
2. **הזרקת תלות:** קבל `SaleContextDB` ב-constructor. שמור בשדה `_context` (או `SaleContextDB _context` – שמור על הסגנון הקיים).
3. **ממשק חובה:** כל Repository חייב `I{Entity}Repository`. רישום DI: `AddScoped<I{X}Repository, {X}Repository>()`.
4. **Async בלבד:** כל הפעולות `async Task` / `Task<T>`.
5. **Include ל-navigation:** כשצריך נתונים מקושרים – `Include()` / `ThenInclude()`. דוגמה: `Orders` עם `Gift` ו-`User`.
6. **AsNoTracking:** לשאילתות read-only שלא יעודכנו – העדף `.AsNoTracking()`.
7. **SaveChanges:** קרא `SaveChangesAsync()` רק בפעולות CUD (Create/Update/Delete).
8. **null במקום Exception:** שליפה לפי ID → החזר `null` אם לא נמצא. אל תזרוק `NotFoundException` מה-Repository.
9. **אין DTO:** Repository מחזיר `Models` בלבד (`Gift`, `Order`, `Bag` וכו').

### דפוסי קוד נפוצים בפרויקט

**שליפה עם Include:**
```csharp
return await _context.Gifts
    .Include(g => g.Doner)
    .FirstOrDefaultAsync(g => g.Id == id);
```

**הוספה:**
```csharp
_context.Gifts.Add(gift);
await _context.SaveChangesAsync();
return gift;
```

**עדכון (UserRepository):**
```csharp
_context.Entry(existing).CurrentValues.SetValues(user);
await _context.SaveChangesAsync();
```

**מחיקה:**
```csharp
var entity = await _context.Gifts.FindAsync(id);
if (entity != null) {
    _context.Gifts.Remove(entity);
    await _context.SaveChangesAsync();
}
```

**בדיקת קיום לפני FK:**
```csharp
var userExists = await _context.Users.AnyAsync(u => u.Id == order.IdUser);
if (!userExists) throw new Exception("User or Gift not found in database.");
```

### מפת Repositories – פעולות ו-Include

| Repository | פעולות עיקריות | Include נדרש |
|------------|----------------|--------------|
| `UserRepository` | CRUD, `GetByEmailAsync`, `EmailExistsAsync` | — |
| `GiftRepository` | CRUD, `GetGiftDoner`, חיפוש לפי שם/תורם | `Doner` |
| `OrderRepository` | `GetAllOrders`, `AddOrder`, `GetOrdersByGiftId`, `GetOrdersByUserId`, מיון פופולריות/מחיר | `Gift`, `User` |
| `BagRepository` | CRUD, `GetBagByUser`, `ClearUserBag`, `RemoveGiftFromAllBags`, merge כמות | `Gift`, `User` |
| `CategoryRepository` | CRUD, `GetGiftByCategoryId` | `Gifts` (בשאילתות קטגוריה) |
| `DonerRepository` | CRUD, `GetAllDonerWithGift` | `Gifts` |
| `RandomRepository` | `GetOrdersForGift`, `SaveWinner`, `IsGiftDrawnAsync`, `GetDrawnGiftIdsAsync` | `User`, `Gift` ב-Winners |

### לוגיקה ייחודית שחייבת להישמר

**BagRepository.NewGiftToBag:**
- אם `(IdUser, IdGift)` כבר קיים → **הגדל Quantity**, אל תיצור רשומה חדשה.
- אחרת → צור `Bag` חדש עם שדות נקיים (ללא Id).
- החזר עם `Include(b => b.Gift)`.

**OrderRepository.AddOrder:**
- וודא ש-User ו-Gift קיימים לפני הוספה.

**RandomRepository.SaveWinner:**
- הוסף `Winner` **וגם** עדכן `Order.Win = true` באותה transaction.

**BagRepository.ClearUserBag:**
- מחק את **כל** פריטי הסל של משתמש (לאחר checkout).

### מה לא לעשות ב-Repository

- ❌ מיפוי ל-DTO
- ❌ ולידציה עסקית (למשל "מתנה כבר הוגרלה")
- ❌ יצירת `OrderGroupId` (זה ב-Service)
- ❌ החזרת `ActionResult` / HTTP status codes
- ❌ `Console.WriteLine` – השתמש ב-throw או השאר ל-Service/Controller

### הוספת Repository חדש – checklist

1. צור `I{Name}Repository.cs` עם חתימות async
2. צור `{Name}Repository.cs` עם `SaleContextDB`
3. רשום ב-`Program.cs`: `AddScoped<I{Name}Repository, {Name}Repository>()`
4. הזרק ל-Service המתאים (לא ל-Controller ישירות)
5. אם יש FK – השתמש ב-`Include` ובדיקת `AnyAsync`

---

<!-- ============================================================ -->
<!-- CONTROLLERS – טען סעיף זה בלבד בעבודה על שכבת ה-API -->
<!-- ============================================================ -->

## § Controllers

### מיקום ומבנה

```
SaleApi/Controllers/
├── AuthController.cs
├── UserController.cs
├── GiftController.cs
├── DonerController.cs
├── CategoryController.cs
├── BagController.cs
├── OrderController.cs      ← קובץ: OrederController.cs (שגיאת כתיב!)
└── RandomController.cs
```

**Route ברירת מחדל:** `[Route("api/[controller]")]` → `/api/Gift`, `/api/Bag` וכו'.

### כללים מחייבים

1. **אחריות יחידה:** Controller מקבל HTTP, קורא ל-**Service** בלבד. **אסור** להזריק Repository ישירות.
2. **Attributes:** `[ApiController]` + `[Route("api/[controller]")]` על כל controller.
3. **DTO בכניסה/יציאה:** `[FromBody]` ל-JSON, `[FromForm]` להעלאת מתנה (תמונה), `[FromQuery]` לחיפוש.
4. **קודי HTTP:**
   - `200 OK` – הצלחה
   - `201 Created` – יצירה (עם `CreatedAtAction` כשמתאים)
   - `204 NoContent` – מחיקה מוצלחת
   - `400 BadRequest` – ולידציה / ArgumentException
   - `401 Unauthorized` – התחברות נכשלה
   - `404 NotFound` – ישות לא נמצאה
   - `500` – שגיאה לא צפויה
5. **ModelState:** ב-POST/PUT – בדוק `ModelState.IsValid` לפני קריאה ל-Service.
6. **Try/Catch:** רוב ה-controllers עוטפים ב-try/catch. `ArgumentException` → 400, אחר → 500.
7. **ProducesResponseType:** השתמש ב-`UserController` / `AuthController` כדוגמה לתיעוד Swagger.

### מפת Controllers – Endpoints

| Controller | Method | Route | תיאור |
|------------|--------|-------|-------|
| **Auth** | POST | `/api/Auth/login` | התחברות → `LoginResponseDto` + JWT |
| **Auth** | POST | `/api/Auth/register` | הרשמה |
| **User** | GET | `/api/User` | כל המשתמשים |
| **User** | GET | `/api/User/{id}` | משתמש לפי ID |
| **User** | POST/PUT/DELETE | `/api/User`, `/{id}` | CRUD |
| **Gift** | GET | `/api/Gift` | כל המתנות |
| **Gift** | GET | `/api/Gift/{id}` | מתנה לפי ID |
| **Gift** | GET | `/api/Gift/{id}/doner` | תורם של מתנה |
| **Gift** | GET | `/api/Gift/doner/?name=` | חיפוש לפי שם תורם |
| **Gift** | GET | `/api/Gift/name/?name=` | חיפוש לפי שם מתנה |
| **Gift** | POST | `/api/Gift` | יצירה `[FromForm]` |
| **Gift** | PUT | `/api/Gift` | עדכון `[FromForm]` |
| **Gift** | DELETE | `/api/Gift/{id}` | מחיקה |
| **Doner** | GET | `/api/Doner` | כל התורמים |
| **Doner** | GET | `/api/Doner/withGifts` | תורמים + מתנות |
| **Doner** | GET | `/api/Doner/withGifts/{id}` | תורם + מתנות לפי ID |
| **Doner** | GET | `/api/Doner/doner/name?`, `/doner/email?`, `/doner/gift?` | חיפושים |
| **Doner** | POST/PUT/DELETE | CRUD | |
| **Category** | GET | `/api/Category` | כל הקטגוריות |
| **Category** | GET | `/api/Category/gift/{categoryId}` | מתנות בקטגוריה |
| **Category** | POST/PUT/DELETE | CRUD | |
| **Bag** | GET | `/api/Bag` | כל הסלים |
| **Bag** | GET | `/api/Bag/{id}`, `/user/{id}`, `/gift/{id}` | שליפות |
| **Bag** | POST | `/api/Bag/add` | הוספה לסל `[FromBody]` |
| **Bag** | POST | `/api/Bag/checkout/{userId}` | **Checkout** → יוצר Orders |
| **Bag** | DELETE | `/api/Bag/{id}` | הסרה מהסל |
| **Order** | GET | `/api/Order` | כל ההזמנות |
| **Order** | GET | `/api/Order/history/{userId}` | היסטוריית משתמש |
| **Order** | GET | `/api/Order/by-gift/{giftId}` | הזמנות למתנה |
| **Order** | GET | `/api/Order/sort/popularity` | מיון לפי פופולריות |
| **Order** | GET | `/api/Order/sort/price` | מיון לפי מחיר |
| **Random** | POST | `/api/Random/{giftId}` | **הגרלה** – בחירת זוכה |
| **Random** | GET | `/api/Random/is-drawn/{giftId}` | האם הוגרל |
| **Random** | GET | `/api/Random/Winner` | רשימת זוכים |

### זרימות עסקיות קריטיות (Controller → Service)

**Checkout (`BagController.Checkout`):**
```
POST /api/Bag/checkout/{userId}
→ BagService.ProcessCheckout
→ לכל פריט בסל: יצירת Order (כמות = מספר שורות) + OrderGroupId
→ ריקון סל
```

**הגרלה (`RandomController.RunDraw`):**
```
POST /api/Random/{giftId}
→ RandomService.ExecuteDraw
→ בחירה אקראית מ-Orders של המתנה
→ שמירת Winner + Order.Win=true
→ הסרת המתנה מכל הסלים
```

**הוספה לסל (`BagController.AddToBag`):**
```
POST /api/Bag/add
→ BagService בודק שלא הוגרל (RandomRepository)
→ BagRepository merge/add
→ מחזיר Bag מלא עם Gift (חשוב ל-Angular!)
```

### Authorization (מצב נוכחי)

- JWT מוגדר ב-`appsettings.json` אך **Authentication מוערת** ב-`Program.cs`.
- `[Authorize(Roles = "Admin")]` קיים בקוד אך **מוערת** ברוב ה-endpoints.
- בעת הפעלה: בטל הערה על JWT middleware + `[Authorize]` לפעולות Admin.

### CORS ו-Static Files

- CORS policy: `"AngularPolicy"` – `AllowAnyOrigin/Method/Header`.
- `UseStaticFiles()` – לתמונות מתנות (`wwwroot`).

### מה לא לעשות ב-Controller

- ❌ גישה ישירה ל-`_context` / EF Core
- ❌ לוגיקה עסקית (חישוב OrderGroupId, הגרלה, hash סיסמה)
- ❌ החזרת Entity עם navigation cycles (מטופל ב-`ReferenceHandler.IgnoreCycles`)
- ❌ `Console.WriteLine` – השתמש ב-`ILogger<T>`

### הוספת Controller / Endpoint – checklist

1. צור Controller עם `[ApiController]` + `[Route("api/[controller]")]`
2. הזרק `I{Entity}Service` + `ILogger<T>` (אופציונלי)
3. הגדר `[HttpGet/Post/Put/Delete]` + `[ProducesResponseType]`
4. DTO מ-`SaleApi/Dto/` (static nested classes: `static SaleApi.Dto.GiftDto`)
5. וודא שה-Service רשום ב-`Program.cs`
6. לפעולות Admin – הוסף `[Authorize(Roles = "Admin")]` כש-JWT פעיל

### DTOs – מיקום

```
SaleApi/Dto/
├── UserDto.cs      → LoginRequestDto, UserCreateDto, UserResponseDto...
├── GiftDto.cs      → CreateGiftDto, GetGiftDto, UpdateGiftDto...
├── BagDto.cs       → CreateBagDto, GetBagDto
├── OrderDto.cs
├── CategoryDto.cs
└── DonerDto.cs
```

**שימוש:** `using static SaleApi.Dto.GiftDto;` → `GetGiftDto`
