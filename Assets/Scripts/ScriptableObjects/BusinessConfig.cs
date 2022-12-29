using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BusinessConfig")]
public class BusinessConfig : ScriptableObject
{
    public string Name;
    public int IncomeDelayInSecs;
    public int BasePrice;
    public int BaseIncome;
    public List<Upgrade> Upgrades;
    public bool IsPurchasedOnStart;
}
