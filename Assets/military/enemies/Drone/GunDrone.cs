using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunDrone : MonoBehaviour {

    public float BobDistance = .2f;
    public float SensorDistance = 10f;
    public Transform[] blades = new Transform[4];
    public Transform body;
    public Transform turret;
    public Transform player;
    public Light StatusLight;

    private bool alive = true;

	// Use this for initialization
	void Start () {

        StatusLight.color = Color.blue;
    }

    private float threatenTimer = 0f; 
    private float bobAmount;
	// Update is called once per frame
	void Update () {
		if (alive)
        {
            foreach(Transform blade in blades)
            {
                blade.Rotate(Vector3.forward, 20f);
            }

            body.Translate(Vector3.up * BobDistance * Mathf.Sin(Mathf.Lerp(0, Mathf.PI * 2, bobAmount)), Space.Self);
            bobAmount += Time.deltaTime;
            if (bobAmount > 1)
                bobAmount = 0;

            float distanceFromPlayer = Vector3.Distance(body.position, player.position);
            if (distanceFromPlayer < SensorDistance)
            {
                turret.LookAt(player.position);
                if (threatenTimer > 1f)
                {
                    StatusLight.color = Color.red;
                }
                else
                {
                    threatenTimer += Time.deltaTime;
                    StatusLight.color = Color.yellow;
                }
            }
            else
            {
                if (threatenTimer > 0f)
                {
                    StatusLight.color = Color.yellow;
                    threatenTimer -= Time.deltaTime;
                    if (threatenTimer <= 0f)
                    {
                        StatusLight.color = Color.blue;
                    }
                }
                turret.localRotation = Quaternion.Slerp(turret.localRotation, Quaternion.identity, Time.deltaTime);
            }
        }
	}
}
