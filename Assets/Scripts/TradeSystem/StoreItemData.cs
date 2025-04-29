using UnityEngine;

[CreateAssetMenu(fileName = "StoreItemData", menuName = "Game/Market")]
public class StoreItemData : ScriptableObject
{
    public ResourceType TradeResourse;
    public int AmountTradeResource;

    public ResourceType CostResourse;
    public int AmountCostResource;
}
