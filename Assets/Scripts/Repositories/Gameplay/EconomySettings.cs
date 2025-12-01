using UnityEngine;

namespace Repositories.Gameplay
{
    [CreateAssetMenu(fileName = "EconomySettings", menuName = "Settings/Economy Settings")]
    public class EconomySettings : ScriptableObject
    {
        [Header("Starting Gold")]
        [SerializeField] private int _initialGold = 500;

        [Header("Building Income Per Tick")]
        [SerializeField] private int _houseBaseIncomePerTick = 1;
        [SerializeField] private int _farmBaseIncomePerTick = 3;
        [SerializeField] private int _mineBaseIncomePerTick = 5;

        [Header("Building Base Costs")]
        [SerializeField] private int _houseBaseCost = 100;
        [SerializeField] private int _farmBaseCost = 150;
        [SerializeField] private int _mineBaseCost = 200;

        public int InitialGold => _initialGold;
        public int HouseBaseIncomePerTick => _houseBaseIncomePerTick;
        public int FarmBaseIncomePerTick => _farmBaseIncomePerTick;
        public int MineBaseIncomePerTick => _mineBaseIncomePerTick;
        public int HouseBaseCost => _houseBaseCost;
        public int FarmBaseCost => _farmBaseCost;
        public int MineBaseCost => _mineBaseCost;
    }
}

