using HotelsManage.Database;
using HotelsManage.Enum;
using HotelsManage.Model;
using HotelsManage.ViewModel;
using Model;

namespace HotelsManage.Services;

public class RoomService : BasicService<Room>
{
    public async Task<Room?> GetAsync(int id)
    {
        return await Repository.FindAsync(id);
    }

    public async IAsyncEnumerable<RoomDetail> GetRoomDetailsAsync()
    {
        var list = await Repository.SearchFor(x => true).ToListAsync();
        var repository = new Repository<HistoryRecord>();

        var occupantService = new OccupantService();

        //var ids = list.Select(x => x.Id).ToList();
        var histories = await repository.Collection.Query()
            .Where(x => x.RecordStatus == RecordStatus.CheckIn).ToListAsync();


        foreach (var item in list)
        {
            var history = histories.FirstOrDefault(y => y.RoomId == item.Id);
            yield return new RoomDetail
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Status = item.Status,
                RoomType = item.RoomType,
                HistoryRecordId = history?.Id,
                Occupants = history == null
                    ? null
                    : await occupantService.GetOccupantsAsync(history.OccupantId),
                ActualPrice = history?.Price,
                Deposit = history?.Deposit,
                DepositStatus = history?.DepositStatus,
                RecordStatus = history?.RecordStatus,
                CheckInTime = history?.CheckInTime,
                DepartureTime = history?.DepartureTime
            };
        }
    }

    /// <summary>
    /// 添加房间
    /// </summary>
    /// <param name="room"></param>
    public async Task AddRoomAsync(Room room)
    {
        await Repository.InsertAsync(room);
    }

    /// <summary>
    /// 修改房间信息
    /// </summary>
    /// <param name="room"></param>
    public async Task EditRoomAsync(Room room)
    {
        await Repository.UpdateAsync(room);
    }

    /// <summary>
    /// 删除房间
    /// </summary>
    /// <param name="room"></param>
    /// <exception cref="BusinessException"></exception>
    public async Task DeleteRoomAsync(Room room)
    {
        var result = await new HistoryRecordService().CheckRoomIdAsync(room.Id);
        if (result)
            throw new BusinessException("房间与历史记录还有关联，不能删除");

        await Repository.DeleteAsync(room.Id);
    }

    /// <summary>
    /// 获取所有房间
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Room>> GetRooms()
    {
        return await Repository.SearchFor(x => true).ToListAsync();
    }

    /// <summary>
    /// 开房
    /// </summary>
    /// <param name="roomId">房间ID</param>
    /// <param name="price">单价/天</param>
    /// <param name="deposit">押金</param>
    /// <param name="depositStatus">押金</param>
    /// <param name="occupants">房客</param>
    public async Task OpenRoom(int roomId, decimal price, decimal deposit, DepositStatus depositStatus,
        List<Occupant> occupants)
    {
        var room = await Repository.FindAsync(roomId);

        if (room == null)
            throw new BusinessException("没有找到房间");

        if (room.Status != RoomStatus.Empty)
            throw new BusinessException("当前房间状态不能开房");

        var historyRecordService = new HistoryRecordService();

        var result = await historyRecordService.RoomCheckInAsync(roomId);
        if (result)
            throw new BusinessException("当前房号有未完成的入住记录，不可以开房");

        var occupantService = new OccupantService();
        foreach (var occupant in occupants)
        {
            await occupantService.RegistrationAsync(occupant);
        }

        var history = new HistoryRecord
        {
            RoomId = roomId,
            CheckInTime = DateTime.Now,
            DepartureTime = null,
            Deposit = deposit,
            DepositStatus = depositStatus,
            Name = room.Name,
            OccupantId = occupants.Select(x => x.Id).ToList(),
            Price = price,
            RecordStatus = RecordStatus.CheckIn,
            RoomType = room.RoomType
        };
        await historyRecordService.AddHistoryAsync(history);

        room.Status = RoomStatus.CheckIn;
        await Repository.UpdateAsync(room);
    }

    /// <summary>
    /// 退房
    /// </summary>
    public async Task RoomCheckOut(int roomId, DepositStatus depositStatus)
    {
        var room = await Repository.FindAsync(roomId);

        if (room == null)
            throw new BusinessException("没有找到房间");

        var historyRecordService = new HistoryRecordService();
        var record = await historyRecordService.FindCheckInAsync(roomId);
        if (record == null)
            throw new BusinessException("没有查找到开房记录，请检查");

        record.DepositStatus = depositStatus;
        record.DepartureTime = DateTime.Now;
        record.RecordStatus = RecordStatus.Complete;

        await historyRecordService.UpdateAsync(record);

        room.Status = RoomStatus.Empty;
        await Repository.UpdateAsync(room);
    }

    /// <summary>
    /// 续费
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="price"></param>
    /// <exception cref="BusinessException"></exception>
    public async Task LeaseRoom(int roomId,decimal price)
    {
        var room = await Repository.FindAsync(roomId);

        if (room == null)
            throw new BusinessException("没有找到房间");

        var historyRecordService = new HistoryRecordService();
        var record = await historyRecordService.FindCheckInAsync(roomId);
        if (record == null)
            throw new BusinessException("没有查找到开房记录，请检查");

        record.Price += price;
        
        await historyRecordService.UpdateAsync(record);
    }
}