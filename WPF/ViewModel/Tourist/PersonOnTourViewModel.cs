namespace BookingApp.WPF.ViewModel.Tourist;

public class PersonOnTourViewModel : ViewModelBase
{
    /*
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }

    public PersonOnTourViewModel(string name, string surname, int age)
    {
        Name = name;
        Surname = surname;
        Age = age;
    }

    public PersonOnTourViewModel()
    {
        
    }
    */
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    private string _surname;
    public string Surname
    {
        get => _surname;
        set
        {
            if (_surname != value)
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
    }

    private int _age;
    public int Age
    {
        get => _age;
        set
        {
            if (_age != value)
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }
    }

    public PersonOnTourViewModel(string name, string surname, int age)
    {
        _name = name;
        _surname = surname;
        _age = age;
    }

    public PersonOnTourViewModel()
    {
        
    }
}