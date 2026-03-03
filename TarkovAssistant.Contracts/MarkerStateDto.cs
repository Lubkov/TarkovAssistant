using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class MarkerStateDto
    {
        public int ProfileId { get; set; }
        public int MarkerId { get; set; }
        public bool IsFinished { get; set; }
        public bool IsSeleced { get; set; }

        public MarkerStateDto()
        {
            ProfileId = 0;
            MarkerId = 0;
            IsFinished = false;
            IsSeleced = false;
        }

        public MarkerStateDto(MarkerStateEntity state)
        {
            ProfileId = state.ProfileId;
            MarkerId = state.MarkerId;
            IsFinished = state.IsFinished;
            IsSeleced = state.IsSeleced;
        }
    }
}
