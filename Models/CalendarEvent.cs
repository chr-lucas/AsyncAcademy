using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class CalendarEvent
    {

        [Required(ErrorMessage = "Event title is required.")]
        public string title { get; set; }

        [Required(ErrorMessage = "Event start time is required.")]
        public DateTime start { get; set; }

        [Required(ErrorMessage = "Event end time is required.")]
        public DateTime end { get; set; }

    }
}