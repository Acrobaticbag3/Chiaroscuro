using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private FleetRingView ringPrefab;

    [Header("Settings")]
    [SerializeField] private float fleetZoomThreshold = 10f;
    [SerializeField] private float clusterDistance = 6f;
    [SerializeField] private float rebuildInterval = 0.25f;

    public IReadOnlyList<FleetGroup> ActiveFleets => activeFleets;

    private readonly List<FleetGroup> activeFleets = new();
    private float timer;

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= rebuildInterval)
        {
            timer = 0f;

            bool fleetMode = mainCamera != null && mainCamera.orthographicSize >= fleetZoomThreshold;
            if (fleetMode) RebuildFleets();
            else ClearFleets();
        }
    }

    private void RebuildFleets()
    {
        // Clear old visuals
        foreach (var fleet in activeFleets)
            if (fleet.ringView != null) Destroy(fleet.ringView.gameObject);
        activeFleets.Clear();

        var ships = ShipManager.AllShips;
        var unVisited = new HashSet<ShipModel>(ships);

        int fleetId = 0;
        while (unVisited.Count > 0)
        {
            // Pop one seed
            var enumerator = unVisited.GetEnumerator();
            enumerator.MoveNext();
            ShipModel seed = enumerator.Current;
            unVisited.Remove(seed);

            // BFS-like cluster
            var group = new FleetGroup { FleedId = fleetId++ };
            var queue = new Queue<ShipModel>();
            queue.Enqueue(seed);

            while (queue.Count > 0)
            {
                var ship = queue.Dequeue();
                group.Members.Add(ship);

                // find neighbours
                var toCheck = new List<ShipModel>();
                foreach (var other in unVisited)
                {
                    if ((other.Position - ship.Position).sqrMagnitude <= clusterDistance * clusterDistance)
                        toCheck.Add(other);
                }

                // add/remove from unvisited
                foreach (var n in toCheck)
                {
                    unVisited.Remove(n);
                    queue.Enqueue(n);
                }
            }

            // Skip singelton groups????
            // Rink view
            var ring = Instantiate(ringPrefab);
            group.ringView = ring;
            group.UpdateGeometry();

            activeFleets.Add(group);
        }
    }

    private void ClearFleets()
    {
        foreach (var fleet in activeFleets)
            if (fleet.ringView != null) Destroy(fleet.ringView.gameObject);

        activeFleets.Clear();
    }

    // Select fleet by clicking ring
    public FleetGroup TryPickFleet(Vector2 worldPoint)
    {
        foreach (var fleet in activeFleets)
        {
            if (fleet.ringView != null && fleet.ringView.IsPointInside(worldPoint, fleet.Center, fleet.Radius))
                return fleet;
        }
        return null;
    }
}
