using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DominandoEFCore19.Extensions;

public static partial class SnakeCaseExtensions
{
    public static void ToSnakeCaseNames(this ModelBuilder modelBuilder)
    {
        // Percorrendo todas as entidades mapeadas
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Convertendo o nome da entidade usando a regex
            var tableName = entity.GetTableName().ToSnakeCase();
            // Alterando o nome da entidade
            entity.SetTableName(tableName);

            foreach (var property in entity.GetProperties())
            {
                var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName, null);

                var columnName = property.GetColumnName(storeObjectIdentifier).ToSnakeCase();

                property.SetColumnName(columnName);
            }

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName().ToSnakeCase();
                key.SetName(keyName);
            }

            foreach (var key in entity.GetForeignKeys())
            {
                var foreignKeyName = key.GetConstraintName().ToSnakeCase();
                key.SetConstraintName(foreignKeyName);
            }

            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName().ToSnakeCase();
                index.SetDatabaseName(indexName);
            }
        }
    }

    // UserId -> user_id
    private static string ToSnakeCase(this string name) => SnakeCase().Replace(name, "$1_$2").ToLower();

    [GeneratedRegex(@"([a-z0-9])([A-Z])")]
    private static partial Regex SnakeCase();
}