# B-SMART Class Diagram and Entity Relationship Diagram

These diagrams are generated from the current C# WinForms project structure and the MySQL tables referenced in the code.

## Class Diagram

```mermaid
classDiagram
    direction LR

    class Program {
        +Main() void
    }

    class Form

    class Session {
        +UserID int
        +Username string
        +Role string
        +BarangayID int
        +BarangayName string
        +IsMayor bool
        +Clear() void
    }

    class RegisterData {
        +FirstName string
        +MiddleName string
        +LastName string
        +Birthday string
        +Age int
        +Gender string
        +BarangayID int
        +BarangayName string
        +Username string
        +Password string
    }

    class DatabaseHelper {
        -connectionString string
        +GetAllRecords() DataTable
        +SearchRecords(keyword) DataTable
        +AddRecord(...) bool
        +UpdateRecord(...) bool
        +ArchiveRecord(recordId) bool
        +DeleteRecord(recordId) bool
        +GetTotalResidents() int
        +GetTotalRecords() int
        +GetMostCommonCase() string
        +GetInventory() DataTable
        +AddInventoryItem(...) bool
        +GetOrCreateResident(...) int
    }

    class BsmartIdHelper {
        +EnsureCodes(conn) void
        +GetOrCreateResidentCode(conn, firstName, lastName, birthday, barangayId) string
        +NextMedicineCode(conn, barangayId) string
        +NextVaccineCode(conn, barangayId) string
    }

    class MayorFormHelper {
        +LoadResidentAccounts(barangay, keyword) DataTable
        +LoadResidentRecords(barangay, keyword) DataTable
        +LoadHealthRecords(barangay, keyword, period) DataTable
        +LoadInventory(barangay, keyword) DataTable
        +LoadRecentHealthRecords() DataTable
        +CountResidents() int
        +CountHealthRecords(barangay) int
        +StyleGrid(grid) void
        +DownloadGridAsPdf(...) void
    }

    class BsmartNotificationService {
        +AttachToOpenForms() void
        -LoadNotificationsForCurrentUser() List
        -ArchiveNearExpiryInventory(conn) void
        -MarkNotificationsViewed(...) void
    }

    class SearchSuggestionService {
        +AttachToOpenForms() void
        -AttachSearchBox(form, textBox) void
        -BuildSuggestions(form) AutoCompleteStringCollection
    }

    class SettingsNavigationService {
        +AttachToOpenForms() void
        -OpenSettingsForCurrentUser() void
        -ReturnToDashboard() void
    }

    class loginAS
    class loginMayor
    class loginLGU
    class loginBCap
    class loginRes

    class Register
    class Register2
    class Register3
    class BarangayItem

    class MayorDash
    class MayorResRec
    class MayorResHealthRec
    class MayorViewHealthRep
    class MayorViewInventory

    class TayudLGU
    class TayudManageHealth
    class TayudViewHealth
    class TayudGenerateReport
    class TayudInventory
    class TayudViewAppointments
    class TayudViewArchive
    class TayudViewArchive2

    class TayudBCapDash
    class TayudBCManageRes
    class TayudBCViewRes
    class TayudBCViewResHealthRep
    class TayudBCapManageResAcc
    class TayudBCapViewResAcc
    class TayudBCapManageFAcc
    class TayudBCapViewArch
    class TayudBCapViewArch2

    class ResidentDash
    class ResidentAppointment
    class ResViewHealth
    class ResViewServices

    class Settings
    class Settings2
    class RoundedBtn
    class NotifItem
    class NotificationDropdown

    Form <|-- loginAS
    Form <|-- loginMayor
    Form <|-- loginLGU
    Form <|-- loginBCap
    Form <|-- loginRes
    Form <|-- Register
    Form <|-- Register2
    Form <|-- Register3
    Form <|-- MayorDash
    Form <|-- MayorResRec
    Form <|-- MayorResHealthRec
    Form <|-- MayorViewHealthRep
    Form <|-- MayorViewInventory
    Form <|-- TayudLGU
    Form <|-- TayudManageHealth
    Form <|-- TayudViewHealth
    Form <|-- TayudGenerateReport
    Form <|-- TayudInventory
    Form <|-- TayudViewAppointments
    Form <|-- TayudViewArchive
    Form <|-- TayudViewArchive2
    Form <|-- TayudBCapDash
    Form <|-- TayudBCManageRes
    Form <|-- TayudBCViewRes
    Form <|-- TayudBCViewResHealthRep
    Form <|-- TayudBCapManageResAcc
    Form <|-- TayudBCapViewResAcc
    Form <|-- TayudBCapManageFAcc
    Form <|-- TayudBCapViewArch
    Form <|-- TayudBCapViewArch2
    Form <|-- ResidentDash
    Form <|-- ResidentAppointment
    Form <|-- ResViewHealth
    Form <|-- ResViewServices
    Form <|-- Settings
    Form <|-- Settings2

    Program --> loginAS : starts
    loginAS --> loginMayor : mayor login
    loginAS --> loginLGU : staff login
    loginAS --> loginBCap : captain login
    loginAS --> loginRes : resident login

    loginMayor --> MayorDash
    loginLGU --> TayudLGU
    loginBCap --> TayudBCapDash
    loginRes --> ResidentDash

    Register --> Register2
    Register2 --> Register3
    Register2 --> BarangayItem
    Register3 --> RegisterData
    Register3 --> BsmartIdHelper

    MayorDash --> MayorFormHelper
    MayorResRec --> MayorFormHelper
    MayorResHealthRec --> MayorFormHelper
    MayorViewHealthRep --> MayorFormHelper
    MayorViewInventory --> MayorFormHelper

    TayudLGU --> BsmartIdHelper
    TayudManageHealth --> BsmartIdHelper
    TayudInventory --> BsmartIdHelper
    TayudViewAppointments --> Session
    ResidentAppointment --> Session
    ResidentAppointment --> BsmartNotificationService

    Settings --> Session
    Settings2 --> Session
    BsmartNotificationService --> Session
    SearchSuggestionService --> Form
    SettingsNavigationService --> Session

    NotificationDropdown --|> Panel
    NotifItem --|> Panel
    RoundedBtn --|> Button
```

## Entity Relationship Diagram

```mermaid
erDiagram
    BARANGAYS ||--o{ USERS : "has accounts"
    BARANGAYS ||--o{ HEALTH_RECORDS : "has health records"
    BARANGAYS ||--o{ APPOINTMENTS : "receives appointments"
    BARANGAYS ||--o{ MEDICINES : "stores medicines"
    BARANGAYS ||--o{ VACCINES : "stores vaccines"

    USERS ||--o{ APPOINTMENTS : "books"
    HEALTH_SERVICES ||--o{ APPOINTMENTS : "selected for"
    HEALTH_RECORDS ||--o{ ARCHIVE_LOG : "logged when archived"

    BARANGAYS {
        int id PK
        varchar name UK
    }

    USERS {
        int id PK
        varchar resident_code
        varchar username UK
        varchar password
        varchar role
        int barangay_id FK
        varchar full_name
        varchar first_name
        varchar middle_name
        varchar last_name
        date birthday
        int age
        varchar gender
        varchar place_of_birth
        varchar civil_status
        varchar religion
        varchar citizenship
        varchar address
        varchar email
        varchar mobile_number
        boolean is_frozen
        boolean is_archived
    }

    HEALTH_RECORDS {
        int ID PK
        varchar resident_code
        varchar first_name
        varchar last_name
        varchar Diagnosis
        varchar Treatment
        varchar assigned_doc_name
        date Date
        varchar gender
        int age
        date birthday
        varchar violation
        boolean is_archived
        int barangay_id FK
    }

    HEALTH_SERVICES {
        int id PK
        varchar name
        text description
        varchar service_type
        varchar status
    }

    APPOINTMENTS {
        int id PK
        int user_id FK
        int service_id FK
        date appt_date
        varchar time_slot
        text notes
        varchar status
        varchar assigned_doc_name
        int barangay_id FK
        datetime created_at
    }

    MEDICINES {
        int id PK
        varchar medicine_code
        varchar name
        int quantity
        date expiry_date
        boolean is_archived
        int barangay_id FK
    }

    VACCINES {
        int id PK
        varchar vaccine_code
        varchar name
        int quantity
        date expiry_date
        boolean is_archived
        int barangay_id FK
    }

    ARCHIVE_LOG {
        int id PK
        int record_id FK
        datetime archived_at
    }

    RESIDENTS {
        int resident_id PK
        varchar first_name
        varchar last_name
        int age
        varchar gender
        date birthday
    }

    INVENTORY {
        int id PK
        varchar item_name
        int quantity
        varchar unit
        int threshold
    }
```

## Main Role Flow

```mermaid
flowchart TD
    A[loginAS] --> B[loginMayor]
    A --> C[loginLGU]
    A --> D[loginBCap]
    A --> E[loginRes]

    B --> F[MayorDash]
    F --> G[Resident Record by Barangay]
    F --> H[Resident Health Record by Barangay]
    F --> I[Barangay Health Report]
    F --> J[Inventory by Barangay]

    C --> K[TayudLGU Staff Dashboard]
    K --> L[Manage Health Records]
    K --> M[View Appointments]
    K --> N[Inventory]
    K --> O[Reports and Archives]

    D --> P[Barangay Captain Dashboard]
    P --> Q[Resident Records]
    P --> R[Resident Accounts]
    P --> S[Frozen Accounts]
    P --> T[Barangay Health Report]

    E --> U[Resident Dashboard]
    U --> V[Book Appointment]
    U --> W[View Health Record]
    U --> X[View Services]
```
