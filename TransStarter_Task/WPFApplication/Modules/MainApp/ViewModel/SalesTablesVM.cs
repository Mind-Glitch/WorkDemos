using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.EntityFrameworkCore;
using TransStarter_Task.WPFApplication.API;
using TransStarter_Task.WPFApplication.API.Xlsx;
using TransStarter_Task.WPFApplication.Common.Entity;
using TransStarter_Task.WPFApplication.Common.Models;
using TransStarter_Task.WPFApplication.Modules.Core.Components;
using TransStarter_Task.WPFApplication.Modules.Core.Model;
using TransStarter_Task.WPFApplication.Modules.Core.ViewModel;
using TransStarter_Task.WPFApplication.Modules.MainApp.Components;
using TransStarter_Task.WPFApplication.Modules.MainApp.Model;
using TransStarter_Task.WPFApplication.Persistance;

namespace TransStarter_Task.WPFApplication.Modules.MainApp.ViewModel;
internal class SalesTablesVM : VMBase
{
    public SalesTablesVM ()
        : base("Таблица продаж")
    {
        ReloadCommand = new ButtonCommand(OnReloadButtonClick);
        SaveToFileCommand = new ButtonCommand(OnSaveToFileButtonClick);
    }

    private void OnSaveToFileButtonClick (object? obj)
    {
        var fileName = obj as string;
        if ( string.IsNullOrEmpty(fileName) )
            return;

        var manager = new XlsxFolderManager("xlsx_exported");

        if ( !manager.CreateFileWithName($"{DateTime.Now:mmmm.dd.yyyy-HH.mm.ss}_{fileName}.xlsx",
               out var fileStream, out var createFileWithNameException) )
        {
            Task.Run(() => MessageBox.Show(createFileWithNameException.Message,
                "Не удалось открыть файл", MessageBoxButton.OK));
            return;
        }

        if ( !XlsxReport.CreateReportInFile(fileStream, AnnualDataDictionary, out var createReportInFileException) )
        {
            Task.Run(() => MessageBox.Show(createReportInFileException.Message,
                "Не удалось обработать файл", MessageBoxButton.OK));
            return;
        }

        fileStream.Close();

        // open file explorer in export folder
        try
        {
            var process = new Process();
            process.StartInfo.FileName = "explorer.exe";
            process.StartInfo.ArgumentList.Add(manager.GetFullPathToFolder);
            process.Start();
        }
        catch (Win32Exception ex) when (ex.NativeErrorCode == 5)
        {
            MessageBox.Show("Этот код пытается открыть папку в 'explorer.exe', " +
                            "в которую был записан файл xlsx. Папка находится в " +
                            "корне приложения + /xlsx_exported/. Возможно, ваше " +
                            "антивирусное ПО блокирует операцию." + ex.Message, 
                            "UnauthorizedAccessException", MessageBoxButton.OK);
        }
        catch ( Exception ex )
        {
            MessageBox.Show("Вызвано необработанное исключение: " + ex.Message, 
                "Unhandled exception", MessageBoxButton.OK);
        }
    }

    private void OnReloadButtonClick (object? obj)
    {
        var dbContext = new DatabaseContext();
        var controller = new LoadingController();

        loadingView = new Loading(controller);
        CurrentView = loadingView;

        Task.Run(async () =>
        {
            try
            {
                var orders = dbContext.Orders.Include(x=>x.Cars).ThenInclude(x=>x.VehicleInfo).ToList();
                AnnualDataDictionary = await CreateReportData(new SalesReport(orders, controller));

                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    _salesTablesList = new SalesTablesList { DataContext = this };
                    CurrentView = _salesTablesList;
                });
            }
            catch ( Exception ex )
            { MessageBox.Show(ex.Message, "Unhandled exception", MessageBoxButton.OK); }
        });
    }

    private const double ITEM_NAME_CELL_WIDTH = 120;
    private const double ITEM_PRICE_CELL_WIDTH = 70;

    [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
    private static Task<Dictionary<int, List<List<TableCellDTO>>>> CreateReportData (SalesReport report)
    {
        Action<Action<int>> forEveryMonth = operation =>
        {
            for (var monthIndex = 1; monthIndex <= 12; monthIndex++)
                operation.Invoke(monthIndex);
        };

        var years = report.RecordsPerItem.Select(x => x.YearIndex).Distinct().ToList();
        years.Sort((x, y) => x.CompareTo(y));

        var result = new Dictionary<int, List<List<TableCellDTO>>>();
        foreach ( var yearIndex in years )
        {
            var data = new List<List<TableCellDTO>>();

            var earlyEntiries = report.Records.Where(x => x.YearIndex == yearIndex).ToList();
            earlyEntiries.Sort((x, y) => string.Compare(x.ItemName, y.ItemName, StringComparison.Ordinal));

            var carNames = earlyEntiries.Select(x => x.ItemName).Distinct().ToList();

            List<TableCellDTO> line =
            [
                new (){Value = "Наименование товара", CellWidth = ITEM_NAME_CELL_WIDTH}
            ];

            forEveryMonth(monthIndex =>
            {
                line.Add(new TableCellDTO
                {
                    Value = new DateTime(2000, monthIndex, 1).ToString("MMMM", new CultureInfo("ru-RU")),
                    CellWidth = ITEM_PRICE_CELL_WIDTH
                });
            });

            line.Add(new TableCellDTO
            {
                Value = "Всего за товар",
                CellWidth = ITEM_NAME_CELL_WIDTH
            });

            data.Add(line);

            foreach ( var carName in carNames )
            {
                line =
                [
                    new TableCellDTO
                    {
                        Value = carName,
                        CellWidth = ITEM_NAME_CELL_WIDTH
                    }
                ];

                var carRelatedEntries = earlyEntiries.Where(x => x.ItemName == carName).ToList();

                forEveryMonth.Invoke(monthIndex =>
                {
                    var entry = carRelatedEntries.Find(x => x.MonthIndex == monthIndex);
                    line.Add(new TableCellDTO
                    {
                        Value = entry == null ? string.Empty : entry.IncomeString,
                        CellWidth = ITEM_PRICE_CELL_WIDTH
                    });
                });

                line.Add(new TableCellDTO
                {
                    Value = report.RecordsPerItem.Find(x => x.ItemName == carName && x.YearIndex == yearIndex)?
                        .IncomeString ?? string.Empty,
                    CellWidth = ITEM_NAME_CELL_WIDTH
                });

                data.Add(line);
            }

            line =
            [
                new TableCellDTO
                {
                    Value = "Доход за месяц",
                    CellWidth = ITEM_NAME_CELL_WIDTH
                }
            ];

            forEveryMonth(monthIndex =>
            {
                var monthReport = report.RecordsPerMonth.Find(x => x.YearIndex == yearIndex && x.MonthIndex == monthIndex);
                line.Add(new TableCellDTO { Value = monthReport?.IncomeString ?? string.Empty, CellWidth = ITEM_PRICE_CELL_WIDTH });
            });

            data.Add(line);
            result.Add(yearIndex, data);
        }

        return Task.FromResult(result);
    }

    private Loading loadingView;
    private SalesTablesList _salesTablesList;
    public Dictionary<int, List<List<TableCellDTO>>> AnnualDataDictionary { get; set; }


    private object? _currentView;
    public object? CurrentView
    {
        get => _currentView;
        private set => _ = SetField(ref _currentView, value);
    }

    public ICommand ReloadCommand { get; private set; }
    public ICommand SaveToFileCommand { get; private set; }
}