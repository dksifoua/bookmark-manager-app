using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace bookmark_manager_app.Persistence.Interceptors;

public class LastModifiedTimeInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetLastModifiedTime(eventData);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetLastModifiedTime(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SetLastModifiedTime(DbContextEventData eventData)
    {
        var entries = eventData
            .Context?
            .ChangeTracker
            .Entries()
            .Where(x => x.State == EntityState.Modified);

        if (entries == null) return;

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseModel model)
            {
                model.UpdateLastModifiedTimeToNow();
            }
        }
    }
}