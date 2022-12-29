using UnityEngine;

namespace Logic
{
    public class Timer : MonoBehaviour
    {
        private float _delay;
        private float _currentValue;
        private bool _isPlaying;
        private Business _currentBusiness;

        public delegate void OnTimerValueChangedHandler(Timer timer, Business business);
        public event OnTimerValueChangedHandler OnTimerEnded;
        public event OnTimerValueChangedHandler OnTimerValueChanged;

        public float CurrentValue => _currentValue;
        public Business CurrentBusiness => _currentBusiness;

        private void Update()
        {
            if (!_isPlaying)
            {
                return;
            }

            _currentValue += Time.deltaTime;
            if (_currentValue >= _delay)
            {
                _isPlaying = false;
                OnTimerEnded?.Invoke(this, _currentBusiness);
            }
            else
            {
                OnTimerValueChanged?.Invoke(this, _currentBusiness);
            }
        }

        public void SetUp(float currentValue, Business business)
        {
            _currentBusiness = business;
            _currentValue = currentValue;
        }

        public void StartTimer(float delay, float fromValue)
        {
            _delay = delay;
            _currentValue = fromValue;
            _isPlaying = true;
        }
    }
}
