using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;
using BookingApp.WPF.View;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;
using BookingApp.WPF.View;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;
using HorizontalAlignment = iText.Layout.Properties.HorizontalAlignment;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using VerticalAlignment = iText.Layout.Properties.VerticalAlignment;

namespace BookingApp.WPF.ViewModel.Tourist;

public class MyReservationsViewModel : ViewModelBase
{
    private TouristWindow _touristWindow;
    private ITourReservationService _tourReservationService;
    private ITourService _tourService;
    private List<TourDto> _allToursDtoList;
    private int _currentTourExecutionId;
    private User _loggedUser;
    
    public ICommand GenerateReportCommand { get; private set; }
    public MyReservationsViewModel(TouristWindow touristWindow)
    {
        _touristWindow = touristWindow;
        
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _tourReservationService = Injector.Container.Resolve<ITourReservationService>();
        _tourService = Injector.Container.Resolve<ITourService>();
        _allToursDtoList = _tourService.GetAllByTouristIdReport(_loggedUser.Id);
        //meni trebaju info o tourexecution koje sam ja rezervisao

        
        GenerateReportCommand = new DelegateCommand(GenerateReportAction);
    }
    public Visibility NoReservationsVisibility => AllToursDto == null || !AllToursDto.Any() ? Visibility.Visible : Visibility.Collapsed;

    public List<TourDto> AllToursDto
    {
        get { return _allToursDtoList; }
        set
        {
            if (value != _allToursDtoList)
            {
                _allToursDtoList = value;
                OnPropertyChanged(nameof(AllToursDto));
                OnPropertyChanged(nameof(NoReservationsVisibility)); // Notify that visibility might have changed
            }
        }
    }
    private void GenerateReportAction(object param)
{
    int tourId = (int)param;

    if (tourId != null)
    {
        var selectedTour = _allToursDtoList.FirstOrDefault(t => t.Id == tourId);
        if (selectedTour != null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF file (*.pdf)|*.pdf",
                FileName = $"TourReport_{selectedTour.Id}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string pdfPath = saveFileDialog.FileName;
                using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfWriter writer = new PdfWriter(fs);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    //slika jebe
                    ImageData imageData = ImageDataFactory.Create("C:\\Users\\Nikola\\Desktop\\sims_projekat\\sims-in-2024-group-1-team-a\\Resources\\Images\\voucher.png");
                    Image logo = new Image(imageData).SetWidth(100).SetHorizontalAlignment(HorizontalAlignment.LEFT);

                    Paragraph companyName = new Paragraph("Wandery")
                        .SetFontSize(14)
                        .SetBold()
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetVerticalAlignment(VerticalAlignment.BOTTOM);

// Postavite logo i naziv u isti red
                    Paragraph header = new Paragraph()
                        .Add(logo)
                        .Add(new Tab())
                        .Add(companyName);

                    document.Add(header);

                    // Dodavanje linija
                    document.Add(new LineSeparator(new SolidLine()));

                    // Dodavanje osnovnih informacija o turi
                    Paragraph title = new Paragraph("Tour reservation report").SetFontSize(20).SetBold().SetTextAlignment(TextAlignment.CENTER);
                    document.Add(title);
                    document.Add(new Paragraph($"Tour name: {selectedTour.Name}").SetFontSize(16));
                    document.Add(new Paragraph($"Language: {selectedTour.Language}"));
                    document.Add(new Paragraph($"Location: {selectedTour.Country}, {selectedTour.City}"));
                    document.Add(new Paragraph($"Tour guide: {selectedTour.TourGuide}"));
                    document.Add(new Paragraph($"Duration: {selectedTour.Duration}"));
                    document.Add(new Paragraph($"Start date: {selectedTour.StartDate.ToShortDateString()}h"));

                    // Dodavanje osoba na turi
                    // Dodavanje osoba na turi kao tabelu
                    document.Add(new Paragraph("People on tour:"));
                    List list = new List();
                    
                    foreach (var person in selectedTour.OtherPeopleOnTour)
                    {
                        list.Add(new ListItem($"{person.Name} {person.Surename}, {person.Age}"));
                    }
                    document.Add(list);

                    
                    document.Add(new Paragraph($"Tour description: {selectedTour.Description}"));
                    
                    document.Close();

                }

                // Otvorite generirani PDF u pregledniku
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                });
            }
        }
    }
}


    
 
    

  
}