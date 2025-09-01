using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortBehaviour : MonoBehaviour, IShipBehaviour 
{
    [SerializeField] private float escortRadius = 2.5f;

    public void ExecuteOrder(ShipModel shipModel, Vector2 target)
    {
        // (Pretend escort logic)
        Vector2 offset = Random.insideUnitCircle.normalized * escortRadius;
        shipModel.TargetPosition = target + offset;
    }
}
