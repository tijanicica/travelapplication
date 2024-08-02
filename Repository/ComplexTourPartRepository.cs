using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository;

public class ComplexTourPartRepository : IComplexTourPartRepository
{
     private const string FilePath = "../../../Resources/Data/complexTourParts.csv";
    
            private readonly Serializer<ComplexTourPart> _serializer;
    
            private List<ComplexTourPart> _complexTourParts;
    
            public ComplexTourPartRepository()
            {
                _serializer = new Serializer<ComplexTourPart>();
                _complexTourParts = _serializer.FromCSV(FilePath);
            }
    
            public List<ComplexTourPart> GetAll()
            {
                return _serializer.FromCSV(FilePath);
            }
    
            public ComplexTourPart Save(ComplexTourPart complexTourPart)
            {
                complexTourPart.Id = NextId();
                _complexTourParts = _serializer.FromCSV(FilePath);
                _complexTourParts.Add(complexTourPart);
                _serializer.ToCSV(FilePath, _complexTourParts);
                return complexTourPart;
            }
    
            public int NextId()
            {
                _complexTourParts = _serializer.FromCSV(FilePath);
                if (_complexTourParts.Count < 1)
                {
                    return 1;
                }
                return _complexTourParts.Max(c => c.Id) + 1;
            }
    
            public void Delete(ComplexTourPart complexTourPart)
            {
                _complexTourParts = _serializer.FromCSV(FilePath);
                ComplexTourPart founded = _complexTourParts.Find(c => c.Id == complexTourPart.Id);
                _complexTourParts.Remove(founded);
                _serializer.ToCSV(FilePath, _complexTourParts);
            }
    
            public ComplexTourPart Update(ComplexTourPart complexTourPart)
            {
                _complexTourParts = _serializer.FromCSV(FilePath);
                ComplexTourPart current = _complexTourParts.Find(c => c.Id == complexTourPart.Id);
                int index = _complexTourParts.IndexOf(current);
                _complexTourParts.Remove(current);
                _complexTourParts.Insert(index, complexTourPart);       // keep ascending order of ids in file 
                _serializer.ToCSV(FilePath, _complexTourParts);
                return complexTourPart;
            }
}