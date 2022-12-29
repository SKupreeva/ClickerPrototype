using Logic;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// class controls business item ui and triggers buttons clicks

namespace UIControlls
{
    public class BusinessItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _businessName;
        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _currentLevel;
        [SerializeField] private TextMeshProUGUI _currentIncome;
        [SerializeField] private TextMeshProUGUI _levelUpPrice;
        [SerializeField] private List<UpgradeButton> _upgradeButtons;
        [SerializeField] private Timer _timer;

        public List<UpgradeButton> UpgradeButtons => _upgradeButtons;

        public delegate void OnTryLevelUpHandler(Business business);
        public event OnTryLevelUpHandler OnTryLevelUp;

        private Business _currentBusiness;

        public Business CurrentBusiness => _currentBusiness;

        private void OnDisable()
        {
            _timer.OnTimerValueChanged -= OnTimerValueChanged;
            _timer.OnTimerEnded -= OnTimerEnded;
        }

        public void SetUp(Business business)
        {
            _currentBusiness = business;

            _timer.SetUp(business.ProgressValue, business);
            _timer.OnTimerValueChanged += OnTimerValueChanged;
            _timer.OnTimerEnded += OnTimerEnded;

            UpdateInfo();
        }

        public void OnLevelUpButtonClicked()
        {
            OnTryLevelUp?.Invoke(_currentBusiness);
        }

        public void UpdateInfo()
        {
            _businessName.text = _currentBusiness.Name;
            _fill.fillAmount = 0;
            _currentLevel.text = _currentBusiness.Level.ToString();
            _currentIncome.text = _currentBusiness.Income.ToString();
            _levelUpPrice.text = _currentBusiness.NextLevelPrice.ToString();
            for (var i = 0; i < _upgradeButtons.Count; i++)
            {
                _upgradeButtons[i].SetUp(_currentBusiness.Upgrades[i], _currentBusiness);
            }

            if (_currentBusiness.IsPurchased)
            {
                _timer.StartTimer(_currentBusiness.IncomeDelay, _currentBusiness.ProgressValue >= 1 ? 0 : _currentBusiness.ProgressValue);
            }
        }

        private void OnTimerValueChanged(Timer timer, Business business)
        {
            _fill.fillAmount = _timer.CurrentValue / _currentBusiness.IncomeDelay;
        }

        private void OnTimerEnded(Timer timer, Business business)
        {
            _fill.fillAmount = 0;
            _timer.StartTimer(_currentBusiness.IncomeDelay, 0);
        }
    }
}
