using HotelsManage.Services;
using HotelsManage.ViewModel;

namespace HotelsManage.Pages;

public partial class Index
{
    private readonly RoomService _roomService = new RoomService();

    private IEnumerable<RoomDetail> _source;

    protected override async Task OnInitializedAsync()
    {
        await LoadRoomDataAsync();
        await base.OnInitializedAsync();
    }

    private async Task LoadRoomDataAsync()
    {
        _source = await _roomService.GetRoomDetailsAsync().ToListAsync();
    }

    private void ShowOccupant(RoomDetail roomDetail)
    {
        roomDetail.ShowOccupant = !roomDetail.ShowOccupant;
    }
}