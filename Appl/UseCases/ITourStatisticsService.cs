using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface ITourStatisticsService
{
    Tour GetMostVisited(int year = 0);
    int CalculateChildrenVisitors(int tourId);
    int CalculateAdultVisitors(int tourId);
    int CalculateElderlyVisitors(int tourId);

}