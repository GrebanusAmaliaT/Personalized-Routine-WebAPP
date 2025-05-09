namespace AplicatieRutina.Models
{
    public class Calendar
    {
        public List<CalendarEvent> CalendarDays { get; set; } = new();
        public List<CalendarEvent> UpcomingEvents { get; set; } = new();
    }
}
