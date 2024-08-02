using System;

namespace BookingApp.WPF.ViewModel.Owner;

public class DatePair
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DatePair(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
}