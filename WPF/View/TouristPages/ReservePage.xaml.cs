using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;

namespace BookingApp.WPF.View.TouristPages;

public partial class ReservePage : Page
{
    private int _currentTourExecutionId;
    private User _loggedUser;

    private ITourService _tourService;
    private ITourReservationService _tourReservationService;
    private ITourExecutionService _tourExecutionService;
    private IVoucherService _voucherService;
    
    
    private int _numberOfPeople;
    private ComboBox voucherComboBox;
    
    public ReservePage(int currentTourExectuionId)
    {
        InitializeComponent();
        this.DataContext = this;
        _currentTourExecutionId = currentTourExectuionId;
        var app = Application.Current as App;
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourReservationService = Injector.Container.Resolve<ITourReservationService>();
        _loggedUser = app.LoggedUser; 
        _voucherService = Injector.Container.Resolve<IVoucherService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
    }
    private void NumberOfPeopleTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;

        if (!int.TryParse(NumberOfPeopleTextBox.Text, out _numberOfPeople) || _numberOfPeople <= 0)
        {
            ErrorTextBlock.Text = "Please enter a valid positive number.";
            return;
        }

        if (!_tourService.IsBelowMaxCapacity(_numberOfPeople, _currentTourExecutionId))
        {
            //ako je uneo previše ljudi iskoci poruku koliko je ljudi i da mora ponovo
            var currentPeopleNumber = _tourService.GetCurrentFreeSpots(_currentTourExecutionId);
            ErrorTextBlock.Text = $"The entered number of tourists exceeds the limit of {currentPeopleNumber-1}. Please enter a smaller number.";
            return;
        }
        ErrorTextBlock.Text = "";
        NumberOfPeopleTextBox.IsEnabled = false;

        GenerateDynamicContentForTourists();
        AddChangeTouristsNumberPanel();
        AddUseVoucherPanel();
        AddActionButtonsPanel();
    }

    private void GenerateDynamicContentForTourists()
    {
        DynamicContentStackPanel.Children.Clear();

        for (int i = 0; i < _numberOfPeople; i++)
        {
            AddTouristHeader(i + 1);
            AddTouristDetails(i); 
        }
    }
    
    private void AddTouristHeader(int index)
    {
        var header = new TextBlock
        {
            Text = $"Tourist {index}:",
            FontWeight = FontWeights.Bold,
            FontSize = 22,
            Margin = new Thickness(5, 10, 5, 5) 
        };

        DynamicContentStackPanel.Children.Add(header);
    }
  
    private void AddTouristDetails(int index)
    {
        // Kreiranje Grid-a za svakog turistu sa tri kolone
        var grid = new Grid
        {
            Margin = new Thickness(5, 10, 5, 10) 
        };
        
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
        
        AddInputLabelAndTextBox(grid, "Name:", $"NameTextBox{index}", 0);
        AddInputLabelAndTextBox(grid, "Surname:", $"SurnameTextBox{index}", 1);
        AddInputLabelAndTextBox(grid, "Age:", $"AgeTextBox{index}", 2);
    
        DynamicContentStackPanel.Children.Add(grid);
    }
    
    private void AddInputLabelAndTextBox(Grid grid, string label, string name, int rowIndex)
    {
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Labele
        var labelBlock = new TextBlock
        {
            Text = label,
            Margin = new Thickness(0, 5, 10, 5),
            HorizontalAlignment = HorizontalAlignment.Left, 
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 20 
        };
        Grid.SetRow(labelBlock, rowIndex);
        Grid.SetColumn(labelBlock, 0);
        grid.Children.Add(labelBlock);

        // TextBox-ovi
        var textBox = new TextBox
        {
            Name = name,
            Margin = new Thickness(10, 5, 0, 5), 
            HorizontalAlignment = HorizontalAlignment.Stretch, 
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 20 
        };
        textBox.TextChanged += TextBox_TextChanged; 
        Grid.SetRow(textBox, rowIndex);
        Grid.SetColumn(textBox, 2);
        grid.Children.Add(textBox);
    }
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var allTextBoxesFilled = AreAllFieldsFilled();
        var submitButton = FindVisualChildren<Button>(this).FirstOrDefault(b => b.Content.ToString() == "Submit");
        if (submitButton != null)
        {
            submitButton.IsEnabled = allTextBoxesFilled;
        }
    }

    private bool AreAllFieldsFilled()
    {
        var textBoxes = FindVisualChildren<TextBox>(DynamicContentStackPanel).ToList();
        
        if (textBoxes.Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
        {
            return false;
        }
        
        var ageTextBoxes = textBoxes.Where(tb => tb.Name.StartsWith("AgeTextBox")).ToList();
        foreach (var ageTextBox in ageTextBoxes)
        {
            if (!int.TryParse(ageTextBox.Text, out int age) || age <= 0)
            {
                return false;
            }
        }
    
        return true;
    }



    
    private void AddChangeTouristsNumberPanel()
    {
        var changeNumberGrid = new Grid { Margin = new Thickness(5) };
        changeNumberGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Za labelu
        changeNumberGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Za dugme, zauzima preostali prostor

        var changeNumberLabel = new TextBlock { 
            Text = "(Optional) Change Tourists Number:", 
            VerticalAlignment = VerticalAlignment.Center, 
            Margin = new Thickness(0, 0, 5, 0),
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Left 
        };
      
        Grid.SetColumn(changeNumberLabel, 0);
        changeNumberGrid.Children.Add(changeNumberLabel);

        var changeNumberButton = new Button { 
            Content = "Change", 
            Width = 100, 
            Height = 35, 
            Margin = new Thickness(5, 0, 0, 0), 
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Right 
        };
        var style = (Style)Application.Current.FindResource("EnhancedButtonStyleReserve");
        if (style != null)
        {
            changeNumberButton.Style = style;
        }
        changeNumberButton.Click += ChangeButton_Click; 
        Grid.SetColumn(changeNumberButton, 1); 
        changeNumberGrid.Children.Add(changeNumberButton);

        DynamicContentStackPanel.Children.Add(changeNumberGrid);
    }


    /*
    private void AddUseVoucherPanel()
    {
        var voucherGrid = new Grid { Margin = new Thickness(5) };
        voucherGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Za labelu
        voucherGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Za ComboBox, zauzima preostali prostor

        var voucherLabel = new TextBlock { 
            Text = "Use Voucher:", 
            VerticalAlignment = VerticalAlignment.Center, 
            Margin = new Thickness(0, 0, 5, 0), // Dodajemo malo razmaka između labele i ComboBox-a
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        Grid.SetColumn(voucherLabel, 0); // Postavljanje labele u prvu kolonu
        voucherGrid.Children.Add(voucherLabel);

        voucherComboBox = new ComboBox { 
            Width = 200, 
            Margin = new Thickness(5, 0, 0, 0), 
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Right,
            Foreground = new SolidColorBrush(Colors.White),
            Background = new SolidColorBrush(Colors.Black),
            BorderBrush = new SolidColorBrush(Colors.Black)
            
        };
        
        var style_combo = (Style)Application.Current.FindResource("EnhancedComboBoxStyle");
        if (style_combo != null)
        {
            voucherComboBox.Style = style_combo;
        }
        PopulateVouchers(voucherComboBox);
        Grid.SetColumn(voucherComboBox, 1);
        voucherGrid.Children.Add(voucherComboBox);

        DynamicContentStackPanel.Children.Add(voucherGrid);
    }
*/
    private void AddUseVoucherPanel()
    {
        var voucherGrid = CreateVoucherGrid();

        // Create and add the label to the grid
        var voucherLabel = CreateLabel("Use Voucher:", new Thickness(0, 0, 5, 0));
        Grid.SetColumn(voucherLabel, 0);
        voucherGrid.Children.Add(voucherLabel);

        // Create and add the combobox to the grid
        voucherComboBox = CreateVoucherComboBox(new Thickness(5, 0, 0, 0), "EnhancedComboBoxStyle");
        PopulateVouchers(voucherComboBox);
        Grid.SetColumn(voucherComboBox, 1);
        voucherGrid.Children.Add(voucherComboBox);

        DynamicContentStackPanel.Children.Add(voucherGrid);
    }

    private Grid CreateVoucherGrid()
    {
        var grid = new Grid { Margin = new Thickness(5) };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // For label
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // For ComboBox, takes up remaining space
        return grid;
    }

    private TextBlock CreateLabel(string text, Thickness margin)
    {
        return new TextBlock
        {
            Text = text,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = margin,
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Left
        };
    }

    private ComboBox CreateVoucherComboBox(Thickness margin, string styleResourceKey)
    {
        var comboBox = new ComboBox
        {
            Width = 200,
            Margin = margin,
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Right,
            Foreground = new SolidColorBrush(Colors.White),
            Background = new SolidColorBrush(Colors.Black),
            BorderBrush = new SolidColorBrush(Colors.Black)
        };

        var style = (Style)Application.Current.FindResource(styleResourceKey);
        if (style != null)
        {
            comboBox.Style = style;
        }

        return comboBox;
    }


    private void PopulateVouchers(ComboBox comboBox)
    {
    
        List<VoucherDto> vouchers = _voucherService.GetVouchers(_loggedUser.Id);
        
        comboBox.Items.Add(new ComboBoxItem { Content = "Select Voucher", IsEnabled = false });
        
        foreach (var voucher in vouchers)
        {
            if (voucher.ExpirationDate >= DateTime.Now)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = voucher.ToString();
                item.Tag = voucher; 
                comboBox.Items.Add(item);
            }
        }
        comboBox.SelectedIndex = 0; // Postavite selekciju na prvi item
    }

   
     private void AddActionButtonsPanel()
    {
        var actionButtonsPanel = new Grid { Margin = new Thickness(5) };

        // Definišemo dva reda: jedan za 'Submit' i 'Cancel', drugi za 'Generate Report'
        actionButtonsPanel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        actionButtonsPanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) });
        actionButtonsPanel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        actionButtonsPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        actionButtonsPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
           
        var submitButton = new Button { Content = "Submit", Width = 100, Height = 35, IsEnabled = false, FontSize = 20, HorizontalAlignment = HorizontalAlignment.Left }; // Initially disable the Submit button
        submitButton.Click += SubmitButton_Click; 
        var style = (Style)Application.Current.FindResource("EnhancedButtonStyleReserve");
        if (style != null)
        {
            submitButton.Style = style;
        }
        Grid.SetColumn(submitButton, 0);
        Grid.SetRow(submitButton, 0); // Postavljamo Submit na prvi red
        actionButtonsPanel.Children.Add(submitButton);

        var cancelButton = new Button { Content = "Cancel", Width = 100, Height = 35, Margin = new Thickness(5, 0, 0, 0), FontSize = 20, HorizontalAlignment = HorizontalAlignment.Right };
        cancelButton.Click += CancelButton_Click; 
        var style2 = (Style)Application.Current.FindResource("EnhancedButtonStyleReserve");
        if (style != null)
        {
            cancelButton.Style = style2;
        }
        Grid.SetColumn(cancelButton, 1);
        Grid.SetRow(cancelButton, 0); // Postavljamo Cancel na prvi red
        actionButtonsPanel.Children.Add(cancelButton);
        
        // Dodavanje Generate Report dugmeta u drugi red
        var generateReportButton = new Button
        {
            Content = "Generate Report",
            Width = 200,
            Height = 35,
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Left, 
            Visibility = Visibility.Collapsed 
        };
        var style3 = (Style)Application.Current.FindResource("EnhancedButtonStyleReserve");
        if (style3 != null)
        {
            generateReportButton.Style = style3;
        }
        generateReportButton.Click += GenerateReportButton_Click;
        Grid.SetColumn(generateReportButton, 0); 
        Grid.SetRow(generateReportButton, 2); 
        actionButtonsPanel.Children.Add(generateReportButton);

        DynamicContentStackPanel.Children.Add(actionButtonsPanel);
    }


    private void ChangeButton_Click(object sender, RoutedEventArgs e)
    {
        DynamicContentStackPanel.Children.Clear();
        NumberOfPeopleTextBox.IsEnabled = true;
        NumberOfPeopleTextBox.Text = "";
        
        NumberOfPeopleTextBox.Focus();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.NavigationService?.Navigate(new HomePage());
    }
        
    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        var peopleOnTour = CollectPeopleData();

        var tourReservation = CreateTourReservation(peopleOnTour);
        _tourReservationService.CreateReservation(tourReservation);

        DeleteVoucher();
        
       

        var currentPeopleNumber = _tourService.GetCurrentFreeSpots(_currentTourExecutionId);
        AddTouristToTourExecution();
        Application.Current.Dispatcher.Invoke(() =>
        {
            var homePage = new HomePage
            {
                MessageText = $"Reservation successful!\nAvailable spots: {currentPeopleNumber}.",
                MessageVisibility = Visibility.Visible
            };

            this.NavigationService?.Navigate(homePage);
        });
        /*

        UpdateUIWithCurrentFreeSpots(currentPeopleNumber);
       
        var generateReportButton = FindVisualChildren<Button>(this).FirstOrDefault(b => b.Content.ToString() == "Generate Report");
        if (generateReportButton != null)
        {
            generateReportButton.Visibility = Visibility.Visible;
        }
        
        
        
        var timer = new System.Windows.Threading.DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(5);
        timer.Tick += (s, args) =>
        {
            OverlayGridSubmit.Visibility = Visibility.Collapsed;
            timer.Stop();
        };
        timer.Start();*/
    }
    
    private void DeleteVoucher()
    {
        if (voucherComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is VoucherDto selectedVoucher)
        {
            _voucherService.Delete(_voucherService.GetById(selectedVoucher.Id));
        }
    }
    
    private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        if (depObj != null)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }

                foreach (T childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }
    }


    private List<PersonOnTour> CollectPeopleData()
    {
        
        Random random = new Random();

        List<PersonOnTour> peopleOnTour = new List<PersonOnTour>();
        var textBoxes = FindVisualChildren<TextBox>(DynamicContentStackPanel).Where(tb => tb.Name.StartsWith("NameTextBox") || tb.Name.StartsWith("SurnameTextBox") || tb.Name.StartsWith("AgeTextBox")).ToList();
        
       // int id = -1;
        for (int i = 0; i < textBoxes.Count; i += 3)
        {
            if (int.TryParse(textBoxes[i + 2].Text, out int age))
            {
                
                Guid newGuid = Guid.NewGuid();
               
                peopleOnTour.Add(new PersonOnTour
                {
                    //Id = ++id,
                    Id = newGuid.ToString(),
                    Name = textBoxes[i].Text,
                    Surename = textBoxes[i + 1].Text,
                    Age = age,
                    Arrived = false
                });
            }
        }

        return peopleOnTour;
    }
    
  

    private TourReservation CreateTourReservation(List<PersonOnTour> peopleOnTour)
    {
        return new TourReservation
        {
            TourExecutionId = _currentTourExecutionId,
            TouristId = _loggedUser.Id,
            TouristNumber = _numberOfPeople,
            OtherPeopleOnTour = peopleOnTour
        };
    }

    private void AddTouristToTourExecution()
    {
        TourTourist tourist = new TourTourist
        {
            TouristId = _loggedUser.Id,
            JoinedAtTourSpot = -1
        };
        
       
        _tourExecutionService.AddTouristToTourExecution(tourist, _currentTourExecutionId);
    }

   /* private void UpdateUIWithCurrentFreeSpots(int currentPeopleNumber)
    {
        OverlayTextSubmit.Text = $"Available spots: {currentPeopleNumber}.";
        OverlayGridSubmit.Visibility = Visibility.Visible;
    }
 */

    private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
    {
      
        OverlayGridSubmit.Visibility = Visibility.Collapsed; 
    }

    
}