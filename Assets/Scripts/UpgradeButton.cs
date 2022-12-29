using Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// class controls upgrade button ui and triggers onclick event

namespace UIControlls
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _businessName;
        [SerializeField] private TextMeshProUGUI _incomeMultiplier;
        [SerializeField] private TextMeshProUGUI _upgradePrice;
        [SerializeField] private RectTransform _purchasedPanel;

        public delegate void OnTryPurchaseUpgradeHandler(Upgrade upgrade, Business business);
        public event OnTryPurchaseUpgradeHandler OnTryPurchaseUpgrade;

        private Upgrade _currentUpgrade;
        private Business _business;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void SetUp(Upgrade upgrade, Business business)
        {
            _currentUpgrade = upgrade;
            _business = business;

            _purchasedPanel.gameObject.SetActive(upgrade.IsPurchased);

            UpdateInfo(upgrade);
        }

        private void OnButtonClicked()
        {
            OnTryPurchaseUpgrade?.Invoke(_currentUpgrade, _business);
        }

        private void UpdateInfo(Upgrade upgrade)
        {
            _businessName.text = upgrade.Name;
            _incomeMultiplier.text = upgrade.IncomeMultiplierInPercents.ToString();
            _upgradePrice.text = upgrade.Price.ToString();

            _purchasedPanel.gameObject.SetActive(upgrade.IsPurchased);
            _button.interactable = !upgrade.IsPurchased;
        }
    }
}
