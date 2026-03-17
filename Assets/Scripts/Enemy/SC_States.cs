using UnityEngine;

public abstract class SC_States : MonoBehaviour
{
    public abstract void OnEnterState(SC_FSMController fsmController);
    public abstract void OnUpdateState();
    public abstract void OnExitState();
    
}
