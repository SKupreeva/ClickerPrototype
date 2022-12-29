using Logic;
using System;
using System.Collections.Generic;

namespace Saves
{
    [Serializable]
    public class SaveData
    {
        public float Balance;
        public List<Business> Businesses;
    }
}
