using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipView : MonoBehaviour
{
    private ShipModel shipModel;
    [SerializeField] private SpriteRenderer highLiteSprite;

    private void Awake()
    {
        shipModel = GetComponent<ShipModel>();
        highLiteSprite?.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        highLiteSprite?.gameObject.SetActive(shipModel.IsSelected);
    }
}
