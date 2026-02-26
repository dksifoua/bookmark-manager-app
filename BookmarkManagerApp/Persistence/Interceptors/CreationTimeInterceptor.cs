using BookmarkManagerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookmarkManagerApp.Persistence.Interceptors;

public class CreationTimeInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetCreationTime(eventData);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetCreationTime(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SetCreationTime(DbContextEventData eventData)
    {
        var entries = eventData
            .Context?
            .ChangeTracker
            .Entries()
            .Where(x => x.State == EntityState.Added);

        if (entries == null) return;

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseModel model)
            {
                model.SetCreationTimeToNow();
            }
        }
    }
}