using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Dto;
using BookingApp.Repository;

namespace BookingApp.Service;

public class VoucherService : IVoucherService
{

    private readonly IVoucherRepository _voucherRepository;
    private readonly ITourReservationService _tourReservationService;
    private readonly ITourExecutionService _tourExecutionService;

   // private readonly IUserRepository _userRepository;
   private readonly IUserService _userService;
   
    
    

    public VoucherService(IVoucherRepository voucherRepository, IUserService userService, ITourReservationService tourReservationService, ITourExecutionService tourExecutionService )

    {
        _voucherRepository = voucherRepository;
        _userService = userService;
        _tourReservationService = tourReservationService;
        _tourExecutionService = tourExecutionService;
    }
    
    public Voucher Save(Voucher voucher)
    {
        return _voucherRepository.Save(voucher);
    }
    public string GetReasonDescription(Reason reason)
    {
        switch (reason)
        {
            case Reason.TourCancelled:
                return "Tour Cancelled";
            case Reason.GuideQuit:
                return "Guide Quit";
            case Reason.Awarded:
                return "Awarded";
            default:
                return "Unknown";
        }
    }

    public List<VoucherDto> GetVouchers(int touristId)
    {
        return _voucherRepository.GetAll().Where(e=>e.TouristId == touristId).Select(e=> new VoucherDto
        {
            Id = e.Id,
            IsValid = e.IsValid,
            ExpirationDate = e.ExpirationDate,
            Reason =  GetReasonDescription(e.Reason),
            TourGuideName = e.TourGuideId == 0 ? "No assigned guide" : _userService.GetUsernameById(e.TourGuideId)
            
        }).ToList();
    }
  
    public Voucher GetById(int id)
    {
        return _voucherRepository.GetById(id);
    }
    
    public void Delete(Voucher voucher)
    {
        _voucherRepository.Delete(voucher);
    }
    
    public void CheckAndRemoveExpiredVouchers()
    {
        var expiredVouchers = _voucherRepository.GetAll()
            .Where(voucher => voucher.ExpirationDate < DateTime.Now).ToList();

        foreach (var expiredVoucher in expiredVouchers)
        {
            _voucherRepository.Delete(expiredVoucher);
        }
    }
    
    public void CheckAndAwardVoucher(int touristId)
    {
       
        // Proverite da li je vaučer već dodat za ovog turistu
        if (!_voucherRepository.GetAll().Any(voucher => voucher.TouristId == touristId && voucher.Reason == Reason.Awarded))
        {
            var tourReservations = _tourReservationService.GetAll()
                .Where(reservation => reservation.TouristId == touristId && 
                                      _tourExecutionService.GetById(reservation.TourExecutionId) != null &&
                                      _tourExecutionService.GetById(reservation.TourExecutionId).StartDate >= DateTime.Now.AddYears(-1))
                .ToList();

            // Broj tura koje je turista posetio u poslednjih godinu dana
            int numberOfToursVisited = tourReservations.Count;

            // Ako je turista posetio 5 ili više tura, dodajemo mu vaučer
            if (numberOfToursVisited >= 5)
            {
                // Kreiramo novi vaučer
                var voucher = new Voucher
                {
                    TouristId = touristId,
                    ExpirationDate = DateTime.Now.AddMonths(6), // Vaučer traje 6 meseci
                    IsValid = true,
                    Reason = Reason.Awarded
                };

                // Čuvamo vaučer u bazi podataka
                _voucherRepository.Save(voucher);
            }
        }
        
    }

   



}