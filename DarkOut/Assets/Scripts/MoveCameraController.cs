using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(Input.GetAxis("ArrowHorizontal") * 3.0f, Input.GetAxis("ArrowVertical") * 3.0f, 0);
    }
}
