using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class CalendarEvent
    {

        [Required(ErrorMessage = "Event title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Event start time is required.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Event end time is required.")]
        public DateTime EndTime { get; set; }

    }
}