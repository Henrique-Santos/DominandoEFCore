using DominandoEFCore17.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore17.ModelFactory
{
    //public class StrategySchemaModelCacheKey : IModelCacheKeyFactory
    //{
    //    //Sempre que o contexto for instanceado irá obter esse modelo como o Schema definido
    //    public object Create(DbContext context, bool designTime)
    //    {
    //        var model = new
    //        {
    //            Type = context.GetType(),
    //            Schema = (context as ApplicationDbContext)?.TenantData.TenantId
    //        };

    //        return model;
    //    }
    //}
}