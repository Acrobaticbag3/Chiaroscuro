using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float arriveDistance = 0.05f;

    private ShipModel shipModel;

    void Awake()
    {
        shipModel = GetComponent<ShipModel>();
        shipModel.TargetPosition = transform.position;
    }

    void Update()
    {
        Vector2 pos = shipModel.Position;
        Vector2 target = shipModel.TargetPosition;

        if ((pos - target).sqrMagnitude > arriveDistance * arriveDistance)
        {
            pos = Vector2.MoveTowards(pos, target, moveSpeed * Time.deltaTime);
            shipModel.Position = pos;
        }
    }
}
