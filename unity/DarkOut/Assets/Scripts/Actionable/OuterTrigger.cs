using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterTrigger : MonoBehaviour
{
    public RotationPlatformController controller;

    void OnTriggerExit2D(Collider2D other) {
        controller.OuterExited(other);
    }
}
