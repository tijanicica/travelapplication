using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BookingApp.WPF.ViewModel.Tourist;

public class ComplexTourRequestModelViewModel
{
    public ObservableCollection<ComplexTourPartModelViewModel> ComplexTourParts { get; set; } = new ObservableCollection<ComplexTourPartModelViewModel>();
    public string Status { get; set; }
    
}