using HotelsManage.Enum;
using HotelsManage.Services;
using HotelsManage.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using MudBlazor;
using DialogResult = MudBlazor.DialogResult;


namespace HotelsManage.Shared.Component;

public partial class CheckInRegisterForm
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public int RoomId { get; set; }

    [Parameter]
    public int? RecordId { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private ILogger<CheckInRegisterForm> Logger { get; set; }

    private string _title = "登记入住";

    private bool _isPost = false;

    private MudForm _form;

    private CheckInRegister _model = new();

    private CheckInRegisterFluentValidator _validator = new();

    protected override async Task OnParametersSetAsync()
    {
        if (RecordId == null)
        {
            var service = new RoomService();
            var room = await service.GetAsync(RoomId);
            if (room == null)
            {
                Snackbar.Add("房间不存在", Severity.Error);
                MudDialog.Cancel();
                return;
            }
            _model.Price = room.Price;
        }
        else
        {
            var service = new HistoryRecordService();
            var registerInfo = await service.GetRegisterInformationAsync(RecordId.Value);
            if (registerInfo == null)
            {
                Snackbar.Add("记录不存在", Severity.Error);
                MudDialog.Cancel();
                return;
            }
            _title = "修改登记信息";
            _model = registerInfo;
        }
        await base.OnParametersSetAsync();
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task SaveRegisterInfoAsync()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            _isPost = true;
            try
            {
                if (RecordId == null)
                {
                    var roomService = new RoomService();
                    var depositStatus = _model.Deposit == 0m ? DepositStatus.NotCharged : DepositStatus.Pay;
                    await roomService.OpenRoom(RoomId, _model.Price, _model.Deposit, _model.Count, depositStatus,
                        _model.ToOccupantList());
                }
                else
                {
                    var historyRecordService = new HistoryRecordService();
                    await historyRecordService.UpdateRegisterInformationAsync(_model);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
                Logger.LogError(e,"save check-in register information fail");
                _isPost = false;
            }
            
            _isPost = false;
            MudDialog.Close(DialogResult.Ok(true));
        }
    }

    private async Task<IEnumerable<string?>> SearchOccupantAsync(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Array.Empty<string>();
        }
        var service = new OccupantService();
        var list = await service.GetOccupantsAsync(value);
        return list.Select(x => x.Name).ToList();
    }

    private async Task SetOccupantAsync(FocusEventArgs e)
    {
        if (!string.IsNullOrEmpty(_model.Name))
        {
            var service = new OccupantService();
            var occupant = await service.GetOccupantAsync(_model.Name);
            if (occupant != null)
            {
                if (string.IsNullOrEmpty(_model.Sex))
                    _model.Sex = occupant.Sex;
                if (string.IsNullOrEmpty(_model.IdCard))
                    _model.IdCard = occupant.IdCard;
                if (string.IsNullOrEmpty(_model.Area))
                    _model.Area = occupant.Area;
                if (string.IsNullOrEmpty(_model.From))
                    _model.From = occupant.From;
                if (string.IsNullOrEmpty(_model.PhoneNumber))
                    _model.PhoneNumber = occupant.PhoneNumber;
            }
        }
    }
}