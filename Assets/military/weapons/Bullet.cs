using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnBulletCollide(Transform bullet, ITarget hit);

public class Bullet : MonoBehaviour {
    public event OnBulletCollide OnBulletCollide;

    private void OnTriggerEnter(Collider other)
    {
        var target = other.transform.root.GetComponent<ITarget>();
        if (target != null)
        {
            if (this.OnBulletCollide != null)
            {
                this.OnBulletCollide(this.transform, target);
            }
        }
    }
}
