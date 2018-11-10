using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnBulletCollide(Transform bullet, ITarget hit);

public class Bullet : MonoBehaviour {
    public event OnBulletCollide OnBulletCollide;

    private void onHitTransform(Transform t)
    {
        var target = t.root.GetComponent<ITarget>();
        if (target != null)
        {
            if (this.OnBulletCollide != null)
            {
                this.OnBulletCollide(this.transform, target);
            }
        }
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        this.onHitTransform(other.transform);
    }
    private void OnCollisionEnter(Collision col)
    {
        this.onHitTransform(col.transform);
    }
}
