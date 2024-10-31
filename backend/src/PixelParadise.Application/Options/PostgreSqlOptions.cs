namespace PixelParadise.Application.Options;

/// <summary>
/// Configuration settings for PostgreSQL database connection.
/// </summary>
public class PostgreSqlOptions
{
    /// <summary>
    /// The hostname or IP address of the PostgreSQL server.
    /// </summary>
    public required string Host { get; init; }

    /// <summary>
    /// The username for connecting to the PostgreSQL database.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// The password for connecting to the PostgreSQL database.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// The port number for connecting to the PostgreSQL server.
    /// </summary>
    public required int Port { get; init; }

    /// <summary>
    /// The name of the PostgreSQL database to connect to.
    /// </summary>
    public required string Database { get; init; }

    /// <summary>
    /// Constructs the connection string for PostgreSQL.
    /// </summary>
    public string GetConnectionString =>
        $"Server={Host};Database={Database};Port={Port};User Id={Username};Password={Password};";
}