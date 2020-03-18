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

    public Stake stake;
    public Witch witch;
    public GameObject lightning;
    public GameObject ffwf;
    public GameObject ps;
    public Barrier barrier;
    public Flash flash;
    public SpellData lilithsBlessing = new SpellData();
    public SpellData praiseSatan = new SpellData();
    public SpellData unholyChant = new SpellData();
    public SpellData fightFireWithFire = new SpellData();
    public TextMeshProUGUI manaText;

    public List<SpellData> spelldata { get; set; }
    public int mana { get; set; } = 0;

    void Awake()
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
        barrier.gameObject.SetActive(true);
        barrier.CreateBarrier();
        lilithsBlessing.castButton.interactable = false;
        lilithsBlessing.active = false;
    }

    IEnumerator StopEnemies()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        var enemiesInRange = Physics2D.OverlapCircleAll(new Vector3(0.0f, -4.0f), 24.0f, LayerMask.GetMask("Enemies"));
        foreach (var enemy in enemiesInRange)
        {
            enemy.SendMessage("SetIdle");
        }
        foreach(var point in spawnPoints)
        {
            point.SendMessage("Deactivate");
        }

        yield return new WaitForSeconds(5.0f);

        enemiesInRange = Physics2D.OverlapCircleAll(new Vector3(0.0f, -4.0f), 24.0f, LayerMask.GetMask("Enemies"));
        foreach (var enemy in enemiesInRange)
        {
            enemy.SendMessage("RestorePreviousState");
        }
        foreach (var point in spawnPoints)
        {
            point.SendMessage("Activate");
        }
    }

    public void PraiseSatan()
    {
        Debug.Log("Cast PS!!!");
        StartCoroutine(KillAllEnemiesInRange());
        praiseSatan.castButton.interactable = false;
        praiseSatan.active = false;
    }

    IEnumerator KillAllEnemiesInRange()
    {
        GameManager.acceptsPlayerInput = false;
        witch.Hide();

        var spellsr = ps.GetComponent<SpriteRenderer>();
        spellsr.flipX = witch.spriteRenderer.flipX;

        var psr = witch.projectileInstance.GetComponent<SpriteRenderer>();
        var oldlayer = psr.sortingLayerID;
        psr.sortingLayerID = 0;
        psr.sortingOrder = 0;
        psr.enabled = false;

        ps.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        witch.Show();
        psr.enabled = true;

        var time = 0.0f;
        var anim = ps.GetComponent<Animator>();
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            var range = Mathf.Lerp(0.0f, 12.0f, time);

            var enemiesInRange = Physics2D.OverlapCircleAll(new Vector3(0.0f, -4.0f), range, LayerMask.GetMask("Enemies"));
            foreach (var enemy in enemiesInRange)
            {
                enemy.SendMessage("SetDead");
            }

            yield return null;

            time += Time.deltaTime;
        }

        ps.SetActive(false);
        psr.sortingLayerID = oldlayer;
        psr.sortingOrder = 9999;
        GameManager.acceptsPlayerInput = true;
    }

    public void UnholyChant()
    {
        Debug.Log("Cast UC!!!");
        StartCoroutine(KillPriest());
        unholyChant.castButton.interactable = false;
        unholyChant.active = false;
    }

    IEnumerator KillPriest()
    {
        var priest = GameObject.FindGameObjectWithTag("Priest");
        var lightningInstance = Instantiate(lightning);
        flash.QuickFlash(0.4f);

        if (priest != null)
        {
            var newPos = lightningInstance.transform.position;
            newPos.x = priest.transform.position.x;
            lightningInstance.transform.position = newPos;

            yield return null;

            priest.SendMessage("DeadByLightning");
        }

        yield return new WaitForSeconds(0.95f);

        Destroy(lightningInstance);
    }

    public void FightFireWithFire()
    {
        GameManager.acceptsPlayerInput = false;
        ffwf.SetActive(true);
        StartCoroutine(WaitForFFWFEnd());

        fightFireWithFire.castButton.interactable = false;
        fightFireWithFire.active = false;
    }

    private IEnumerator WaitForFFWFEnd()
    {
        yield return new WaitForSeconds(0.3f);

        var enemiesInRange = Physics2D.OverlapCircleAll(new Vector3(0.0f, -4.0f), 1.0f, LayerMask.GetMask("Enemies"));
        foreach (var enemy in enemiesInRange)
        {
            enemy.SendMessage("SetDead");
        }
        stake.ResetDurability();

        var anim = ffwf.GetComponent<Animator>();
        while(!anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            yield return null;
        }

        ffwf.SetActive(false);
        GameManager.acceptsPlayerInput = true;
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
