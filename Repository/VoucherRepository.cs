using System.Collections.Generic;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;


namespace BookingApp.Repository;

public class VoucherRepository : IVoucherRepository
{
    private const string FilePath = "../../../Resources/Data/vouchers.csv";

    private readonly Serializer<Voucher> _serializer;

    private List<Voucher> _vouchers;

    public VoucherRepository()
    {
        _serializer = new Serializer<Voucher>();
        _vouchers = _serializer.FromCSV(FilePath);
    }
    
    public Voucher Save(Voucher voucher)
    {
        voucher.Id = NextId();
        _vouchers = _serializer.FromCSV(FilePath);
        _vouchers.Add(voucher);
        _serializer.ToCSV(FilePath, _vouchers);
        return voucher;
    }
    
    public int NextId()
    {
        _vouchers = _serializer.FromCSV(FilePath);
        if (_vouchers.Count < 1)
        {
            return 1;
        }
        return _vouchers.Max(c => c.Id) + 1;
    }
    
    public IEnumerable<Voucher> GetAll()
    {
        _vouchers = _serializer.FromCSV(FilePath);
        return _vouchers;
    }
    
    public void Delete(Voucher voucher)
    {
        _vouchers = _serializer.FromCSV(FilePath);
        Voucher founded = _vouchers.Find(c => c.Id == voucher.Id);
        _vouchers.Remove(founded);
        _serializer.ToCSV(FilePath, _vouchers);
    }
    
    public Voucher GetById(int Id)
    {
        UpdateList();
        var tour = _vouchers.FirstOrDefault(t => t.Id == Id);
        return tour;
    }
    
    private void UpdateList()
    {
        _vouchers = _serializer.FromCSV(FilePath);
    }


}