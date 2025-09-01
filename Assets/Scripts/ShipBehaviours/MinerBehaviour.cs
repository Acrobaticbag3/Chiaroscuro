using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBehaviour : MonoBehaviour, IShipBehaviour
{
    public void ExecuteOrder(ShipModel shipModel, Vector2 target)
    {
        // (Pretend mining logic)
        shipModel.TargetPosition = target;
    }
}
