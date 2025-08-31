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
        if (highLiteSprite != null) highLiteSprite.enable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (highLiteSprite != null) highLiteSprite.enable = model.IsSelected;
    }
}
