using System.Collections.Generic;
using BookingApp.Domain.Model;
using BookingApp.Dto;

namespace BookingApp.Appl.UseCases;

public interface IVoucherService
{
     Voucher Save(Voucher voucher);
     List<VoucherDto> GetVouchers(int touristId);
     Voucher GetById(int id);
     void Delete(Voucher voucher);
     void CheckAndRemoveExpiredVouchers();
     void CheckAndAwardVoucher(int touristId);

}