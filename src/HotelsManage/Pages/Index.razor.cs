﻿using HotelsManage.Enum;
using HotelsManage.Services;
using HotelsManage.Shared.Component;
using HotelsManage.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace HotelsManage.Pages;

public partial class Index
{
    private readonly RoomService _roomService = new RoomService();

    private IEnumerable<RoomDetail> _source = new List<RoomDetail>();

    private bool _isLoading = true;

    [Inject] private IDialogService DialogService { get; set; }

    [Inject] private ISnackbar Snackbar { get; set; }

    [Inject] private ILogger<Index> Logger { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadRoomDataAsync();
        await base.OnInitializedAsync();
    }

    private async Task LoadRoomDataAsync()
    {
        _isLoading = true;
        _source = await _roomService.GetRoomDetailsAsync().ToListAsync();
        _isLoading = false;
    }

    private void ShowOccupant(RoomDetail roomDetail)
    {
        roomDetail.ShowOccupant = !roomDetail.ShowOccupant;
    }

    private async Task SaveRoomAsync(int? roomId = null)
    {
        var parameters = new DialogParameters
        {
            ["RoomId"] = roomId,
        };
        var dialog = DialogService.Show<RoomForm>("", parameters);
        var result = await dialog.Result;
        if (!result.Cancelled && result.Data is true)
        {
            Snackbar.Add($"成功{(roomId.HasValue ? "修改" : "添加")}房间", Severity.Success);
            await LoadRoomDataAsync();
        }
    }

    /// <summary>
    /// 保存登记信息
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="recordId"></param>
    private async Task SaveRegisterInformationAsync(int roomId,int? recordId = null)
    {
        var parameters = new DialogParameters
        {
            ["RoomId"] = roomId,
            ["RecordId"] = recordId
        };
        var dialog = DialogService.Show<CheckInRegisterForm>("", parameters);
        var result = await dialog.Result;
        if (!result.Cancelled && result.Data is true)
        {
            Snackbar.Add("登记信息保存成功", Severity.Success);
            await LoadRoomDataAsync();
        }
    }

    private async Task DeleteRoomAsync(RoomDetail context)
    {
        if (context.Status != RoomStatus.Empty)
        {
            Snackbar.Add("不是空房状态下，无法删除", Severity.Warning);
            return;
        }

        var checkResult = await DialogService.ShowMessageBox("提示", "删除房间操作不可撤销，确定要删除吗？", "删除", "取消");
        if (checkResult == true)
        {
            try
            {
                await _roomService.DeleteRoomAsync(context.Id);
                Snackbar.Add($"删除[{context.Name}]成功", Severity.Success);
                await LoadRoomDataAsync();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "delete room fail");
                Snackbar.Add(e.Message, Severity.Error);
            }
        }
    }

    private async Task CheckOutRoomAsync(RoomDetail context)
    {
        var checkResult = await DialogService.ShowMessageBox("提示", "退房操作不可撤销确定要退房吗？", "退房", "取消");
        if (checkResult == true)
        {
            var room = await _roomService.GetAsync(context.Id);
            if (room == null)
            {
                Snackbar.Add("没有查询到该房间", Severity.Error);
                return;
            }

            if (room.Status == RoomStatus.Empty)
            {
                Snackbar.Add("此房间为空房，不能进行退房操作", Severity.Warning);
                return;
            }

            var service = new HistoryRecordService();
            var record = await service.FindCheckInAsync(room.Id);
            if (record == null)
            {
                Snackbar.Add("没有查找到开房记录，请检查", Severity.Warning);
                return;
            }

            var depositStatus = record.Deposit == 0m ? DepositStatus.NotCharged : DepositStatus.Pay;
            if (record.DepositStatus == DepositStatus.Pay)
            {
                var result = await DialogService.ShowMessageBox("提示", $"是否退还押金[{record.Deposit}]？", "退还", "不退还");
                depositStatus = result switch
                {
                    true => DepositStatus.Returned,
                    false => DepositStatus.Unreturned,
                    _ => depositStatus
                };
            }

            await _roomService.RoomCheckOut(room.Id, depositStatus);
            await LoadRoomDataAsync();
        }
    }

    private async Task LeaseAsync(RoomDetail context)
    {
        if (context.Status == RoomStatus.CheckIn)
        {
            var parameters = new DialogParameters
            {
                ["RoomId"] = context.Id,
            };
            var dialog = DialogService.Show<LeaseForm>("", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled && result.Data is true)
            {
                Snackbar.Add("续房成功", Severity.Success);
                await LoadRoomDataAsync();
            }
        }
    }
}