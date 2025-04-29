using System;
using UnityEngine;
using UnityEngine.UI;

public class TradeButton : MonoBehaviour
{
    [SerializeField] private Sprite _trade;
    [SerializeField] private Sprite _noTrade;
    [SerializeField] private Image _buttonImage;

    // Добавим ссылку на конкретный StoreItem
    [SerializeField] private StoreItem _associatedStoreItem;

    public static event Action<StoreItem> ActivateTrading;
    public static event Action<StoreItem> DeactivateTrading;

    public bool TradeResource { get; private set; } = false;

    public void SellButtonClick()
    {
        TradeResource = !TradeResource;
        _buttonImage.sprite = TradeResource ? _trade : _noTrade;

        if (TradeResource)
            ActivateTrading?.Invoke(_associatedStoreItem);
        else
            DeactivateTrading?.Invoke(_associatedStoreItem);
    }
}
