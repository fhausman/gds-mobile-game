using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialTutorialState : IState
{
    const string message =
        "To shoot a spark touch the screen on either left or right. The longer you hold the more the parabola of the spark will change.";

    public Tutorial tut;
    private bool activated = false;

    public void Exit()
    {
        activated = false;
    }

    public void Init()
    {
        tut.uiManager.ShowTutorial();
        tut.witch.SetActive();
        tut.stake.Deactivate();
        tut.spawnPointLeft.Deactivate();
        tut.spawnPointRight.Deactivate();
    }

    public void Update()
    {
        if (tut.witch.anim.GetCurrentAnimatorStateInfo(0).IsName("Intro"))
            return;
        else if(!activated)
        {
            activated = true;
            tut.uiManager.SetTutorialText(message);
        }

        if(Input.GetMouseButton(0))
        {
            tut.states.ChangeState(1);
        }
    }
}

public class WaitForProjectileToExplode : IState
{
    public Tutorial tut;

    public void Exit()
    {
    }

    public void Init()
    {
        tut.uiManager.SetTutorialText("");
    }

    public void Update()
    {
        var projs = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(var proj in projs)
        {
            if(proj.GetComponent<Projectile>().anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
            {
                tut.states.ChangeState(2);
            }
        }
    }
}

public class FirstPeasantTutorialState : IState
{
    const string message =
        "Hit the peasant before he reaches the stake.\nFor the duration of the tutorial you're immune from all damage.";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
        checkState = false;
    }

    public void Init()
    {
        GameManager.acceptsPlayerInput = false;
        tut.uiManager.SetTutorialText(message);
        tut.StartCoroutine(SpawnEnemies());
    }

    public void Update()
    {
        if (!checkState)
            return;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            tut.states.ChangeState(3);

    }

    private IEnumerator SpawnEnemies()
    {
        checkState = true;
        tut.spawnPointLeft.SpawnMob(SpawnPoint.Enemies.BasicMob);

        yield return new WaitForSeconds(2.0f);

        GameManager.acceptsPlayerInput = true;
    }
}

public class StrongPeasantTutorialState : IState
{
    const string message =
        "Bigger peasant is more resistant. You need to hit him twice.";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
        checkState = false;
    }

    public void Init()
    {
        GameManager.acceptsPlayerInput = false;
        tut.uiManager.SetTutorialText(message);
        tut.StartCoroutine(SpawnEnemies());
    }

    public void Update()
    {
        if (!checkState)
            return;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            tut.states.ChangeState(4);
    }

    private IEnumerator SpawnEnemies()
    {
        checkState = true;
        tut.spawnPointRight.SpawnMob(SpawnPoint.Enemies.StrongMob);

        yield return new WaitForSeconds(2.0f);

        GameManager.acceptsPlayerInput = true;
    }
}

public class PriestTutorialState : IState
{
    const string message =
        "Priest is not holding a torch, but he is giving more speed to the peasants that are going past him.";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
        checkState = false;
    }

    public void Init()
    {
        GameManager.acceptsPlayerInput = false;
        tut.uiManager.SetTutorialText(message);
        tut.StartCoroutine(SpawnEnemies());
    }

    public void Update()
    {
        if (!checkState)
            return;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var priest = GameObject.FindGameObjectsWithTag("Priest");
        if (enemies.Length == 0 && priest.Length == 0)
            tut.states.ChangeState(5);
    }
    
    private IEnumerator SpawnEnemies()
    {
        tut.spawnPointLeft.SpawnPriest();

        yield return new WaitForSeconds(3.0f);

        checkState = true;
        tut.spawnPointLeft.SpawnMob(SpawnPoint.Enemies.BasicMob);

        yield return new WaitForSeconds(2.0f);

        GameManager.acceptsPlayerInput = true;
    }
}

public class SlybootTutorialState : IState
{
    const string message =
        "The spinster will not come close to the stake, but she will throw torches. If her torch hits the stake, the stake will receive a single amount of damage.";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
        checkState = false;
    }

    public void Init()
    {
        GameManager.acceptsPlayerInput = false;
        tut.uiManager.SetTutorialText(message);
        tut.StartCoroutine(SpawnEnemies());
    }

    public void Update()
    {
        if (!checkState)
            return;

        var enemies = GameObject.FindGameObjectsWithTag("Slyboot");
        if (enemies.Length == 0)
            tut.states.ChangeState(6);
    }

    private IEnumerator SpawnEnemies()
    {
        tut.spawnPointRight.SpawnSlyboot();

        yield return new WaitForSeconds(5.0f);

        checkState = true;

        GameManager.acceptsPlayerInput = true;
    }
}

public class SpellbookInformationTutorialState : IState
{
    const string message =
        "After each battle you'll receive some mana so you can spend it for powerful spells. Look for them in the main menu after you die.";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
        checkState = false;
    }

    public void Init()
    {
        GameManager.acceptsPlayerInput = false;
        tut.uiManager.SetTutorialText(message);
        tut.StartCoroutine(Delay());
    }

    public void Update()
    {
        if (!checkState)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            tut.states.ChangeState(7);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f);

        checkState = true;
    }
}

public class FinalTutorialState : IState
{
    const string message =
        "Congratulations! You're now ready to fight the mob! Touch the screen when you're ready for battle..";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
        checkState = false;
    }

    public void Init()
    {
        GameManager.acceptsPlayerInput = false;
        tut.uiManager.SetTutorialText(message);
        tut.StartCoroutine(Delay());
    }

    public void Update()
    {
        if (!checkState)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Score.value = 0;
            tut.gameManager.tutorialEnabled = false;
            tut.gameManager.Save();
            tut.stake.Activate();
            tut.stake.ResetDurability();
            tut.spawnPointLeft.Activate();
            tut.spawnPointRight.Activate();
            tut.uiManager.PlayUi();
            tut.witch.StartCoroutine(InputDelay());
            tut.uiManager.SetTutorialText("");
            tut.gameObject.SetActive(false);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f);

        checkState = true;
    }
    
    private IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.5f);

        GameManager.acceptsPlayerInput = true;
    }
}


public class Tutorial : MonoBehaviour
{
    public GameManager gameManager;
    public UIManager uiManager;
    public SpawnPoint spawnPointLeft;
    public SpawnPoint spawnPointRight;
    public Witch witch;
    public Stake stake;

    public StateMachine states { get; private set; } = new StateMachine();

    void Start()
    {
        Debug.Log("Start");
        states.AddState(0, new InitialTutorialState { tut = this });
        states.AddState(1, new WaitForProjectileToExplode { tut = this });
        states.AddState(2, new FirstPeasantTutorialState { tut = this });
        states.AddState(3, new StrongPeasantTutorialState { tut = this });
        states.AddState(4, new PriestTutorialState { tut = this });
        states.AddState(5, new SlybootTutorialState { tut = this });
        states.AddState(6, new SpellbookInformationTutorialState { tut = this });
        states.AddState(7, new FinalTutorialState { tut = this });
        gameObject.SetActive(false);
    }

    void Update()
    {
        states.Update();
    }

    public void Begin()
    {
        states.ChangeState(0);
    }
}
