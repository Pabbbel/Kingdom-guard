using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Resources;

public class BuildingDescriptionDisplay : MonoBehaviour
{
    [SerializeField] private ResourceIconDatabase _iconDatabase;
    [SerializeField] private Animator _animator;
    [SerializeField] private Button _buyButton;

    [SerializeField] private TextMeshProUGUI _nameText;

    [SerializeField] private GameObject _requirementsPanel;

    [SerializeField] private GameObject _prodountDisplay;
    [SerializeField] private GameObject _recDisplay;
    [SerializeField] private GameObject _costDisplay;

    [SerializeField] private GameObject _resourseCounterPrefab;

    public void ShowDescriptionPanel()
    {
        _animator.SetTrigger("description");
    }

    public void HideDescriptionPanel()
    {
        _animator.SetTrigger("out");
    }

    public void UpdateCostDisplay(BuildingData buildingData)
    {
        if (_costDisplay.transform.childCount > 0)
        {
            foreach (UnityEngine.Transform child in _costDisplay.transform)
            {
                Destroy(child.gameObject);
            }
        }

        GameObject resourseCounterPrefab = Instantiate(_resourseCounterPrefab, _costDisplay.transform.position, Quaternion.identity);
        resourseCounterPrefab.transform.SetParent(_costDisplay.transform, false);

        Image _costIcon = resourseCounterPrefab.transform.GetChild(1).GetComponentInChildren<Image>();
        TextMeshProUGUI _costCountText = resourseCounterPrefab.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();

        if (buildingData != null && buildingData.BuildCostDictionary.Keys.Any())
        {
            ResourceType firstResourceType = buildingData.BuildCostDictionary.Keys.First();

            if(buildingData.Type == BuildingType.Market)
                _nameText.text = $"{buildingData.BuildingName}";
            else
                _nameText.text = $"{buildingData.BuildingName} производит";

            _costIcon.sprite = _iconDatabase.GetResourseIcon(firstResourceType);
            int resourceAmount = buildingData.BuildCostDictionary[firstResourceType];
            _costCountText.text = resourceAmount.ToString();
        }
        else
        {
            Color redColor = new Color(1, 0, 0, 1);
            _costCountText.color = redColor;
            Debug.LogWarning("Нет ресурсов для постройки.");
        }
    }

    public void UpdateProductionDisplay(BuildingData buildingData)
    {
        if (_prodountDisplay.transform.childCount > 0)
        {
            foreach (UnityEngine.Transform child in _prodountDisplay.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (buildingData.Type == BuildingType.Market)
            _prodountDisplay.SetActive(false);
        else
            _prodountDisplay.SetActive(true);

        GameObject resourseCounterPrefab = Instantiate(_resourseCounterPrefab, _prodountDisplay.transform.position, Quaternion.identity);
        resourseCounterPrefab.transform.SetParent(_prodountDisplay.transform, false);

        Image _prodIcon = resourseCounterPrefab.transform.GetChild(1).GetComponentInChildren<Image>();
        TextMeshProUGUI _prodtCountText = resourseCounterPrefab.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();

        if (buildingData != null && buildingData.ProductionDictionary.Keys.Any())
        {
            ResourceType firstResourceType = buildingData.ProductionDictionary.Keys.First();
            _prodIcon.sprite = _iconDatabase.GetResourseIcon(firstResourceType);
            int resourceAmount = buildingData.ProductionDictionary[firstResourceType];
            _prodtCountText.text = resourceAmount.ToString();
        }
        else
        {
            Debug.LogWarning("Нет ресурсов для производства.");
        }
    }

    public void UpdateRequiredResourceDisplay(BuildingData buildingData)
    {
        if (_recDisplay.transform.childCount > 0)
        {
            foreach (UnityEngine.Transform child in _recDisplay.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (buildingData != null && buildingData.ProductionRequirementsDictionary.Keys.Any())
        {
            _requirementsPanel.gameObject.SetActive(true);

            foreach (ResourceType resourceType in buildingData.ProductionRequirementsDictionary.Keys)
            {
                GameObject resourseCounterPrefab = Instantiate(_resourseCounterPrefab, _recDisplay.transform.position, Quaternion.identity);
                resourseCounterPrefab.transform.SetParent(_recDisplay.transform, false);
                Image _prodIcon = resourseCounterPrefab.transform.GetChild(1).GetComponentInChildren<Image>();
                TextMeshProUGUI _prodtCountText = resourseCounterPrefab.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();

                _prodIcon.sprite = _iconDatabase.GetResourseIcon(resourceType);
                int resourceAmount = buildingData.ProductionRequirementsDictionary[resourceType];
                _prodtCountText.text = resourceAmount.ToString();
            }

        }
        else
        {
            _requirementsPanel.gameObject.SetActive(false);
            if (buildingData.Type == BuildingType.College || buildingData.Type == BuildingType.Workshop || buildingData.Type == BuildingType.Cafe)
                Debug.LogWarning("Не указаны требуемые ресурсы для производства.");
        }
    }
}
