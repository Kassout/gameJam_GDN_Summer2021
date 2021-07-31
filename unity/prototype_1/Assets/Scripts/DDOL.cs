using UnityEngine;

/**
 * <c>DDOL</c> class is used to avoid attached gameobject to be destroyed on scene loads. Practical mechanism for general game behaviour.
 */
public class DDOL : MonoBehaviour
{
    /**
     * This method is called when the script instance is being loaded.
     */
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
