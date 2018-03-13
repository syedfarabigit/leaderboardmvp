using SiSU.Infrastructure;
using SiSU.Model;
using System.Collections.Generic;

namespace SiSU.Services
{
    public abstract class ContestResult
    {
        private List<Participant> _participants = new List<Participant>();
        private int _contestId { get; set; } 
        public bool IsDrawn { get; set; }
        public int? WinnerId { get; set; }

        public ContestResult(int contestId)
        {
            _contestId = contestId;
        }

        public void AddParticipant(Participant participant)
        {
            _participants.Add(participant);
        }

        public void RemoveParticipant(Participant participant)
        {
            _participants.Remove(participant);
        }

        public void Notify()
        {
            foreach (var participant in _participants)
            {
                participant.Update(IsDrawn, WinnerId);
            }
        }  
    }
    
}
