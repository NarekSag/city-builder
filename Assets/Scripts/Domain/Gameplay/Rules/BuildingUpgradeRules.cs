namespace Domain.Gameplay.Rules
{
    public static class BuildingUpgradeRules
    {
        public static bool CanUpgrade(Models.Building building, int maxLevel = 3)
        {
            if (building == null)
            {
                return false;
            }

            return building.CanUpgrade(maxLevel);
        }
    }
}

