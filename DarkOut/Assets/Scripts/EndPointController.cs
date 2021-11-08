using UnityEngine;

/// <summary>
/// TODO: comments
/// </summary>
public class EndPointController : MonoBehaviour
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="other">TODO: comments</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LoadNextLevel();
        }
    }
}
