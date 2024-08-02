using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Repository;
using BookingApp.Service;
using System;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BookingApp.Domain.Model;
using ISerializable = BookingApp.Serializer.ISerializable;
using BookingApp.Serializer;
using BookingApp.Utils;

namespace BookingApp
{
    public partial class App : Application
    {
        private User _loggedUser = null;


        public User LoggedUser
        {
            get => _loggedUser;
            set
            {
                if (value != _loggedUser)
                    _loggedUser = value;
            }
        }


        public App()
        {
            Injector.Configure();
        }
    }
}