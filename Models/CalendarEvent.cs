using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class CalendarEvent
    {

        //contributions from Chris L. in naming conventions to correct display errors

        [Required(ErrorMessage = "Event title is required.")]
        public string title { get; set; }

        //time class occurs
        [Required(ErrorMessage = "Event start time is required.")]
        public DateTime startTime { get; set; }

        //time class ends
        [Required(ErrorMessage = "Event end time is required.")]
        public DateTime endTime { get; set; }

        //Date semester begins
        public DateTime startRecur { get; set; }

        //Date semester ends
        public DateTime endRecur { get; set; }

        //Days classes occur
        public int[]? daysOfWeek { get; set; }

    }
}