using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaigiamasisDarbas.Enums;

namespace BaigiamasisDarbas.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ResponsiblePersonId { get; set; }
        public Admin ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public MeetingCategory Category { get; set; }
        public MeetingType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<MeetingParticipant> Participants { get; set; }


        public Meeting() { }

        public Meeting(int id, string name, Admin responsiblePerson, string description, MeetingCategory category, MeetingType type, DateTime startDate, DateTime endDate, List<MeetingParticipant> participants)
        {
            Id = id;
            Name = name;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = category;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            Participants = participants;
        }

        public Meeting( string name, Admin responsiblePerson, string description, MeetingCategory category, MeetingType type, DateTime startDate, DateTime endDate, List<MeetingParticipant> participants)
        {
            Name = name;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = category;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            Participants = participants;
        }


        public override string ToString()
        {
            return $"Meeting ID:{Id} - Name:{Name}\nResponsible person{ResponsiblePerson}\n" +
                $"Decription:{Description}\nCategory:{Category}\nType:{Type}\n" +
                $"Start date:{StartDate} - End date:{EndDate}";
        }
    }
}
