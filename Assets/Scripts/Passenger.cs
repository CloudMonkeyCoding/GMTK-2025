using UnityEngine;

public class Passenger : PathFollower
{
    private BagRequester bagRequester;
    private bool waitingForBag = false;

    protected override void Start()
    {
        base.Start();
        bagRequester = GetComponent<BagRequester>();
        if (bagRequester != null)
        {
            bagRequester.BagDelivered += HandleBagDelivered;
        }
        else
        {
            Debug.LogWarning($"{name} has no BagRequester component");
        }
    }

    protected override void Update()
    {
        if (waitingForBag) return;
        base.Update();
    }

    private void HandleBagDelivered()
    {
        waitingForBag = false;
        currentWaypointIndex = 0; // head back out
    }

    private void OnDestroy()
    {
        if (bagRequester != null)
        {
            bagRequester.BagDelivered -= HandleBagDelivered;
        }
    }

    protected override bool OnWaypointReached(int waypointIndex)
    {
        if (waypointIndex == 1)
        {
            waitingForBag = true;
            return false; // stop until bag delivered
        }
        else if (waypointIndex == 0)
        {
            if (bagRequester != null)
            {
                bagRequester.RequestRandomBag();
            }
        }
        return true;
    }
}
