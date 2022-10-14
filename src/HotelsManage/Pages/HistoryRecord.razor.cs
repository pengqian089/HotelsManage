using HotelsManage.Enum;
using HotelsManage.Services;
using HotelsManage.ViewModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotelsManage.Pages;

public partial class HistoryRecord
{
    [Inject] private ISnackbar Snackbar { get; set; }

    // private DateTime _startDate = DateTime.Now.AddDays(-7);
    //
    // private DateTime _endDate = DateTime.Now;

    private int _index = -1;

    private List<ChartSeries> _series = new();
    private string[] _axisLabels = Array.Empty<string>();
    private List<Summary> _source = new();
    private decimal _total = 0m;
    private int _length = 0;
    private int _count = 0;

    private bool? _isLoading = null;

    private DateRange _dateRange = new(DateTime.Now.AddDays(-7), DateTime.Now);

    private async Task QueryDataAsync()
    {
        if (_dateRange.Start == null || _dateRange.End == null)
        {
            Snackbar.Add("请选择日期", Severity.Warning);
            return;
        }

        var start = _dateRange.Start.Value.Date;
        var end = _dateRange.End.Value.Date.AddDays(1).AddMilliseconds(-1);

        if (start > end)
        {
            Snackbar.Add("开始时间不能大于结束时间", Severity.Warning);
            return;
        }

        _isLoading = true;

        var service = new HistoryRecordService();

        var list = await service.GetHistoryRecordsAsync(start, end).ToListAsync();

        #region chart
        
        var axisLabels = new List<string>();
        var occupants = new List<double>();
        var amounts = new List<double>();
        for (var i = 0; i < (end - start).TotalDays; i++)
        {
            var date = start.AddDays(i);
            var filter = list
                .Where(x => x.CheckInTime.HasValue && x.CheckInTime.Value.Date == date);
            var occupantCount = filter.Sum(x => x.OccupantCount);
            var totalAmount =
                list
                    .Where(x => x.CheckInTime.HasValue && x.CheckInTime.Value.Date == date)
                    .Sum(x => x.Price) +
                list
                    .Where(x => x.CheckInTime.HasValue && x.CheckInTime.Value.Date == date &&
                                x.DepositStatus == DepositStatus.Unreturned)
                    .Sum(x => x.Deposit);
            occupants.Add(occupantCount);
            amounts.Add((double)totalAmount);
            axisLabels.Add(date.ToString("yyyy/MM/dd"));
        }
        _series = new List<ChartSeries>
        {
            new() { Name = "入住人次", Data = occupants.ToArray() },
            new() { Name = "收入", Data = amounts.ToArray() },
        };
        _axisLabels = axisLabels.ToArray();

        #endregion

        _source = list;

        _total = list.Sum(x => x.Price) +
                 list.Where(x => x.DepositStatus == DepositStatus.Unreturned).Sum(x => x.Deposit);
        _length = list.Count;
        _count = list.Sum(x => x.OccupantCount);
        
        _isLoading = false;
    }
    
    private void ShowOccupant(Summary context)
    {
        context.ShowOccupant = !context.ShowOccupant;
    }
}