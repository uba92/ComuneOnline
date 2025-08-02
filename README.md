# ComuneOnline Project Summary
Progetto di apprendimento di ASP.NET Core MVC con Entity Framework Core e PostgreSQL.
# Obiettivo:
Simulare una mini-app basata su un'app reale sviluppata per i Comuni.

# 🧾 Riepilogo attività – Progetto "ComuneOnline"  
**Obiettivo:** Imparare ASP.NET Core MVC con EF Core + PostgreSQL, simulando un'app reale come quella sviluppata da Wemapp per i Comuni.

## 📅 Giornate di lavoro: 29–30 Luglio

---

## 🧱 1. Setup iniziale del progetto

### ✅ Organizzazione della repository Git
- **Scelta consapevole:** inizializzare il repository **in locale**, poi collegarlo a GitHub.
- **Comandi eseguiti:**
  ```bash
  mkdir ComuneOnline
  cd ComuneOnline
  git init
  git branch -M main  # rinomina "master" in "main"
  touch README.md
  git add .
  git commit -m "init repo with README"
  git remote add origin https://github.com/uba92/ComuneOnline.git
  git push -u origin main
  ```

---

## ⚙️ 2. Creazione del progetto ASP.NET Core MVC

- Scelto il template `mvc`:
  ```bash
  dotnet new mvc -n ComuneOnline
  ```
- Questo comando ha generato:
  - `Program.cs`, `Startup.cs` (a seconda della versione .NET)
  - Cartelle `Controllers`, `Views`, `Models`, ecc.
- Fatto un nuovo commit:
  ```bash
  git add .
  git commit -m "add asp.net core mvc structure"
  git push
  ```

---

## 💻 3. Passaggio a Visual Studio

- Aperto il progetto `.sln` da Visual Studio
- Verificato il corretto funzionamento con una **compilazione**:
  ```
  Compilazione riuscita – 0 errori, 0 warning
  ```

---

## 📦 4. Aggiunta di Entity Framework Core + PostgreSQL

- Aggiunti pacchetti via NuGet:
  - `Microsoft.EntityFrameworkCore`
  - `Microsoft.EntityFrameworkCore.Tools`
  - `Npgsql.EntityFrameworkCore.PostgreSQL`
- Confermata installazione corretta

---

## 🛢️ 5. Connessione a PostgreSQL

- **Obiettivo:** collegare l'app al DB PostgreSQL locale
- Trovato il nome server tramite `pgAdmin` → es: `localhost`, porta `5432`
- Ragionato sulla ConnectionString:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ComuneDb;Username=postgres;Password=tuapassword"
  }
  ```
- Discutiuto su parametri extra (`TrustServerCertificate`, `SSL Mode`, ecc.)

---

## 🔍 6. Gestione corretta del versionamento

### ❌ Problema:
- Git dava errore nel versionare `.vs/` e `obj/`

### ✅ Soluzione:
- Creato un file `.gitignore`:
  ```bash
  touch .gitignore
  ```
  Con contenuti tipo:
  ```
  .vs/
  bin/
  obj/
  *.user
  *.suo
  ```

- Comandi successivi:
  ```bash
  git add .
  git commit -m "add .gitignore and installed EF"
  git push
  ```

---

## 🚦 Stato attuale del progetto

| Componente                      | Stato    |
|--------------------------------|----------|
| Repo Git inizializzata         | ✅        |
| Struttura MVC creata           | ✅        |
| Visual Studio operativo        | ✅        |
| EF Core + PostgreSQL installati| ✅        |
| ConnectionString inserita      | ✅        |
| .gitignore funzionante         | ✅        |
| Ultimo push aggiornato         | ✅        |

---

### ✅ Giorno 3: Creazione del DbContext

Ho creato una nuova cartella chiamata `Data`, che ospiterà tutto ciò che riguarda l'accesso al database.

All'interno ho aggiunto la classe `ComuneDbContext`, che rappresenta il punto di contatto tra la mia applicazione e il database
PostgreSQL tramite Entity Framework Core.

Questa classe estende `DbContext`, la classe base fornita da EF Core.  
Ho anche aggiunto un costruttore che accetta un oggetto `DbContextOptions` e lo passa alla classe base. 
Questo è necessario per permettere a ASP.NET Core di iniettare automaticamente le opzioni, come la connection string.

+-------------------------+
|      ComuneDbContext    |
|-------------------------|
|  Ha bisogno di sapere   |
|  - Dove si trova il DB  |
|  - Che provider usare   |
|  - Come gestire i dati  |
+-------------------------+
           ▲
           │
           │  (le opzioni gli arrivano già pronte)
           │
+-------------------------+
|   DbContextOptions      |
|-------------------------|
|  Contiene la connection |
|  string, provider (es.  |
|  PostgreSQL), regole... |
+-------------------------+
```

Queste opzioni **non vengono scritte dentro al contesto stesso**, ma configurate altrove (nel file `Program.cs`) e 
passate **automaticamente** da ASP.NET Core, così il codice rimane più pulito e modulare.

---

```csharp
// Esempio in Program.cs
builder.Services.AddDbContext<ComuneDbContext>(options =>
    options.UseNpgsql("Host=...;Database=...;Username=...;Password=...")
);
```

In questo modo, l'applicazione può collegarsi al database ogni volta che serve — sfruttando il `ComuneDbContext` — 
senza bisogno di hardcoded o logiche interne difficili da gestire.

---

Per ora il contesto è vuoto, ma sarà qui che andrò ad aggiungere i vari `DbSet<T>` che rappresentano le tabelle del database.

Esempio futuro:
- `DbSet<Cittadino> Cittadini` → per memorizzare i cittadini
- `DbSet<Domanda> Domande` → per le istanze inviate

Questo passaggio mi ha aiutato a capire dove mettere il codice relativo ai dati e come strutturare il cuore 
del collegamento tra codice e database.

---

### ✅ Giorno 4: Prima entità, migration e factory design-time

#### 🔸 1. Creazione della prima entità: `Cittadino`

- Ho creato il file `Cittadino.cs` nella cartella `Models/Entities`.
- Per indicare a EF Core di usare il nome della tabella in **minuscolo**, ho aggiunto l'attributo `[Table("cittadini")]` sopra alla classe.

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComuneOnline.Models.Entities
{
    [Table("cittadini")]
    public class Cittadino
    {
        [Key]
        public int CittadinoId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cognome { get; set; }

        public DateTime DataNascita { get; set; }
    }
}
