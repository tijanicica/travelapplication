using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;

namespace BookingApp.WPF.View.GuestPages
{
    public partial class DetailsPage : Page
    {
        public Accommodation SelectedAccommodation { get; set; }
        private AccommodationReservation accommodationReservation;
        private IAccommodationReservationService _accommodationReservationService;
        
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestNumber { get; set; }
        public int NumberOfDays { get; set; }

        public DetailsPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public DetailsPage(Accommodation accommodation)
        {
            InitializeComponent();
            SelectedAccommodation = accommodation;
            this.DataContext = SelectedAccommodation;
        }
        

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

    
        
      
    }
}