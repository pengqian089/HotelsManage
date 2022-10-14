using HotelsManage.Database;
using HotelsManage.Model;
using Model;

namespace HotelsManage.Services;

public class OccupantService : BasicService<Occupant>
{
    /// <summary>
    /// 登记房客
    /// </summary>
    /// <param name="occupant"></param>
    public async Task RegistrationAsync(Occupant occupant)
    {
        await Repository.InsertAsync(occupant);
    }

    public async Task<List<Occupant>> GetOccupantsAsync(ICollection<int> id)
    {
        return await Repository.SearchFor(x => id.Contains(x.Id)).ToListAsync();
    }

    public async Task<List<Occupant>> GetOccupantsAsync(string name)
    {
        return await Repository.SearchFor(x => x.Name != null && x.Name.Contains(name)).ToListAsync();
    }
    
    public async Task<Occupant?> GetOccupantAsync(string name)
    {
        return await Repository.SearchFor(x => x.Name != null && x.Name == name).FirstOrDefaultAsync();
    }

    public async Task<List<Occupant>> GetOccupantsAsync(int historyRecordId)
    {
        var repository = new Repository<HistoryRecord>();
        var history = await repository.FindAsync(historyRecordId);
        if (history == null)
            return new List<Occupant>();

        return await GetOccupantsAsync(history.OccupantId);
    }

    public async Task UpdateAsync(Occupant occupant)
    {
        await Repository.UpdateAsync(occupant);
    }
}