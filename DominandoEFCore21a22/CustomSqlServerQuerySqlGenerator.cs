using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace DominandoEFCore21a22;

public class CustomSqlServerQuerySqlGenerator : SqlServerQuerySqlGenerator
{
    public CustomSqlServerQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies, IRelationalTypeMappingSource typeMappingSource, ISqlServerSingletonOptions sqlServerSingletonOptions) : base(dependencies, typeMappingSource, sqlServerSingletonOptions)
    {
    }

    protected override Expression VisitTable(TableExpression tableExpression)
    {
        var table = base.VisitTable(tableExpression);
        Sql.Append(" WITH (NOLOCK)");

        return table;
    }
}

public class CustomSqlServerQuerySqlGeneratorFactory : SqlServerQuerySqlGeneratorFactory
{
    private readonly QuerySqlGeneratorDependencies _dependencies;
    private readonly IRelationalTypeMappingSource _typeMappingSource;
    private readonly ISqlServerSingletonOptions _sqlServerSingletonOptions;

    public CustomSqlServerQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies, IRelationalTypeMappingSource typeMappingSource, ISqlServerSingletonOptions sqlServerSingletonOptions) : base(dependencies, typeMappingSource, sqlServerSingletonOptions)
    {
        _dependencies = dependencies;
        _typeMappingSource = typeMappingSource;
        _sqlServerSingletonOptions = sqlServerSingletonOptions;
    }

    public override QuerySqlGenerator Create()
    {
        return new CustomSqlServerQuerySqlGenerator(_dependencies, _typeMappingSource, _sqlServerSingletonOptions);
    }
}