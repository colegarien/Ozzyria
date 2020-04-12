namespace Ozzyria.Game.Component
{
    public class ExperienceBoost
    {
        public int Experience { get; set; } = 10;
        public bool HasBeenAbsorbed { get; set; } = false;

        public void AbsorbInto(Player player)
        {
            if (HasBeenAbsorbed)
            {
                return;
            }

            player.Stats.AddExperience(Experience);
            HasBeenAbsorbed = true;
        }
    }
}
