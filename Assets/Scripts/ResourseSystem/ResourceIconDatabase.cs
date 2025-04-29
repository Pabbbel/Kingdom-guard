using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "ResourceIconDatabase", menuName = "Game/Resource icons data")]
public class ResourceIconDatabase : ScriptableObject
{
    [System.Serializable]
    public class ResourceIconEntry
    {
        public ResourceType Type; // Связь с enum
        public Sprite Icon; // Спрайт для этого типа
    }

    public List<ResourceIconEntry> IconMappings;

    public Sprite GetResourseIcon(ResourceType type)
    {
        var mapping = IconMappings.FirstOrDefault(x => x.Type == type);
        return mapping?.Icon;
    }
}
