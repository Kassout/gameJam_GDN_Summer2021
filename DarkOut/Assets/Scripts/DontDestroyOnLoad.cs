using UnityEngine;

/// <summary>
/// Class <c>DontDestroyOnLoad</c> is a Unity component script used to avoid attached gameobject to be destroyed on scene loads. Practical mechanism for general game behaviour.
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}