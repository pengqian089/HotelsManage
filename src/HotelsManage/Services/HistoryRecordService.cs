using HotelsManage.Enum;
using HotelsManage.Model;

namespace HotelsManage.Services;

/// <summary>
/// 历史记录
/// </summary>
public class HistoryRecordService : BasicService<HistoryRecord>
{
    public async Task AddHistoryAsync(HistoryRecord record)
    {
        await Repository.InsertAsync(record);
    }

    public async Task<bool> CheckRoomIdAsync(int roomId)
    {
        var record = await Repository.SearchFor(x => x.RoomId == roomId).FirstOrDefaultAsync();
        return record != null;
    }

    public async Task<bool> RoomCheckInAsync(int roomId)
    {
        var record = await Repository.SearchFor(x => x.RoomId == roomId && x.RecordStatus == RecordStatus.CheckIn)
            .FirstOrDefaultAsync();
        return record != null;
    }

    public async Task<HistoryRecord?> FindCheckInAsync(int roomId)
    {
        return await Repository.SearchFor(x => x.RoomId == roomId && x.RecordStatus == RecordStatus.CheckIn)
            .SingleOrDefaultAsync();
    }

    public async Task UpdateAsync(HistoryRecord historyRecord)
    {
        await Repository.UpdateAsync(historyRecord);
    }
}