using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class SC_PatrolsState : SC_States
{
    private SC_FSMController myController;
    private bool patrolActive;
    [SerializeField] private Transform patrolRoute;
    private List<Vector3> patrolRoutePoints = new();//lista donde se añadirán todos los puntos de la ruta
    private int currentPatrolPoint = 0;

    [SerializeField] private AudioSource steps;

    private SC_ChaseAttackState chase;
    
    
    private NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        chase = GetComponent<SC_ChaseAttackState>();
        foreach (Transform points in patrolRoute)
        {
            patrolRoutePoints.Add(points.position);
        }
    }

    private void OnEnable()
    {
        SC_TriggerEnemyBegginMove.OnTriggerEnemy += BeginMovement;
    }
    private void OnDisable()
    {
        SC_TriggerEnemyBegginMove.OnTriggerEnemy -= BeginMovement;
    }

    private void BeginMovement()
    {
        patrolActive = true;
        StartCoroutine(PatrolAndWait());
    }

    private IEnumerator PatrolAndWait()
    {
        //Sale por pantalla al HUD "Te sientes observado" durante un segundo
        yield return new WaitForSeconds(3);
        //Comienza a caminar haciendo ruido, de manera que se puede saber con antelación cuando viene
        while (patrolActive)
        {
            agent.isStopped = false;
            agent.SetDestination(patrolRoutePoints[currentPatrolPoint]);
            yield return new WaitUntil(ReachedDestination);
            yield return new WaitForSeconds(Random.Range(0.2f, 1.5f));
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolRoutePoints.Count;

        }
    }
    
    private bool ReachedDestination()
    {
        return !agent.pathPending &&
               agent.remainingDistance <= agent.stoppingDistance; //si no tienes pendiente un camino y la distancia
        //que queda hasta el punto es menor o igual al de tu parada
    }

    public override void OnEnterState(SC_FSMController fsmController)
    {
        myController = fsmController;
    }

    public override void OnUpdateState()
    {
        //Mientras está haciendo la patrulla investiga a ver si encuentra al jugador
        //Si recibe el jugador sale del estado actual al de chase
        //Si en chase pierde el jugador vuelve a patrullar
        //Si en chase está muy cerca le embiste para comerselo
        myController.ChangeState(chase);
    }

    public override void OnExitState()
    {
        //paras todas las cosas que tuvieras encendidas
        patrolActive = false;
        agent.isStopped = true;
        agent.ResetPath();
        StopAllCoroutines();
    }
}
