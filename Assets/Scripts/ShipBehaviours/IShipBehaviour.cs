using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShipBehaviour
{
    public void ExecuteOrder(ShipModel ship, UnityEngine.Vector2 target);
}
