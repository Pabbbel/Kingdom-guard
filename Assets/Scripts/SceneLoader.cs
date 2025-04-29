using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private int _sceneIndex;

    public void LoadScene()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(_sceneIndex);
    }
}