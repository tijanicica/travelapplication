using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class TourStatisticsPageViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;

    private string _mostVisitedTourEver;
    private string _mostVisitedTourForYear;
    private ITourStatisticsService _tourStatisticsService;
    private ITourService _tourService;
    private ObservableCollection<TourStatisticsModelViewModel> _finishedTours;
    private string _tourName;

    private ITourExecutionService _tourExecutionService;
    // private TourExecution _selectedExecution;

    public ICommand GenerateAllTimePdfCommand { get; private set; }
    public ICommand GenerateYearlyPdfCommand { get; private set; }


    public TourStatisticsPageViewModel(TourGuideWindow tourGuideWindow)
    {
        _tourGuideWindow = tourGuideWindow;
        _tourStatisticsService = Injector.Container.Resolve<ITourStatisticsService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
        _tourService = Injector.Container.Resolve<ITourService>();
        _mostVisitedTourEver = _tourStatisticsService.GetMostVisited().Name;
        EnterYearCommand = new DelegateCommand(CalculateMostVisitedTourForYear);
        
        //_finishedTours = new ObservableCollection<TourStatisticsModelViewModel>(_tourService.GetAllFinished());
        GetTourNames();
        ViewTouristStatisticsCommand = new DelegateCommand(OpenTouristsStatisticsPage);
        GenerateAllTimePdfCommand = new DelegateCommand(GenerateAllTimePdf);
        GenerateYearlyPdfCommand = new DelegateCommand(GenerateYearlyPdf);
    }
    

    public string MostVisitedTourEver
    {
        get => _mostVisitedTourEver;
        set
        {
            if (value != _mostVisitedTourEver)
            {
                _mostVisitedTourEver = value;
                OnPropertyChanged();
            }
        }
    }

    public string MostVisitedTourForYear
    {
        get => _mostVisitedTourForYear;
        set
        {
            if (value != _mostVisitedTourForYear)
            {
                _mostVisitedTourForYear = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<TourStatisticsModelViewModel> FinishedTours
    {
        get => _finishedTours;
        set
        {
            if (value != _finishedTours)
            {
                _finishedTours = value;
                OnPropertyChanged();
            }
        }
    }

    public string TourName
    {
        get => _tourName;
        set
        {
            if (value != _tourName)
            {
                _tourName = value;
                OnPropertyChanged();
            }
        }
    }

    public int Year { get; set; } = -1;

    public int[] Years { get; set; } = new[] { 2022, 2023, 2024 };

    public ICommand EnterYearCommand { get; private set; }

    private void CalculateMostVisitedTourForYear(object o)
    {
        try
        {
            MostVisitedTourForYear = _tourStatisticsService.GetMostVisited(Years[Year]).Name;
        }
        catch (Exception ex)
        {
            MostVisitedTourForYear = "";
            MessageBox.Show("Please select a year first.");
        }
     //   MostVisitedTourForYear = _tourService.GetMostVisited(Int32.Parse(Year)).Name;
    }

    private void GetTourNames()
    {
        FinishedTours =
            new ObservableCollection<TourStatisticsModelViewModel>();


        foreach (var tour in _tourService.GetAll())
        {
            foreach (var execution in _tourService.GetAllFinished())
            {
                if (execution.TourId == tour.Id)
                {
                    FinishedTours.Add(new TourStatisticsModelViewModel(tour.Name, tour.Id));
                }
            }
        }
    }

    public ICommand ViewTouristStatisticsCommand { get; private set; }

    private void OpenTouristsStatisticsPage(object param)
    {
        int tourId = (int)param;
        if (tourId != null)
        {
            _tourGuideWindow.MainFrameTourGuideWindow.Content = new TouristStatisticsPage()
            {
                DataContext =
                    new TouristStatisticsViewModel(_tourGuideWindow, _tourService.GetById(tourId))
            };
        }
    }
    
    private void GenerateAllTimePdf(object o)
    {
        string pdfPath = GeneratePdf("Most visited tour of all time is", MostVisitedTourEver);
        OpenPdfInBrowser(pdfPath);
    }

    private void GenerateYearlyPdf(object o)
    {
        if (Year != -1)
        {
            string pdfPath = GeneratePdf($"Most visited tour of year {Years[Year]} is", MostVisitedTourForYear);
            OpenPdfInBrowser(pdfPath);
        }
        else
        {
            MessageBox.Show("Please select a wanted year first.");
        }
        
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

    // Adding footer
    PdfPTable footerTable = new PdfPTable(1);
    footerTable.TotalWidth = 300; // Set the width of the table
    PdfPCell cell = new PdfPCell(new Phrase("Wandery Inc. 1234 Wanderlust Road, Adventure City. AC 58789 Phone: (123) 456-7890 | Email: contact@wandery.com", new Font(baseFont, 10, Font.NORMAL, BaseColor.BLACK)));
    cell.Border = PdfPCell.NO_BORDER;
    cell.HorizontalAlignment = Element.ALIGN_CENTER;
    footerTable.AddCell(cell);

// Calculate X position to center the table horizontally
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

