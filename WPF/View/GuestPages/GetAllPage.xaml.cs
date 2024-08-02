using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utils;

namespace BookingApp.WPF.View.GuestPages
{
    public partial class GetAllPage : Page
    {



        public GetAllPage()
        {
            InitializeComponent();
        }
    }
}