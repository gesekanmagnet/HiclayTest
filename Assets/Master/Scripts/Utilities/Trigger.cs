using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private int[] levels;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (levels.Length == 0) return;

        GameController.Instance.LoadLevel(levels[0], true);
        foreach (var item in levels)
        {
            GameController.Instance.LoadLevel(item);
        }
    }
}