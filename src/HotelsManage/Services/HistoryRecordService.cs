using HotelsManage.Database;
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
    
    /// <summary>
    /// 续费
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="price"></param>
    /// <exception cref="BusinessException"></exception>
    public async Task LeaseRoom(int roomId, decimal price)
    {
        var roomRepository = new Repository<Room>();
        var room = await roomRepository.FindAsync(roomId);

        if (room == null)
            throw new BusinessException("没有找到房间");

        var record = await FindCheckInAsync(roomId);
        if (record == null)
            throw new BusinessException("没有查找到开房记录，请检查");

        record.Price += price;

        await UpdateAsync(record);
    }
    
    public async Task<List<HistoryRecord>> GetHistoryRecordsAsync(DateTime start, DateTime end)
    {
        return await Repository.SearchFor(x =>
            x.RecordStatus == RecordStatus.Complete && x.CheckInTime >= start && x.CheckInTime <= end).ToListAsync();
    }

    public async Task UpdateDepositAsync(int roomId,decimal deposit)
    {
        if (deposit < 0)
        {
            throw new BusinessException("押金不能小于0");
        }

        var roomRepository = new Repository<Room>();
        var room = await roomRepository.FindAsync(roomId);

        if (room == null)
            throw new BusinessException("没有找到房间");

        if (room.Status != RoomStatus.CheckIn)
        {
            throw new BusinessException("没有入住的房间不能修改押金");
        }

        var record = await FindCheckInAsync(roomId);
        if (record == null)
            throw new BusinessException("没有查找到开房记录，请检查");

        record.Deposit = deposit;

        await UpdateAsync(record);

    }
}