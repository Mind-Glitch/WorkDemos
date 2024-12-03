using TransStarter_Task.WPFApplication.API;
using TransStarter_Task.WPFApplication.Common.Entity;

namespace TransStarter_Task.WPFApplication.Common.Models;
internal class SalesReport
{
    public SalesReport (List<Order> orders, LoadingController? loadingController = null)
    {
        #region Полный рапорт
        loadingController?.SetMax(orders.Count);
        loadingController?.SetCurrent(0);
        loadingController?.SetOperationName("Подготовка основного рапорта");
        foreach ( var order in orders )
        {

            loadingController?.SetCurrent(orders.IndexOf(order) + 1);
            foreach ( var car in order.Cars )
            {
                var vehicleName = GetVehicleName(car.VehicleInfo);

                var record = _records.Find(x =>
                        x.MonthIndex == order.OrderDate.Month &&
                        x.ItemName == vehicleName &&
                        x.YearIndex == order.OrderDate.Year);

                if ( record == null )
                {
                    _records.Add(new ReportRecord()
                    {
                        Income = car.Price,
                        ItemName = vehicleName,
                        MonthIndex = order.OrderDate.Month,
                        YearIndex = order.OrderDate.Year
                    });
                }
                else
                    record.Income += car.Price;
            }
        }
        #endregion

        #region Итоговый доход за месяц
        loadingController?.SetMax(orders.Count);
        loadingController?.SetCurrent(0);
        loadingController?.SetOperationName("Подготовка результатов за месяц");
        foreach ( var order in orders )
        {
            loadingController?.SetCurrent(orders.IndexOf(order) + 1);

            var perMonthRecord = _recordsPerMonth.Find(x =>
                x.MonthIndex == order.OrderDate.Month &&
                x.YearIndex == order.OrderDate.Year);

            if ( perMonthRecord == null )
            {
                _recordsPerMonth.Add(new ReportRecord()
                {
                    Income = order.TotalPrice,
                    MonthIndex = order.OrderDate.Month,
                    YearIndex = order.OrderDate.Year
                });
            }
            else
                perMonthRecord.Income += order.TotalPrice;
        }
        #endregion

        #region Годичный доход за каждую модель
        loadingController?.SetMax(orders.Count);
        loadingController?.SetCurrent(0);
        loadingController?.SetOperationName("Подготовка результатов по модели");
        foreach ( var order in orders )
        {
            loadingController?.SetCurrent(orders.IndexOf(order) + 1);

            if ( order.Cars != null )
                foreach ( var car in order.Cars )
                {
                    var vehicleName = GetVehicleName(car.VehicleInfo);

                    var perItemRecord = _recordsPerItem.Find(x =>
                        x.ItemName == vehicleName &&
                        x.YearIndex == order.OrderDate.Year
                    );

                    if ( perItemRecord == null )
                    {
                        _recordsPerItem.Add(new ReportRecord
                        {
                            ItemName = vehicleName,
                            Income = car.Price,
                            YearIndex = order.OrderDate.Year
                        });
                    }
                    else
                        perItemRecord.Income += car.Price;
                }
        }
        #endregion

        loadingController?.Complete();
    }

    private static string GetVehicleName (VehicleInfo? vehicle)
    {
        return vehicle?.GetVehicleFullName() ?? "NOT_SPECIFIED";
    }

    private readonly List<ReportRecord> _records = [];
    private readonly List<ReportRecord> _recordsPerMonth = [];
    private readonly List<ReportRecord> _recordsPerItem = [];

    public List<ReportRecord> Records => _records;
    public List<ReportRecord> RecordsPerMonth => _recordsPerMonth;
    public List<ReportRecord> RecordsPerItem => _recordsPerItem;
}