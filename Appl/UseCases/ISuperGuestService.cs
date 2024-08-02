using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface ISuperGuestService
{
    public List<SuperGuest> GetAll();

        public SuperGuest Save(SuperGuest superGuest);

        public void Delete(SuperGuest superGuest);

        public SuperGuest Update(SuperGuest superGuest);

        public SuperGuest GetById(int id);

        public SuperGuest GetByGuestId(int guestId);

        public void GetSuperGuests(User loggedInGuest);

        public void ConfirmReservation(int guestNumber, int accommodationId, int guestId, string selectedDateRange,
            int days);
        public void UpdateSuperGuest(int guestId);
        public bool IsUserSuperGuest(int userId);

}