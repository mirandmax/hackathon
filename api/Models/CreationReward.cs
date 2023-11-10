namespace api.Models
{
    public class CreationReward
    {
        public bool FullReward { get; set; }

        public int Reward { get; set; }

        public CreationReward(bool fullReward, int reward)
        {
            FullReward = fullReward;
            Reward = reward;
        }
    }
}