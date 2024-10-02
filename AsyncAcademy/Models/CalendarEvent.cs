using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class CalendarEvent
    {

        //contributions from Chris L. in naming conventions to correct display errors

        [Required(ErrorMessage = "Event title is required.")]
        public string title { get; set; }

        //DateTime of Start time (For Assignments start and end should match)
        public DateTime? start { get; set; }

        //DateTime of End time (For Assignments start and end should match)
        public DateTime? end { get; set; }
        
        //Clickable URL assigned to object
        public string? url { get; set; }

        //time class occurs (For Recurring events)
        public string? startTime { get; set; }

        //time class ends (For Recurring events)
        public string? endTime { get; set; }

        //Date semester begins (For Recurring events)
        public DateTime? startRecur { get; set; }

        //Date semester ends (For Recurring events)
        public DateTime? endRecur { get; set; }

        //Days classes occur
        public int[]? daysOfWeek { get; set; }

        //Set Event Display Attributes
        public string? display { get; set; } = "auto";
        public string? backgroundColor { get; set; }
        public string? borderColor { get; set; }
        public string? textColor { get; set; }

    }
}