using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Spellbook : MonoBehaviour
{
    [System.Serializable]
    public struct SpellData
    {
        public int basePrice;
        public int multiplier;
        public int buyCount;
        public Button bookButton;
        public Button castButton;
    }

    public SpellData lilithsBlessing;
    public SpellData praiseSatan;
    public SpellData unholyChant;
    public SpellData fightFireWithFire;

    private int mana = 0;

    void Start()
    {
    }

    public void LilithsBlessing()
    {
        lilithsBlessing.castButton.interactable = false;
    }

    public void PraiseSatan()
    {
        praiseSatan.castButton.interactable = false;
    }

    public void UnholyChant()
    {
        unholyChant.castButton.interactable = false;
    }

    public void FightFireWithFire()
    {
        fightFireWithFire.castButton.interactable = false;
    }
}
