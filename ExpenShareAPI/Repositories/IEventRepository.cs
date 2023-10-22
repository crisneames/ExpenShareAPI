using ExpenShareAPI.Models;

namespace ExpenShareAPI.Repositories
{
    public interface IEventRepository
    {
        void AddNewEvent(Event gig);
        void DeleteEvent(int id);
        Event GetEventById(int id);
        List<Event> GetEvents();
        void UpdateEvent(Event gig);
    }
}