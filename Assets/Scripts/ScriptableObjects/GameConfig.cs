using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameConfig")]
public class GameConfig : ScriptableObject
{
    public List<BusinessConfig> Businesses;
}
