using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour {

    public Transform bulletPrefab;
    public Transform muzzle;
    public Transform PoolParent;

    private const int MaxBullets = 100;
    private float[] BulletLifetimes = new float[MaxBullets];
    private Queue<int> inactiveBullets = new Queue<int>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int index = 0;
		foreach(Transform child in PoolParent)
        {
            if (child != null && child.gameObject.activeInHierarchy)
            {
                MoveBullet(child, index);
            }
            index++;
        }
	}

    private void MoveBullet(Transform bullet, int bulletIndex)
    {
        //int bulletIndex = bullet.GetSiblingIndex();
        if (BulletLifetimes[bulletIndex] < 0f)
        {
            bullet.GetChild(0).GetComponent<TrailRenderer>().Clear();
            bullet.gameObject.SetActive(false);
            //if (inactiveBullets.Contains(bulletIndex))
            //{
            //    UnityEngine.Debug.Log("Bullet number " + bulletIndex + " is already listed as 'inactive'");
            //}
            inactiveBullets.Enqueue(bulletIndex);
        }
        else
        {
            BulletLifetimes[bulletIndex] -= Time.deltaTime;
            bullet.Translate(Vector3.forward, Space.Self);
        }
    }

    internal void Shoot()
    {
        int bulletIndex;
        Transform newBullet = null;
        if (inactiveBullets.Count > 0)
        {
            bulletIndex = inactiveBullets.Dequeue();
            newBullet = PoolParent.GetChild(bulletIndex);
            //UnityEngine.Debug.Log("reusing bullet number "+bulletIndex);
        }
        else
        {
            newBullet = GameObject.Instantiate(bulletPrefab, muzzle);
            bulletIndex = PoolParent.childCount;
            newBullet.GetComponent<Bullet>().OnBulletCollide += this.OnBulletCollide;
            //UnityEngine.Debug.Log("creating new bullet number " + bulletIndex);
        }
        BulletLifetimes[bulletIndex] = 5f;
        newBullet.gameObject.SetActive(true);
        newBullet.SetParent(muzzle);
        newBullet.localPosition = Vector3.zero;
        newBullet.localRotation = Quaternion.identity;
        newBullet.SetParent(PoolParent);
        newBullet.SetSiblingIndex(bulletIndex);
        newBullet.GetChild(0).GetComponent<TrailRenderer>().Clear();
        newBullet.gameObject.SetActive(true);
        //UnityEngine.Debug.Log(newBullet.gameObject.activeInHierarchy);      
    }
    
    internal void OnBulletCollide(Transform bullet, ITarget target)
    {
        bullet.gameObject.SetActive(false);
        inactiveBullets.Enqueue(bullet.GetSiblingIndex());
        target.TakeDamage(UnityEngine.Random.Range(1, 10));
    }
}
