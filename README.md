# CompanyManager With WebApi

**Lernziele:**

- Wie mit einer REST-Api Daten aus der Datenbank abgerufen werden.

**Hinweis:** Als Startpunkt wird die Vorlage [CompanyManagerWithSqlite](https://github.com/leoggehrer/CompanyManagerWithSqlite) verwendet.

## Vorbereitung

Bevor mit der Umsetzung begonnen wird, sollte die Vorlage heruntergeladen und die Funktionalität verstanden werden.

### WebApi-Projekt erstellen

- Erstellen Sie ein neues Projekt vom Typ **ASP.NET Core Web API** und vergeben Sie den Namen **CompanyManager.WebApi**.
- Verbinden Sie das Projekt **CompanyManager.WebApi** mit dem Projekt **CompanyManager.Logic**.

### Erstellen der Models

Erstellen Sie im Projekt **CompanyManager.WebApi** einen Ordner **Models** und fügen Sie die Klassen **Company**, **Customer** und **Employee** hinzu.

Nachfolgend ein Beispiel für das **Company**-Model:

```csharp
/// <summary>
/// Represents a company entity.
/// </summary>
public class Company : ModelObject, Logic.Contracts.ICompany
{
    /// <summary>
    /// Gets or sets the name of the company.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the company.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the description of the company.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Copies the properties from another company instance.
    /// </summary>
    /// <param name="other">The company instance to copy properties from.</param>
    public virtual void CopyProperties(Logic.Contracts.ICompany other)
    {
        base.CopyProperties(other);

        Name = other.Name;
        Address = other.Address;
        Description = other.Description;
    }

    /// <summary>
    /// Creates a new company instance from an existing company.
    /// </summary>
    /// <param name="company">The company instance to copy properties from.</param>
    /// <returns>A new company instance.</returns>
    public static Company Create(Logic.Contracts.ICompany company)
    {
        var result = new Company();

        result.CopyProperties(company);
        return result;
    }
}
```

Diese Implementierung kann als Vorlage für alle anderen Models verwendet werden.

**Erläuterung:**

Die abstrakte Klasse `ModelObject` ist die Basisklasse für alle Models. Es beinhaltet die Eigenschaft `Id` (diese Eigenschaft stellen alle Models bereitstellen) und eine Methode `public virtual void CopyProperties(IIdentifiable other)`.
Die Klasse **Company** erbt die Eigenschaften und Methoden der Klasse `ModelObject` und ergänzt diese um weitere Eigenschaften und Methoden. Die Methoden `public virtual void CopyProperties(ICompany company)`und das Überschreiben der Methode `public override ToString()` ist für die Entität nicht erforderlich, sind aber im Verlauf für die weitere Entwicklung hilfreich.

```csharp
/// <summary>
/// Represents an abstract base class for model objects that are identifiable.
/// </summary>
public abstract class ModelObject : Logic.Contracts.IIdentifiable
{
    /// <summary>
    /// Gets or sets the unique identifier for the model object.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Copies the properties from another identifiable object.
    /// </summary>
    /// <param name="other">The other identifiable object to copy properties from.</param>
    /// <exception cref="ArgumentNullException">Thrown when the other object is null.</exception>
    public virtual void CopyProperties(Logic.Contracts.IIdentifiable other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        Id = other.Id;
    }
}
```

### Erstellen des DataContext

Der **DbContext** ist die Hauptklasse, die mit der Datenbank kommuniziert. Es ist eine Kombination aus Unit of Work und Repository-Mustern. Es kann als eine Sammlung von Entitäten betrachtet werden, die in einer Datenbank gespeichert sind. Der **DbContext** ist verantwortlich für das Abrufen, Speichern, Aktualisieren und Löschen von Entitäten aus der Datenbank.

Erweitern Sie die vorhandene Schnittstelle `IContext` um folgende Eigenschaften:

```csharp
public interface IContext : IDisposable
{
    DbSet<Entities.Company> CompanySet { get; }
    DbSet<Entities.Customer> CustomerSet { get; }
    DbSet<Entities.Employee> EmployeeSet { get; }

    int SaveChanges();
}
```

Im nächsten Schritt wird die Kontext-Klasse erstellt. Wir nennen die Klasse `CompanyContext` und erweitern sie um die Klasse `DbContext` aus dem Namespace `Microsoft.EntityFrameworkCore`.

```csharp
internal class CompanyContext : DbContext, IContext
{
    #region fields
    private static string ConnectionString = "data source=CompanyManager.db";
    #endregion fields

    public DbSet<Entities.Company> CompanySet { get; set; }
    public DbSet<Entities.Customer> CustomerSet { get; set; }
    public DbSet<Entities.Employee> EmployeeSet { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConnectionString);

        base.OnConfiguring(optionsBuilder);
    }
}
```

Überschreiben Sie in der Klasse `CompanyContext` die Methode `OnConfiguring`, um die Verbindung zur Datenbank herzustellen. In diesem Fall wird die Datenbank **SQLite** verwendet. Die Verbindungsinformationen werden in der statischen Variable `ConnectionString` gespeichert.

### Erstellen der Datenbank

Das Erstellen der Datenbank erfolgt über den **CodeFirst**-Ansatz. Dies bedeutet, dass die Datenbank aus den Entitäten erstellt wird. Der **DbContext** ist für das Erstellen und Aktualisieren der Datenbank verantwortlich. Die einzelnen Schritte sind von der verwendeten IDE abhängig. Eine Anleitung zum Erstellen der Datenbank finden Sie [hier](https://github.com/leoggehrer/Slides/tree/main/EFCreateDatabase).

### Erstellen der Factory-Klassen

Für den Zugriff auf den Database-Kontext wird eine Factory-Klasse benötigt. Erstellen Sie die Klasse `Factory` im Ordner *DataContext*.

```csharp
/// <summary>
/// Factory class to create instances of IMusicStoreContext.
/// </summary>
public static class Factory
{
    /// <summary>
    /// Creates an instance of IContext.
    /// </summary>
    /// <returns>An instance of IContext.</returns>
    public static IContext CreateContext()
    {
        var result = new CompanyContext();

        result.Database.EnsureCreated();

        return result;
    }
}
```

### Testen des Systems

Im Konsolen Programm ist bereits ein Menü zum Testen der Funktionalität implementiert. Erweitern Sie das Menü um die Funktionalitäten für die Entitäten **Company**,  **Customer** und **Employee**.

Fügen Sie das Package `System.Linq.Dynamic.Core` hinzu, um Zeichenfolgen (strings) in  LINQ-Abfragen zu verwenden. Das Hinzufügen des Packages erfolgt im Konsole-Programm und die Anleitung dazu finden Sie [hier](https://github.com/leoggehrer/Slides/tree/main/NutgetInstall).

## Hilfsmitteln

- Foliensätze

## Abgabe

- Termin: 1 Woche nach der Ausgabe
- Klasse:
- Name:

## Quellen

- keine Angabe

> **Viel Erfolg!**
