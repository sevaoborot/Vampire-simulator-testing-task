using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "Vampire Sim/New player")]
public class PlayerData : ScriptableObject
{
    public float maxHealth; //boundaries
    public float speed;
    public List<WeaponData> weapons; //can I use it like these?

    //need to add boundaries + should I make a separate SO?
    public float expAmountForUnlockingLevel;
    public float levelIncrement;
}
