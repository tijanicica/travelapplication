using System;

namespace BookingApp.Dto;

public class VoucherDto
{
    public int Id { get; set; }
    public bool IsValid { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Reason { get; set; }
    public string TourGuideName { get; set; }
    
    public override string ToString()
    {
        return $"{Reason}\nExp: {ExpirationDate:dd/MM/yyyy}";
    }
}