using UnityEngine;

public class SC_FSMController : MonoBehaviour
{
    private SC_States currentState;
    private SC_PatrolsState PatrolState { get; set; } //se puede acceder a este estado desde otras clases y modificarlo
    
    
    
    private void Awake()
    {
        PatrolState = GetComponent<SC_PatrolsState>();
        ChangeState(PatrolState);
        
    }

    public void ChangeState(SC_States newState)
    {
        currentState?.OnExitState();
        currentState = newState;
        currentState.OnEnterState(this);

    }

    private void Update()
    {
        currentState.OnUpdateState();
    }
}
