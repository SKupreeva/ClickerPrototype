// class realizes balance logic

namespace Logic
{
    public class BalanceController
    {
        public delegate void OnBalanceChangedHandler();
        public event OnBalanceChangedHandler OnBalanceChanged;

        private float _balance;
        public float Balance
        {
            get => _balance;
            set
            {
                if (_balance == value)
                {
                    return;
                }
                _balance = value;
                OnBalanceChanged?.Invoke();
            }
        }

        public void IncreaseBalance(float points)
        {
            Balance += points;
        }

        public bool TryToPurchase(int price)
        {
            if (Balance >= price)
            {
                Balance -= price;
                return true;
            }
            return false;
        }

        public void SetUp(float newValue)
        {
            Balance = newValue;
        }
    }
}
