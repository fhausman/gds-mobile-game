using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialTutorialState : IState
{
    const string message =
        "To shoot spark touch screen either on left or right. Longer holding will change parabola of the spark.";

    public Tutorial tut;
    

    public void Exit()
    {
    }

    public void Init()
    {
        tut.uiManager.ShowTutorial();
        tut.uiManager.SetTutorialText(message);
        tut.witch.SetInactive();
        tut.stake.Deactivate();
        tut.spawnPointLeft.Deactivate();
        tut.spawnPointRight.Deactivate();
    }

    public void Update()
    {
        if(Input.GetMouseButton(0))
        {
            tut.witch.SetActive();
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
        "Hit the peasant before he reaches the stake.\nFor sake of tutorial you're immune now";

    public Tutorial tut;


    public void Exit()
    {
    }

    public void Init()
    {
        tut.uiManager.SetTutorialText(message);
        tut.spawnPointLeft.SpawnMob(SpawnPoint.Enemies.BasicMob);
    }

    public void Update()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            tut.states.ChangeState(3);

    }
}

public class StrongPeasantTutorialState : IState
{
    const string message =
        "Bigger peasant is more resistant. You need to hit him twice.";

    public Tutorial tut;


    public void Exit()
    {
    }

    public void Init()
    {
        tut.uiManager.SetTutorialText(message);
        tut.spawnPointRight.SpawnMob(SpawnPoint.Enemies.StrongMob);
    }

    public void Update()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            tut.states.ChangeState(4);
    }
}

public class PriestTutorialState : IState
{
    const string message =
        "Priest is not holding torch, but he is giving speed to peasants that are going past him.";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
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
        "Spinster is not coming close to stake, but she is throwing torches. If her torch hits the stake it will receive single amount of damage.";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
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
        "After each battle you'll receive some mana, you can spend it for powerful spells. Look for them in main menu after you die.";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
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
        "Congratulations!\nYou're now ready to fight the mob!\nTouch the screen when you're ready for battle...";

    public Tutorial tut;
    private bool checkState = false;

    public void Exit()
    {
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
            tut.gameManager.tutorialEnabled = false;
            tut.gameManager.Save();
            tut.gameManager.RestartGame();
            tut.gameObject.SetActive(false);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f);

        checkState = true;
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
        states.ChangeState(0);
    }

    void Update()
    {
        states.Update();
    }
}
