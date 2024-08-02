using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class TouristStatisticsViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;
    private Tour _currentTour;
    private ITourStatisticsService _tourStatisticsService;
    public string TourName { get; set; }
    
    public int Children { get; set; }
    public int Adults { get; set;  }
    public int Elderly { get; set; }
    
    public SeriesCollection AgeGroupSeries { get; set; }

    public ICommand GenerateTouristStatisticsPdfCommand { get; private set; }
    public TouristStatisticsViewModel(TourGuideWindow tourGuideWindow, Tour currentTour)
    {
        _tourGuideWindow = tourGuideWindow;
        _tourStatisticsService = Injector.Container.Resolve<ITourStatisticsService>();
        _currentTour = currentTour;
        TourName = currentTour.Name;
        Children = _tourStatisticsService.CalculateChildrenVisitors(currentTour.Id);
        Adults = _tourStatisticsService.CalculateAdultVisitors(currentTour.Id);
        Elderly = _tourStatisticsService.CalculateElderlyVisitors(currentTour.Id);

        InitializeStatisticsData();
        GenerateTouristStatisticsPdfCommand = new DelegateCommand(GenerateTouristStatisticsPdf);

    }
    
    private void InitializeStatisticsData()
    {
        var seriesColors = new List<SolidColorBrush>
        {
            new SolidColorBrush(Colors.LightGray),
            new SolidColorBrush(Colors.White),
            new SolidColorBrush(Colors.DarkGray)
        };

        var children = _tourStatisticsService.CalculateChildrenVisitors(_currentTour.Id);
        var adults = _tourStatisticsService.CalculateAdultVisitors(_currentTour.Id);
        var elderly = _tourStatisticsService.CalculateElderlyVisitors(_currentTour.Id);
        AgeGroupSeries = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "Children",
                Values = new ChartValues<int> { children },
                Fill = seriesColors[0]
            },
            new ColumnSeries
            {
                Title = "Adults",
                Values = new ChartValues<int> { adults },
                Fill = seriesColors[1]
            },
            new ColumnSeries
            {
                Title = "Elderly",
                Values = new ChartValues<int> { elderly },
                Fill = seriesColors[2]
            }
        };
    }
    
    private void GenerateTouristStatisticsPdf(object o)
    {
        string pdfPath = GeneratePdf($"Tourists statistics for {TourName}", GetTouristStatisticsContent());
        OpenPdfInBrowser(pdfPath);
    }

    private string GetTouristStatisticsContent()
    {
        return $"Number of children: {Children}\nNumber of adults: {Adults}\nNumber of elderly: {Elderly}";
    }

     private string GeneratePdf(string title, string content)
{
    string pdfPath = Path.Combine(Path.GetTempPath(), $"{title.Replace(" ", "_")}.pdf");

    Document document = new Document();
    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));
    document.Open();

    var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
    var headerFont = new Font(baseFont, 18, Font.BOLD, new BaseColor(0xFF, 0x5C, 0x4E));
    var header = new Paragraph("WanderY", headerFont)
    {
        Alignment = Element.ALIGN_CENTER
    };
    document.Add(header);

    document.Add(new Paragraph("\n"));

    var infoFont = new Font(baseFont, 12, Font.NORMAL);
    var info = new Paragraph("Wandery statistics provides you the following information for tour statistics:", infoFont)
    {
        Alignment = Element.ALIGN_LEFT
    };
    document.Add(info);

    document.Add(new Paragraph("\n"));

    var titleFont = new Font(baseFont, 14, Font.NORMAL);
    var contentFont = new Font(baseFont, 12, Font.BOLD);

    var titleParagraph = new Paragraph(title, titleFont)
    {
        Alignment = Element.ALIGN_LEFT
    };
    document.Add(titleParagraph);

    var contentParagraph = new Paragraph(content, contentFont)
    {
        Alignment = Element.ALIGN_LEFT
    };
    document.Add(contentParagraph);

    PdfPTable footerTable = new PdfPTable(1);
    footerTable.TotalWidth = 300; // Set the width of the table
    PdfPCell cell = new PdfPCell(new Phrase("Wandery Inc. 1234 Wanderlust Road, Adventure City. AC 58789 Phone: (123) 456-7890 | Email: contact@wandery.com", new Font(baseFont, 10, Font.NORMAL, BaseColor.BLACK)));
    cell.Border = PdfPCell.NO_BORDER;
    cell.HorizontalAlignment = Element.ALIGN_CENTER;
    footerTable.AddCell(cell);

    float xPos = (document.PageSize.Width - footerTable.TotalWidth) / 2;

    if (footerTable.TotalWidth > 0)
    {
        footerTable.WriteSelectedRows(0, -1, xPos, document.BottomMargin + 10, writer.DirectContent);
    }


    document.Close();

    return pdfPath;
}

    private void OpenPdfInBrowser(string pdfPath)
    {
        Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
    }
    
}