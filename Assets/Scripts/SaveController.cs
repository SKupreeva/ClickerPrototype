using Logic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// class controls save system and initializes data

namespace Saves
{
    public class SaveController : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;
        [SerializeField] private GameController _gameController;

        private void Awake()
        {
            var data = SaveSystem.LoadGameData();

            if (data == null)
            {
                var businesses = new List<Business>();
                foreach (var item in _config.Businesses)
                {
                    var business = new Business(item);
                    businesses.Add(business);
                }
                _gameController.SetUp(businesses, 0);
            }
            else
            {
                var businesses = new List<Business>();
                foreach (var item in _config.Businesses)
                {
                    var business = new Business(item);
                    business.SetUp(data.Businesses.First(b => b.Name == business.Name));
                    businesses.Add(business);
                }
                _gameController.SetUp(businesses, data.Balance);
            }
        }


        private void OnApplicationPause()
        {
            SaveSystem.SaveGameData(new SaveData() { Balance = _gameController.Balance, Businesses = _gameController.Businesses });
        }
    }
}
