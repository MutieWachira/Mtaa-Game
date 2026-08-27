using UnityEngine;
using UnityEngine.AI;

///<summary>
/// Handles physiacll NPC navigation using Unity NavMesh.
/// </summary>
/// 
[RequireComponent(typeof(NavMeshAgent))]
public sealed class NPCMovement : MonoBehaviour
{
    [SerializeField]
    private float stoppingDistance = 1.2f;
    private NavMeshAgent _agent;

    public bool HasReachedDestination { get; private set;}

    public NPCMovementState CurrentState { get; private set; }
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = stoppingDistance;
        CurrentState = NPCMovementState.Idle;
    }

    private void Update()
    {
        UpdateMovementState();
    }
    
    ///<summary>
    ///Sends the NPC toward a world position.
    /// </summary>
    public void MoveTo(Vector3 destination)
    {
        if(!_agent.isOnNavMesh)
        {
            Debug.LogWarning(
                $"[NPCMovement] {name} " +
                "is not currently on a NavMesh."
            );
            return;
        }

        bool success = _agent.SetDestination(destination);

        if(!success)
        {
             Debug.LogWarning(
                $"[NPCMovement] {name} " +
                "failed to set destination."
            );
            return;
        }
        HasReachedDestination= false;    
    } 
     private void UpdateMovementState()
    {
        if (!_agent.isOnNavMesh ||
            _agent.pathPending)
        {
            return;
        }

        if (_agent.remainingDistance <=
            _agent.stoppingDistance)
        {
            if (!HasReachedDestination)
            {
                HasReachedDestination = true;

                CurrentState =
                    NPCMovementState.Idle;
            }

            return;
        }

        HasReachedDestination = false;

        CurrentState =
            _agent.velocity.sqrMagnitude > 0.01f
                ? NPCMovementState.Walking
                : NPCMovementState.Idle;
    }

    private void OnDrawGizmosSelected()
    {
        if (_agent == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(
            transform.position,
            _agent.stoppingDistance
        );
    }
}