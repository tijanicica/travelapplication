using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.OwnerPages;
using OwnerNotification = BookingApp.Domain.Model.OwnerNotification;

namespace BookingApp.WPF.ViewModel.Owner;

public class SelectedForumPageViewModel: ViewModelBase, INotifyPropertyChanged
{
    private OwnerWindow _ownerWindow;
    private readonly IForumService _forumService;
    
    public List<Forum> forums { get; set; }
    
    public List<string> guestComments { get; set; }
    public List<string> ownerComments { get; set; }
    public string unsplitedOwnerComments { get; set; }
    private ObservableCollection<Tuple<string, string>> _ownerCommentsDictionary;
    public Forum forumPublic { get; set; }
    
    public string? City { get; set; }
    
    public string? Country { get; set;  }

    private User _loggedUser;
    private IUserService _userService;
    
    public ObservableCollection<Tuple<string, string>> OwnerCommentsDictionary
    {
        get { return _ownerCommentsDictionary; }
        set
        {
            if (_ownerCommentsDictionary != value)
            {
                _ownerCommentsDictionary = value;
                OnPropertyChanged(nameof(OwnerCommentsDictionary));
            }
        }
    }
    public Dictionary<string, string> guestCommentsDictionary { get; set; }
    private string _newComment;

    public string NewComment
    {
        get { return _newComment; }
        set
        {
            if (_newComment != value)
            {
                _newComment = value;
                OnPropertyChanged();
            }
        }
    }

    public int numberOfGuestComments {  get; set; }
    public int numberOfOwnerComments {  get; set; }
    public bool reallyUsefull {  get; set; }
    public int numberEitherFromNotificationsOrAllForums {  get; set; }
    
    public ICommand AddCommentCommand { get; set; }
    public ICommand BackCommand { get; set; }


    public SelectedForumPageViewModel(OwnerWindow ownerWindow,User loggedUser, Forum seleectedForum, int number)
    {
        _ownerWindow = ownerWindow;
        var app = Application.Current as App;
        _forumService = Injector.Container.Resolve<IForumService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _loggedUser = app.LoggedUser;
      
        forumPublic = seleectedForum;
        City = seleectedForum.Location.City;
        Country = seleectedForum.Location.Country;
        guestComments = seleectedForum.AllGuestCommentsSplited;
        ownerComments = seleectedForum.AllOwnerCommentsSplited;
        unsplitedOwnerComments = seleectedForum.AllOwnerCommentsUnsplited;
        numberEitherFromNotificationsOrAllForums = number;
        

        AddCommentCommand = new ExecuteCommand<object>(AddCommentMethod);
        BackCommand = new ExecuteCommand<object>(BackMethod);
        
      
        Initialize();
        
    }
    private void Initialize()
    {
        OwnerCommentsDictionary = GetOwnerCommentsDictionary();
        guestCommentsDictionary = GetGuestCommentsDictionary();

        numberOfOwnerComments = OwnerCommentsDictionary.Count;
        numberOfGuestComments = guestCommentsDictionary.Count;

        reallyUsefull = numberOfOwnerComments >= 10 && numberOfGuestComments >= 20;
    }
   public void BackMethod(object parameter)
        {
            if(numberEitherFromNotificationsOrAllForums==1)
            {
                _ownerWindow.MainFrameOwnerWindow.Content = new OwnerNotification()
                {

                   //DataContext = new OwnerNotificationPageViewModel(_ownerWindow, _loggedUser)
                };
                
               // frameMain.Navigate(new NotificationsPage(logged, frameMain));
            }else
            {
                _ownerWindow.MainFrameOwnerWindow.Content = new AllForums()
                {
                    DataContext = new AllForumPageViewModel(_ownerWindow, _loggedUser)
                };
               // frameMain.Navigate(new AllForumsPage(logged, frameMain));
            }
        }

        public void AddCommentMethod(object parameter)
        {
            Tuple<string, string> newTuple = Tuple.Create(_loggedUser.Username, NewComment);
            OwnerCommentsDictionary.Add(newTuple);
            unsplitedOwnerComments += "*" + _loggedUser.Username + ":" + NewComment;
            forumPublic.AllOwnerCommentsUnsplited = unsplitedOwnerComments;
            _forumService.Update(forumPublic);
            NewComment = "";
        }

        public ObservableCollection<Tuple<string, string>> GetOwnerCommentsDictionary()
        {
            return GetCommentsDictionaryOwner(ownerComments);
        }

        public Dictionary<string, string> GetGuestCommentsDictionary()
        {
            return GetCommentsDictionary(guestComments);
        }

        private Dictionary<string, string> GetCommentsDictionary(List<string> comments)
        {
            if (comments == null)
            {
                return new Dictionary<string, string>();
            }

            var commentsDictionary = comments
                .Select(comment =>
                {
                    var parts = comment.Split(new[] { ':' }, 2);
                    return new { Author = parts[0], Comment = parts.Length > 1 ? parts[1] : string.Empty };
                })
                .ToDictionary(x => x.Author, x => x.Comment);

            return commentsDictionary;
        }

        private ObservableCollection<Tuple<string, string>> GetCommentsDictionaryOwner(List<string> comments)
        {
            if (comments == null)
            {
                return new ObservableCollection<Tuple<string, string>>();
            }

            var commentsList = comments
                .Select(comment =>
                {
                    var parts = comment.Split(new[] { ':' }, 2);
                    return Tuple.Create(parts[0], parts.Length > 1 ? parts[1] : string.Empty);
                })
                .ToList();

            return new ObservableCollection<Tuple<string, string>>(commentsList);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
}