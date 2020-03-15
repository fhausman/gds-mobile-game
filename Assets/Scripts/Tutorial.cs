﻿using System.Collections;
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
        var proj = GameObject.FindGameObjectsWithTag("Projectile");
        if(proj.Length == 0)
        {
            tut.states.ChangeState(2);
        }

    }
}

public class FirstPeasantTutorialState : IState
{
    const string message =
        "Hit the peasant before he reaches stack.";

    public Tutorial tut;


    public void Exit()
    {
    }

    public void Init()
    {
        tut.uiManager.SetTutorialText(message);
        tut.witch.SetInactive();
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            tut.witch.SetActive();
            tut.states.ChangeState(1);
        }
    }
}

public class Tutorial : MonoBehaviour
{
    public UIManager uiManager;
    public SpawnPoint spawnPointLeft;
    public SpawnPoint spawnPointRight;
    public Witch witch;

    public StateMachine states { get; private set; } = new StateMachine();

    //chwila na widok planszy

    //Kiedy gra się odpala na początku widać tylko stos, bez tłuszczy.Następuje freeze i napis
    //To shoot spark touch screen either on left or right. Longer holding will change parabola of the spark. 

    //Ponownie, dopóki gracz nie zrobi tego co zostało napisane tutorial nie idzie dalej.Pojawia się pierwszy basic wieśniak. Znowu freeze- pojawia się napis,
    //Hit the peasant before he reaches stack.

    //że trzeba zabić wieśniaka zanim podejdzie z pochodnią do stosu.Freeze znika, wieśniak idzie do stosu.Kiedy gracz go zestrzeli pojawia się gruby wieśniak z drugiej strony.Pojawia się napis,
    //Bigger peasant is more resistant. You need to hit him twice. 


    //Kończy się freeze, wieśniak zaczyna iść.Kiedy gracz zastrzeli wieśniaka pojawia się ksiądz i basic wieśniak. Gra zatrzymuje się.
    //Priest is not holding torch, but he is giving speed to peasants that are going past him.

    //Gra rusza, wieśniak idzie, mija księdza i przyspiesza. Po zastrzeleniu ich pojawia się BABA. Gra freezuje. 
    //Spinster is not coming close to stack, but she is throwing torches. If her torch hits the stack you need to hit it before it will ignite the stack.

    //Po zestrzeleniu baby, graczowi odblokowuje się zaklęcie PRAISE SATAN
    //After dying you can prepare spell in spell book for mana you have collected during game, that you can use in next game. Right now you have prepared spell PRAISE SATAN that is destroying all enemies on stage.

    //Rusza tłum z prawej i z lewej.Gracz musi wcisnąć zaklęcie. Kiedy gracz je kliknie, wszyscy giną. 

    //Now you are prepared to face angry mob! Good luck!

    void Start()
    {
        Debug.Log("Start");
        states.AddState(0, new InitialTutorialState { tut = this });
        states.AddState(1, new WaitForProjectileToExplode { tut = this });
        states.AddState(2, new FirstPeasantTutorialState { tut = this });
        states.ChangeState(0);
    }

    void Update()
    {
        states.Update();
    }
}
