using UnityEngine;

public class ConveyorBelt : PathFollower
{
    [SerializeField] private int startWaypointIndex = 0;

    protected override void Start()
    {
        base.Start();
        currentWaypointIndex = startWaypointIndex;
    }
}
