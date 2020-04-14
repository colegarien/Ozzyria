namespace Ozzyria.Game.Component
{
    public class ExperienceBoost : IComponent
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
