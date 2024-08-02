using System.Reflection;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using BookingApp.Service;

//using IContainer = System.ComponentModel.IContainer;

namespace BookingApp.Utils;

public static class Injector
{
    public static IContainer Container { get; set; }

    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();
        
        //repo
        builder.RegisterType<TourExecutionRepository>().As<ITourExecutionRepository>().SingleInstance();
        builder.RegisterType<TourGuideRepository>().As<ITourGuideRepository>().SingleInstance();
        builder.RegisterType<TouristRepository>().As<ITouristRepository>().SingleInstance();
        builder.RegisterType<TourReservationRepository>().As<ITourReservationRepository>().SingleInstance();
        builder.RegisterType<TourReviewRepository>().As<ITourReviewRepository>().SingleInstance();
        builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
        builder.RegisterType<VoucherRepository>().As<IVoucherRepository>().SingleInstance();
        builder.RegisterType<TourRepository>().As<ITourRepository>().SingleInstance();
        builder.RegisterType<NotificationRepository>().As<INotificationRepository>().SingleInstance();
        builder.RegisterType<TourSpotRepository>().As<ITourSpotRepository>().SingleInstance();
        builder.RegisterType<AccommodationRepository>().As<IAccommodationRepository>().SingleInstance();
        builder.RegisterType<AccommodationReservationRepository>().As<IAccommodationReservationRepository>().SingleInstance();
        builder.RegisterType<GuestRepository>().As<IGuestRepository>().SingleInstance();
        builder.RegisterType<GuestReviewRepository>().As<IGuestReviewRepository>().SingleInstance();
        builder.RegisterType<ReservationRescheduleRepository>().As<IReservationRescheduleRepository>().SingleInstance();
        builder.RegisterType<OwnerRepository>().As<IOwnerRepository>().SingleInstance();
        builder.RegisterType<AccommodationReviewRepository>().As<IAccommodationReviewRepository>().SingleInstance();
        builder.RegisterType<OwnerNotificationRepository>().As<IOwnerNotificationRepository>().SingleInstance();
        builder.RegisterType<GuestNotificationRepository>().As<IGuestNotificationRepository>().SingleInstance();
        builder.RegisterType<TourRequestRepository>().As<ITourRequestRepository>().SingleInstance();
        builder.RegisterType<RequestedTourRepository>().As<IRequestedTourRepository>().SingleInstance();
        builder.RegisterType<RenovationsRepository>().As<IRenovationsRepository>().SingleInstance();
        builder.RegisterType<SuperGuestRepository>().As<ISuperGuestRepository>().SingleInstance();

        builder.RegisterType<ForumRepository>().As<IForumRepository>().SingleInstance();

        builder.RegisterType<ComplexTourPartRepository>().As<IComplexTourPartRepository>().SingleInstance();
        builder.RegisterType<ComplexTourRequestRepository>().As<IComplexTourRequestRepository>().SingleInstance();
        

       
        
        
        //service
        builder.RegisterType<NotificationService>().As<INotificationService>().SingleInstance();
        builder.RegisterType<TourExecutionService>().As<ITourExecutionService>().SingleInstance();
        builder.RegisterType<TourReservationService>().As<ITourReservationService>().SingleInstance();
        builder.RegisterType<TourReviewService>().As<ITourReviewService>().SingleInstance();
        builder.RegisterType<TourService>().As<ITourService>().SingleInstance();
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
        builder.RegisterType<VoucherService>().As<IVoucherService>().SingleInstance();
        builder.RegisterType<TourSpotService>().As<ITourSpotService>().SingleInstance();
        builder.RegisterType<AccommodationService>().As<IAccommodationService>().SingleInstance();
        builder.RegisterType<AccommodationReservationService>().As<IAccommodationReservationService>().SingleInstance();
        builder.RegisterType<GuestService>().As<IGuestService>().SingleInstance();
        builder.RegisterType<GuestReviewService>().As<IGuestReviewService>().SingleInstance();
        builder.RegisterType<ReservationRescheduleService>().As<IReservationRescheduleService>().SingleInstance();
        builder.RegisterType<OwnerService>().As<IOwnerService>().SingleInstance();
        builder.RegisterType<AccomodationReviewService>().As<IAccommodationReviewService>().SingleInstance();
        builder.RegisterType<OwnerNotificationService>().As<IOwnerNotificationService>().SingleInstance();
        builder.RegisterType<GuestNotificationService>().As<IGuestNotificationService>().SingleInstance();
        builder.RegisterType<TourStatisticsService>().As<ITourStatisticsService>().SingleInstance();
        builder.RegisterType<TourRequestService>().As<ITourRequestService>().SingleInstance();
        builder.RegisterType<RequestedTourService>().As<IRequestedTourService>().SingleInstance();
        builder.RegisterType<RenovationsService>().As<IRenovationsService>().SingleInstance();
        builder.RegisterType<SuperGuestService>().As<ISuperGuestService>().SingleInstance();

        builder.RegisterType<ForumService>().As<IForumService>().SingleInstance();

        builder.RegisterType<ComplexTourPartService>().As<IComplexTourPartService>().SingleInstance();
        builder.RegisterType<ComplexTourRequestService>().As<IComplexTourRequestService>().SingleInstance();
        builder.RegisterType<TourGuideService>().As<ITourGuideService>().SingleInstance();


        
        Container = builder.Build();
        return Container;
    }
}