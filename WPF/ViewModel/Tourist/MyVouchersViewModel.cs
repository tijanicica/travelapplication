using System.Collections.Generic;
using System.Windows;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;

namespace BookingApp.WPF.ViewModel.Tourist;

public class MyVouchersViewModel : ViewModelBase
{
    private List<VoucherDto> _allVouchers;
    
    private IVoucherService _voucherService;
    private User _loggedUser;
    
    public MyVouchersViewModel()
    {
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _voucherService = Injector.Container.Resolve<IVoucherService>();
       
        CheckAndRemoveExpiredVouchers();
        _allVouchers = _voucherService.GetVouchers(_loggedUser.Id);
        CheckVoucherReservation();
    }
    
    public List<VoucherDto> Vouchers
    {
        get { return _allVouchers; }
        set
        {
            if (value != _allVouchers)
            {
                _allVouchers = value;
                OnPropertyChanged(nameof(Vouchers));
            }
        }
    }
    
    private void CheckAndRemoveExpiredVouchers()
    {
        _voucherService.CheckAndRemoveExpiredVouchers();
    }
    
    private void CheckVoucherReservation()
    {
        _voucherService.CheckAndAwardVoucher(_loggedUser.Id);
    }
}