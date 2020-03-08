using System.Collections.Generic;

public interface IState
{
    void Init();
    void Update();
    void Exit();
}

public class EmptyState : IState
{
    public void Exit()
    {
    }

    public void Init()
    {
    }

    public void Update()
    {
    }
}

public class StateMachine
{
    IState currentState = new EmptyState();
    Dictionary<int, IState> states = new Dictionary<int, IState>();

    public int currentStateId { get; private set; } = -1;

    public void AddState(int id, IState state)
    {
        states.Add(id, state);
    }

    public void ChangeState(int id)
    {
        if(!states.ContainsKey(id))
        {
            throw new System.Exception(string.Format("Invalid state requested! {0}", id));
        }

        currentState.Exit();
        var newState = states[id];
        newState.Init();
        currentStateId = id;
        currentState = newState;
    }

    public void Update()
    {
        currentState.Update();
    }
}
