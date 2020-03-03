using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Spellbook : MonoBehaviour
{
    [System.Serializable]
    public class SpellData
    {
        public int basePrice;
        public int multiplier;
        public int buyCount;
        public Button bookButton;
        public Button castButton;
        public TextMeshProUGUI priceText;
        [HideInInspector]
        public bool active = false;

        public int cost { get => basePrice * multiplier * (buyCount + 1); }
        public void UpdatePrice()
        {
            priceText.text = string.Format("Mana: {0}", cost);
        }
    }

    public SpellData lilithsBlessing = new SpellData();
    public SpellData praiseSatan = new SpellData();
    public SpellData unholyChant = new SpellData();
    public SpellData fightFireWithFire = new SpellData();
    public TextMeshProUGUI manaText;

    private List<SpellData> spelldata;
    private int mana = 10000;

    void Start()
    {
        spelldata = new List<SpellData> { lilithsBlessing, praiseSatan, unholyChant, fightFireWithFire };
    }

    public void UpdateShop()
    {
        UpdateManaText();
        foreach(var spell in spelldata)
        {
            spell.UpdatePrice();
            spell.bookButton.interactable = (mana >= spell.cost && !spell.active);
        }
    }

    public void UpdateActiveSpells()
    {
        foreach (var spell in spelldata)
        {
            spell.castButton.interactable = spell.active;
        }
    }

    public void UpdateManaText()
    {
        manaText.text = string.Format("Available mana: {0}", mana);
    }

    public void AddMana(int additionalMana)
    {
        mana += additionalMana;
    }

    public void BuyLilithsBlessing()
    {
        Buy(lilithsBlessing);
    }

    public void BuyPraiseSatan()
    {
        Buy(praiseSatan);
    }

    public void BuyUnholyChant()
    {
        Buy(unholyChant);
    }

    public void BuyFightFireWithFire()
    {
        Buy(fightFireWithFire);
    }

    public void LilithsBlessing()
    {
        Debug.Log("Cast LB!!!");
        lilithsBlessing.castButton.interactable = false;
    }

    public void PraiseSatan()
    {
        Debug.Log("Cast PS!!!");
        praiseSatan.castButton.interactable = false;
    }

    public void UnholyChant()
    {
        Debug.Log("Cast UC!!!");
        unholyChant.castButton.interactable = false;
    }

    public void FightFireWithFire()
    {
        Debug.Log("Cast FFWF!!!");
        fightFireWithFire.castButton.interactable = false;
    }

    private void Buy(SpellData obj)
    {
        mana -= obj.cost;
        obj.buyCount += 1;
        UpdateShop();
        obj.bookButton.interactable = false;
        obj.active = true;
    }
}
