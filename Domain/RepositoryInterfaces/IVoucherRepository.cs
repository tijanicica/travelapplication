using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IVoucherRepository
{
    Voucher Save(Voucher voucher);


     int NextId();
    IEnumerable<Voucher> GetAll();
    void Delete(Voucher voucher);
    Voucher GetById(int Id);
    

}