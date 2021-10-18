using UnityEngine;

public class EndPointController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LoadNextLevel();
        }
    }
}
