using UnityEngine;
using UnityEngine.UI;

public class PanelLastSibling : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private Button _panelButton;

    private void Start()
    {
        _panelButton = GetComponent<Button>();
        _panelButton.onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        _panel.transform.SetAsLastSibling();
    }
}
