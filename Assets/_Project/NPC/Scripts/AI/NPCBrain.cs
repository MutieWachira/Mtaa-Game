using UnityEngine;
using UnityEngine.AI;

///<summary>
/// Controls high-level NPC desicion making.
/// 
/// This component decides what the NPC should do.
/// It does not directly control animation or low-level movement.
/// </summary>

[RequireComponent(typeof(NPCMovement))]
public sealed class NPCBrain : MonoBehaviour
{
    public NPCState CurrentState { get; private set; }
    private NPCMovement _movement;

    [SerializeField]
    private float decisionInterval = 5f;

    [SerializeField]
    private float roamingRadius = 20f;

    private float _nextDecisionTime;

    private void Awake()
    {
        _movement = GetComponent<NPCMovement>();
        SetState(NPCState.Idle);
    }

    private void Update()
    {
        EvaluateBehaviour();
    }

    private void EvaluateBehaviour()
    {
        switch(CurrentState)
        {
            case NPCState.Idle:
            HandleIdle();
            break;

            case NPCState.GoingToDestination:
            HandleDestination();
            break;

            default:
            break;
        }
    }

    private void HandleIdle()
    {
        if(Time.time < _nextDecisionTime)
        {
            return;
        }
        _nextDecisionTime = Time.time + decisionInterval;

        if (TryFindRoamingDestination( out Vector3 destination))
        {
            _movement.MoveTo(destination);
            SetState(NPCState.GoingToDestination);
        }
    }

    private bool TryFindRoamingDestination( out Vector3 destination)
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamingRadius;
        randomDirection.y = 0f;
        Vector3 candidate = transform.position + randomDirection;

        if(NavMesh.SamplePosition(candidate, out NavMeshHit hit, roamingRadius, NavMesh.AllAreas))
        {
            destination = hit.position;

            return true;
        }

        destination = Vector3.zero;
        return false;
    }

    private void HandleDestination()
    {
        if(_movement.HasReachedDestination)
        {
            SetState(NPCState.Idle);
        }
    }

    private void SetState(NPCState newState)
    {
        if(CurrentState == newState)
        {
            return;
        }
        NPCState previousState = CurrentState;
        CurrentState = newState;

        Debug.Log(
            $"[NPCBrain] {name}: " + $"[previousState] -> {CurrentState}"
        );
    } 
}