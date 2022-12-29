using System.Collections.Generic;

// class for main runtime business logic

namespace Logic
{
    public class Business
    {
        public string Name;
        public int IncomeDelay;
        public int Level;
        public bool IsPurchased;
        public List<Upgrade> Upgrades;
        public float ProgressValue;

        public float Income => Level * _baseIncome * _upgradesMultiplier;
        public int NextLevelPrice => (Level + 1) * _basePrice;

        private int _baseIncome;
        private int _basePrice;
        private float _upgradesMultiplier
        {
            get
            {
                var multiplier = 0;
                foreach (var upgrade in Upgrades)
                {
                    if (upgrade.IsPurchased)
                    {
                        multiplier += 1 + upgrade.IncomeMultiplierInPercents / 100;
                    }
                }

                if (multiplier > 0)
                {
                    return multiplier;
                }

                return 1;
            }
        }

        public Business(BusinessConfig config)
        {
            Name = config.Name;
            IncomeDelay = config.IncomeDelayInSecs;
            _baseIncome = config.BaseIncome;
            _basePrice = config.BasePrice;
            IsPurchased = config.IsPurchasedOnStart;
            Upgrades = config.Upgrades;
            Level = IsPurchased ? 1 : 0;
            ProgressValue = 0;
        }

        public void SetUp(Business business)
        {
            IsPurchased = business.IsPurchased;
            Upgrades = business.Upgrades;
            Level = business.Level;
            ProgressValue = business.ProgressValue;
        }

        public void LevelUp()
        {
            Level++;
        }

        public void Upgrade(Upgrade upgrade)
        {
            foreach (var item in Upgrades)
            {
                if (item == upgrade)
                {
                    item.IsPurchased = true;
                }
            }
        }
    }
}
