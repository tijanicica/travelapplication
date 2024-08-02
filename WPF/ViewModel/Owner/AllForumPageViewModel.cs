using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.OwnerPages;

namespace BookingApp.WPF.ViewModel.Owner;

public class AllForumPageViewModel: ViewModelBase
{
    private OwnerWindow _ownerWindow;
    private readonly IForumService _forumService;
    
    public List<Forum> forums { get; set; }
    public int numberOfOwnerComments { get; set; }
    public int numberOfGuestComments { get; set; }
    public string? City { get; set; }
    
    public string? Country { get; set;  }

    private User _loggedUser;
    private IUserService _userService;
    public ICommand ViewSelectedForumCommand { get; private set; }


    public AllForumPageViewModel(OwnerWindow ownerWindow , User loggedUser)
    {
        _ownerWindow = ownerWindow;
        var app = Application.Current as App;
        _forumService = Injector.Container.Resolve<IForumService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _loggedUser = loggedUser;
      
        ViewSelectedForumCommand = new ExecuteCommand<object>(ViewSelectedForumMetod);
        InitializeForums();
        
    }
    private void InitializeForums()
    {
        forums = _forumService.GetAll();
    }
    private void ViewSelectedForumMetod(object parameter)
    {
        var forum = parameter as Forum;
        _ownerWindow.MainFrameOwnerWindow.NavigationService.Navigate(new SelectedForum()
        {
            DataContext = new SelectedForumPageViewModel(_ownerWindow, _loggedUser, forum, 1)
        });
        // _ownerWindow.MainFrameOwnerWindow.Content = new SelectedForumPageViewModel(_ownerWindow, _loggedUser, forum, 1);

    }
}