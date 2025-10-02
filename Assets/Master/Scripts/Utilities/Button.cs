using UnityEngine;

public class Button : MonoBehaviour
{
    private UnityEngine.UI.Button button;

    private void Awake()
    {
        button = GetComponent<UnityEngine.UI.Button>();
    }

    private void Start()
    {
        EventCallback.OnGameStart += GameStart;
    }

    private void GameStart(Transform t)
    {
        button.onClick.AddListener(() =>
        {
            t.GetComponent<Health>().CantDie();
        });
    }
}