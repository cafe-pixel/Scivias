using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SC_InvestigateState : SC_States
{
    private SC_FSMController myController;
    private SC_SensorSystem sensorSystem;
    private NavMeshAgent agent;
    private GameObject target => FirstPersonController.player;
    private SC_PatrolsState patrols;
    private SC_ChaseAttackState chase;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        sensorSystem = GetComponent<SC_SensorSystem>();
        myController = GetComponent<SC_FSMController>();
        agent = GetComponent<NavMeshAgent>();
        patrols = GetComponent<SC_PatrolsState>();
        chase = GetComponent<SC_ChaseAttackState>();
    }

    public override void OnEnterState(SC_FSMController fsmController)
    {
        myController = fsmController;
        animator.SetTrigger("lookAround");
    }

    public override void OnUpdateState()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            myController.ChangeState(patrols);
        }
    }

    public override void OnExitState()
    {
        agent.isStopped = true;
        //target = null;
        StopAllCoroutines();
        agent.ResetPath();
    }

    private void OnEnable()
    {
        SC_ChaseAttackState.OnPlayerLost += OnPlayerLost;
        SC_SensorSystem.OnPlayerFound += OnPlayerFound;
    }

    private void OnPlayerFound(GameObject player)
    {
        myController.ChangeState(chase);
    }

    private void OnPlayerLost(Vector3 lastPlayerPosition)
    {
        //StartCoroutine(SearchPlayer(lastPlayerPosition));
        sensorSystem.FoundPlayer = false;
        agent.SetDestination(sensorSystem.LastPlayerPosition);
    }
}