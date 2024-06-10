using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DominandoEFCore11.Interceptadores;

public class InterceptadorDePersistencia : SaveChangesInterceptor
{
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        Console.WriteLine(eventData.Context.ChangeTracker.DebugView.LongView);

        return result;
    }
}