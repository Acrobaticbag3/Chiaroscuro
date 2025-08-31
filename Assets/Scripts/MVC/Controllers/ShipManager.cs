using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    private static List<ShipModel> allShips = new List<ShipModel>();

    public static List <ShipModel> AllShips => allShips;

    public static void RegisterShip(ShipModel ship)
    {
        if (!AllShips.Contains(ship)) allShips.Add(ship);
    }

    public static void UnregisterShip(ShipModel ship)
    {
        if (AllShips.Contains(ship)) allShips.Remove(ship);
    }
}
