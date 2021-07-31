using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <c>DontDestroyOnLoad</c> class is used to avoid attached gameobject to be destroyed on scene loads. Practical mechanism for general game behaviour.
 */
public class DontDestroyOnLoad : MonoBehaviour
{
    /**
     * This method is called when the script instance is being loaded.
     */
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}