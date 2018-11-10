using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour {
    public DamageAnnotator Annotator;
    public Transform bulletPrefab;
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
            bullet.Translate(Vector3.forward*.8f, Space.Self);
        }
    }

    internal void Shoot(Vector3 startPos, Quaternion startRot)
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
            newBullet = GameObject.Instantiate(bulletPrefab);
            bulletIndex = PoolParent.childCount;
            newBullet.GetComponent<Bullet>().OnBulletCollide += this.OnBulletCollide;
            //UnityEngine.Debug.Log("creating new bullet number " + bulletIndex);
        }
        BulletLifetimes[bulletIndex] = 5f;
        newBullet.position = startPos;
        newBullet.rotation = startRot;
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
        int damage = UnityEngine.Random.Range(1, 10);
        target.TakeDamage(damage);
        Annotator.ShowText(target.transform.position, (-damage).ToString());
    }
}
