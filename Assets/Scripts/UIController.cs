using Logic;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// class controls all app ui

namespace UIControlls
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        [SerializeField] private TextMeshProUGUI _balance;
        [SerializeField] private Transform _content;
        [SerializeField] private BusinessItem _businessItemPrefab;

        private List<BusinessItem> _businessItems = new List<BusinessItem> ();

        private void OnDestroy()
        {
            foreach (var item in _businessItems)
            {
                item.OnTryLevelUp -= OnTryLevelUp;
                foreach (var button in item.UpgradeButtons)
                {
                    button.OnTryPurchaseUpgrade -= OnTryPurchaseUpgrade;
                }
            }
        }

        public void SetUp(List<Business> businesses)
        {
            Clean();
            foreach(var item in businesses)
            {
                var newItem = Instantiate(_businessItemPrefab, _content);
                newItem.SetUp(item);
                newItem.OnTryLevelUp += OnTryLevelUp;
                foreach(var button in newItem.UpgradeButtons)
                {
                    button.OnTryPurchaseUpgrade += OnTryPurchaseUpgrade;
                }
                _businessItems.Add(newItem);
            }
        }

        public void UpdateBalance(float newBalance)
        {
            _balance.text = newBalance.ToString();
        }

        public void UpdateBusinessItem(Business business)
        {
            foreach (var item in _businessItems)
            {
                if (item.CurrentBusiness.Name == business.Name)
                {
                    item.UpdateInfo();
                }
            }
        }

        private void Clean()
        {
            foreach(Transform child in _content)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnTryPurchaseUpgrade(Upgrade upgrade, Business business)
        {
            _gameController.TryUpgradeBusiness(business, upgrade);
        }

        private void OnTryLevelUp(Business business)
        {
            _gameController.TryLevelUpBusiness(business);
        }
    }
}
