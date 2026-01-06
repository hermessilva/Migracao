using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public abstract class BaseEntityConfiguration
{  /// <summary>
   /// Provider de banco de dados atual. Pode ser configurado via variável de ambiente ou configuração.
   /// Default: MySQL (banco de produção)
   /// </summary>
    protected static DatabaseProvider CurrentProvider { get; private set; } = DatabaseProvider.MySql;

    /// <summary>
    /// Configura o provider de banco de dados globalmente para todas as configurations.
    /// Deve ser chamado na inicialização da aplicação antes do DbContext ser criado.
    /// </summary>
    /// <param name="provider">Provider de banco de dados a ser utilizado</param>
    public static void SetDatabaseProvider(DatabaseProvider provider)
    {
        CurrentProvider = provider;
    }

    /// <summary>
    /// Detecta automaticamente o provider baseado na variável de ambiente ou connection string.
    /// </summary>
    public static void AutoDetectProvider()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var dbProvider = Environment.GetEnvironmentVariable("DB_PROVIDER");

        if (dbProvider?.Equals("sqlite", StringComparison.OrdinalIgnoreCase) == true || environment?.Equals("Testing", StringComparison.OrdinalIgnoreCase) == true)
            CurrentProvider = DatabaseProvider.Sqlite;
        else
            CurrentProvider = DatabaseProvider.MySql;
    }
}

/// <summary>
/// Classe base abstrata para configurações de entidades do Entity Framework.
/// Fornece métodos auxiliares para mapeamento de tipos de dados compatíveis com MySQL e SQLite.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade a ser configurada</typeparam>
public abstract class BaseEntityConfiguration<TEntity> : BaseEntityConfiguration, IEntityTypeConfiguration<TEntity>
    where TEntity : class
{

    /// <summary>
    /// Método abstrato que deve ser implementado pelas classes derivadas para configurar a entidade.
    /// </summary>
    /// <param name="builder">Builder de configuração da entidade</param>
    public abstract void Configure(EntityTypeBuilder<TEntity> builder);

    #region Tipos Numéricos

    /// <summary>
    /// Retorna o tipo de coluna para DECIMAL com precisão e escala especificadas.
    /// MySQL: decimal(p,s) | SQLite: REAL
    /// </summary>
    /// <param name="precision">Precisão total (número de dígitos)</param>
    /// <param name="scale">Escala (número de casas decimais)</param>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string Decimal(int precision, int scale)
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => $"decimal({precision},{scale})",
            DatabaseProvider.Sqlite => "REAL",
            _ => $"decimal({precision},{scale})"
        };
    }

    /// <summary>
    /// Retorna o tipo de coluna para inteiros pequenos (2 bytes).
    /// MySQL: smallint | SQLite: INTEGER
    /// </summary>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string SmallInt()
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => "smallint",
            DatabaseProvider.Sqlite => "INTEGER",
            _ => "smallint"
        };
    }

    /// <summary>
    /// Retorna o tipo de coluna para inteiros.
    /// MySQL: int | SQLite: INTEGER
    /// </summary>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string Int()
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => "int",
            DatabaseProvider.Sqlite => "INTEGER",
            _ => "int"
        };
    }

    /// <summary>
    /// Retorna o tipo de coluna para booleanos representados como tinyint(1).
    /// MySQL: tinyint(1) | SQLite: INTEGER
    /// </summary>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string TinyIntBoolean()
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => "tinyint(1)",
            DatabaseProvider.Sqlite => "INTEGER",
            _ => "tinyint(1)"
        };
    }

    /// <summary>
    /// Retorna o tipo de coluna para inteiros muito pequenos (1 byte).
    /// MySQL: tinyint | SQLite: INTEGER
    /// </summary>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string TinyInt()
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => "tinyint",
            DatabaseProvider.Sqlite => "INTEGER",
            _ => "tinyint"
        };
    }

    #endregion

    #region Tipos de Texto

    /// <summary>
    /// Retorna o tipo de coluna para CHAR com tamanho fixo.
    /// MySQL: char(n) | SQLite: TEXT
    /// </summary>
    /// <param name="length">Tamanho fixo do campo</param>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string Char(int length)
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => $"char({length})",
            DatabaseProvider.Sqlite => "TEXT",
            _ => $"char({length})"
        };
    }

    /// <summary>
    /// Retorna o tipo de coluna para VARCHAR.
    /// MySQL: varchar(n) | SQLite: TEXT
    /// </summary>
    /// <param name="length">Tamanho máximo do campo</param>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string VarChar(int length)
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => $"varchar({length})",
            DatabaseProvider.Sqlite => "TEXT",
            _ => $"varchar({length})"
        };
    }

    /// <summary>
    /// Retorna o tipo de coluna para MEDIUMBLOB (dados binários até 16MB).
    /// MySQL: mediumblob | SQLite: BLOB
    /// </summary>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string MediumBlob()
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => "mediumblob",
            DatabaseProvider.Sqlite => "BLOB",
            _ => "mediumblob"
        };
    }

    #endregion

    #region Tipos ENUM

    /// <summary>
    /// Retorna o tipo de coluna para ENUM (apenas retorna a string do tipo).
    /// MySQL: enum('valor1','valor2',...) | SQLite: TEXT
    /// Use ConfigureEnum para configuração completa com MaxLength.
    /// </summary>
    /// <param name="values">Valores permitidos para o enum</param>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string Enum(params string[] values)
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => $"enum({string.Join(",", values.Select(v => $"'{v}'"))})",
            DatabaseProvider.Sqlite => "TEXT",
            _ => $"enum({string.Join(",", values.Select(v => $"'{v}'"))})"
        };
    }

    /// <summary>
    /// Configura uma propriedade ENUM com tipo de coluna e MaxLength apropriados.
    /// MySQL: enum('valor1','valor2',...) | SQLite: TEXT com MaxLength baseado no maior valor
    /// </summary>
    /// <typeparam name="TProperty">Tipo da propriedade</typeparam>
    /// <param name="propertyBuilder">Builder da propriedade a ser configurada</param>
    /// <param name="values">Valores permitidos para o enum</param>
    /// <returns>PropertyBuilder para encadeamento fluente</returns>
    protected PropertyBuilder<TProperty> ConfigureEnum<TProperty>(
        PropertyBuilder<TProperty> propertyBuilder,
        params string[] values)
    {
        if (values == null || values.Length == 0)
            throw new ArgumentException("Deve ser informado pelo menos um valor para o enum.", nameof(values));

        var maxLength = values.Max(v => v.Length);

        return CurrentProvider switch
        {
            DatabaseProvider.MySql => propertyBuilder
                .HasColumnType($"enum({string.Join(",", values.Select(v => $"'{v}'"))})"),

            DatabaseProvider.Sqlite => propertyBuilder
                .HasColumnType("TEXT")
                .HasMaxLength(maxLength),

            _ => propertyBuilder
                .HasColumnType($"enum({string.Join(",", values.Select(v => $"'{v}'"))})")
        };
    }

    /// <summary>
    /// Configura uma propriedade ENUM com tipo de coluna, MaxLength e CHECK constraint para SQLite.
    /// MySQL: enum('valor1','valor2',...) | SQLite: TEXT com MaxLength e CHECK constraint
    /// </summary>
    /// <typeparam name="TProperty">Tipo da propriedade</typeparam>
    /// <param name="propertyBuilder">Builder da propriedade a ser configurada</param>
    /// <param name="columnName">Nome da coluna para uso no CHECK constraint</param>
    /// <param name="values">Valores permitidos para o enum</param>
    /// <returns>PropertyBuilder para encadeamento fluente</returns>
    protected PropertyBuilder<TProperty> ConfigureEnumWithCheck<TProperty>(
        PropertyBuilder<TProperty> propertyBuilder,
        string columnName,
        params string[] values)
    {
        if (values == null || values.Length == 0)
            throw new ArgumentException("Deve ser informado pelo menos um valor para o enum.", nameof(values));

        var maxLength = values.Max(v => v.Length);

        return CurrentProvider switch
        {
            DatabaseProvider.MySql => propertyBuilder
                .HasColumnType($"enum({string.Join(",", values.Select(v => $"'{v}'"))})"),

            DatabaseProvider.Sqlite => propertyBuilder
                .HasColumnType("TEXT")
                .HasMaxLength(maxLength),
            // CHECK constraint pode ser adicionado via migration manual ou HasCheckConstraint se disponível

            _ => propertyBuilder
                .HasColumnType($"enum({string.Join(",", values.Select(v => $"'{v}'"))})")
        };
    }

    #endregion

    #region Tipos de Data/Hora

    /// <summary>
    /// Retorna o tipo de coluna para DATE.
    /// MySQL: date | SQLite: TEXT (formato ISO8601)
    /// </summary>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string Date()
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => "date",
            DatabaseProvider.Sqlite => "TEXT",
            _ => "date"
        };
    }

    /// <summary>
    /// Retorna o tipo de coluna para DATETIME.
    /// MySQL: datetime | SQLite: TEXT (formato ISO8601)
    /// </summary>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string DateTime()
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => "datetime",
            DatabaseProvider.Sqlite => "TEXT",
            _ => "datetime"
        };
    }

    /// <summary>
    /// Retorna o tipo de coluna para DATETIME com precisão de fração de segundo.
    /// MySQL: datetime(n) | SQLite: TEXT
    /// </summary>
    /// <param name="precision">Precisão da fração de segundo (0-6)</param>
    /// <returns>String do tipo de coluna apropriado para o provider</returns>
    protected string DateTimeWithPrecision(int precision = 6)
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => $"datetime({precision})",
            DatabaseProvider.Sqlite => "TEXT",
            _ => $"datetime({precision})"
        };
    }

    #endregion

    #region Valores Default

    /// <summary>
    /// Retorna a expressão SQL para timestamp atual.
    /// MySQL: CURRENT_TIMESTAMP(6) | SQLite: datetime('now')
    /// </summary>
    /// <returns>Expressão SQL para timestamp atual</returns>
    protected string CurrentTimestamp()
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => "CURRENT_TIMESTAMP(6)",
            DatabaseProvider.Sqlite => "datetime('now')",
            _ => "CURRENT_TIMESTAMP(6)"
        };
    }

    /// <summary>
    /// Retorna a expressão SQL para timestamp atual com precisão específica.
    /// MySQL: CURRENT_TIMESTAMP(n) | SQLite: datetime('now')
    /// </summary>
    /// <param name="precision">Precisão da fração de segundo (0-6), usado apenas no MySQL</param>
    /// <returns>Expressão SQL para timestamp atual</returns>
    protected string CurrentTimestampWithPrecision(int precision = 6)
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => $"CURRENT_TIMESTAMP({precision})",
            DatabaseProvider.Sqlite => "datetime('now')",
            _ => $"CURRENT_TIMESTAMP({precision})"
        };
    }

    /// <summary>
    /// Retorna a expressão SQL para data atual.
    /// MySQL: CURRENT_DATE | SQLite: date('now')
    /// </summary>
    /// <returns>Expressão SQL para data atual</returns>
    protected string CurrentDate()
    {
        return CurrentProvider switch
        {
            DatabaseProvider.MySql => "CURRENT_DATE",
            DatabaseProvider.Sqlite => "date('now')",
            _ => "CURRENT_DATE"
        };
    }

    #endregion
}
public enum DatabaseProvider
{
    MySql,
    Sqlite
}