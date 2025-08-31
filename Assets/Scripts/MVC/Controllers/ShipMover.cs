using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float arriveDistance = 0.05f;

    private ShipModel shipModel;

    void Awake() => shipModel = GetComponent<ShipModel>();

    void Update()
    {
        Vector2 pos = model.Position;
        Vector2 target = model.TargetPosition;

        if ((pos - target).sqrMagnitude > arriveDistance * arriveDistance)
        {
            Vector2 dir = (target - pos).normalized;
            pos += dir * moveSpeed * Time.deltaTime;
            model.Position = pos;
        }
    }
}
