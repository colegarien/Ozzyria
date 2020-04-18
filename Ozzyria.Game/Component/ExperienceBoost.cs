namespace Ozzyria.Game.Component
{
    public class ExperienceBoost : Component
    {
        public override ComponentType Type() => ComponentType.ExperienceBoost;
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
