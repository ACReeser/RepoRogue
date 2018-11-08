using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    public Vector3 offset;
    public float speed = 2.0f;

    void Update()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = this.transform.position;
        position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x+offset.x, interpolation);
        position.z = Mathf.Lerp(this.transform.position.z, objectToFollow.transform.position.z+offset.z, interpolation);

        this.transform.position = position;
    }
}
