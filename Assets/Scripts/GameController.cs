using System.Collections.Generic;
using UIControlls;
using UnityEngine;

// class controls app logic

namespace Logic
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Timer _timerPrefab;
        [SerializeField] private UIController _uiController;

        private BalanceController _balance;
        private List<Business> _businesses = new List<Business>();
        private List<Timer> _timers = new List<Timer>();

        public float Balance => _balance == null ? 0 : _balance.Balance;
        public List<Business> Businesses => _businesses;

        private void OnDestroy()
        {
            foreach (var item in _businesses)
            {
                foreach (var upgrade in item.Upgrades)
                {
                    upgrade.IsPurchased = false;
                }
            }

            foreach (var timer in _timers)
            {
                timer.OnTimerEnded -= OnTimerEnded;
                timer.OnTimerValueChanged -= OnTimerValueChanged;
            }
            _balance.OnBalanceChanged -= OnBalanceChanged;
        }

        public void SetUp(List<Business> businesses, float balance)
        {
            _businesses = new List<Business>(businesses);
            _timers.Clear();

            _balance = new BalanceController();
            _balance.OnBalanceChanged += OnBalanceChanged;
            _balance.SetUp(balance);
            OnBalanceChanged();

            _uiController.SetUp(_businesses);

            foreach (var item in _businesses)
            {
                var timer = Instantiate(_timerPrefab, transform);
                timer.SetUp(item.ProgressValue, item);
                timer.OnTimerEnded += OnTimerEnded;
                timer.OnTimerValueChanged += OnTimerValueChanged;
                _timers.Add(timer);

                if (item.IsPurchased)
                {
                    timer.StartTimer(item.IncomeDelay, item.ProgressValue >= 1 ? 0 : item.ProgressValue);
                }
            }
        }

        public void TryLevelUpBusiness(Business business)
        {
            if (!_balance.TryToPurchase(business.NextLevelPrice))
            {
                return;
            }

            foreach (var timer in _timers)
            {
                if(timer.CurrentBusiness.Name == business.Name)
                {
                    timer.StartTimer(business.IncomeDelay, business.ProgressValue >= 1 ? 0 : business.ProgressValue);
                }
            }

            business.IsPurchased = true;
            business.LevelUp();
            _uiController.UpdateBusinessItem(business);
        }

        public void TryUpgradeBusiness(Business business, Upgrade upgrade)
        {
            if (!_balance.TryToPurchase(upgrade.Price))
            {
                return;
            }

            foreach (var item in business.Upgrades)
            {
                if (upgrade.Name == item.Name)
                {
                    item.IsPurchased = true;
                }
            }

            foreach (var timer in _timers)
            {
                if (timer.CurrentBusiness.Name == business.Name)
                {
                    timer.StartTimer(business.IncomeDelay, business.ProgressValue >= 1 ? 0 : business.ProgressValue);
                }
            }

            business.Upgrade(upgrade);
            _uiController.UpdateBusinessItem(business);
        }

        private void OnTimerEnded(Timer timer, Business business)
        {
            _balance.IncreaseBalance(business.Income);
            timer.StartTimer(business.IncomeDelay, 0);
        }

        private void OnTimerValueChanged(Timer timer, Business business)
        {
            business.ProgressValue = timer.CurrentValue;
        }

        private void OnBalanceChanged()
        {
            _uiController.UpdateBalance(_balance.Balance);
        }
    }
}
