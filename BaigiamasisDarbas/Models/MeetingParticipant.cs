using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Models
{
    public class MeetingParticipant
    {
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public int ParticipantId { get; set; }
        public Worker Participant { get; set; }
        public DateTime JoinedDate { get; set; }


      
    }
}
