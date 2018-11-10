using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal delegate T StateMachineAction<T>();
internal class StateMachine<T>: IEnumerable<KeyValuePair<T, StateMachineAction<T>>>
{
    private Dictionary<T, StateMachineAction<T>> _actions = new Dictionary<T, StateMachineAction<T>>();
    public T State { get; set; }
    public StateMachineAction<T> this[T state]
    {
        get { return _actions[state]; }
        set { _actions[state] = value; }
    }
    public void Update()
    {
        State = _actions[State]();
    }

    public IEnumerator<KeyValuePair<T, StateMachineAction<T>>> GetEnumerator(){ return _actions.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() /*******************************/ { return _actions.GetEnumerator(); }
    public void Add(T state, StateMachineAction<T> action) /****************/ { _actions.Add(state, action); }
}

internal enum DroneState
{
    Patrol,
    Targeting,
    Firing,
    Reloading,
    Dead
}

public class GunDrone : MonoBehaviour, ITarget {
    private const float SensorLockOnTimeSeconds = 2f;
    private const float GunReloadTimeSeconds = 2f;
    private const int GunBurstCount = 3;
    public float BobDistance = .2f;
    public float SensorDistance = 10f;
    public Transform[] blades = new Transform[4];
    public Transform body;
    public Transform turret;
    public Transform player;
    public Transform shadow;
    public Collider DeathCollider;
    public Transform ExplosionPrefab;
    public Light StatusLight;
    public BulletShooter shooter;

    private float bulletReloadTime = 0f;
    private int bulletBurstCount = 0;

    private StateMachine<DroneState> state;

	// Use this for initialization
	void Start () {
        state = new StateMachine<DroneState>() {
        {
            DroneState.Dead, () => { return DroneState.Dead; }
        },
        {
            DroneState.Patrol, () =>
            {
                StatusLight.color = Color.blue;
                float distanceFromPlayer = Vector3.Distance(body.position, player.position);

                if (distanceFromPlayer < SensorDistance){
                    turret.localRotation = Quaternion.Slerp(turret.localRotation, Quaternion.identity, Time.deltaTime);
                    return DroneState.Targeting;
                } else {
                    return DroneState.Patrol;
                }
            }
        },
        {
            DroneState.Targeting, () =>
            {
                StatusLight.color = Color.yellow;
                float distanceFromPlayer = Vector3.Distance(body.position, player.position);
                turret.LookAt(player.position + Vector3.up);
                if (distanceFromPlayer > SensorDistance){
                    if (threatenTimer < -1f) {
                        threatenTimer = 0;
                        return DroneState.Patrol;
                    } else {
                        threatenTimer -= Time.deltaTime;
                        return DroneState.Targeting;
                    }
                } else if (threatenTimer > SensorLockOnTimeSeconds)
                {
                    return DroneState.Firing;
                }
                else
                {
                    threatenTimer += Time.deltaTime;
                    return DroneState.Targeting;
                }
            }
        },
        {
            DroneState.Firing, () =>
            {
                StatusLight.color = Color.red;
                turret.LookAt(player.position + Vector3.up);
                if (bulletBurstCount >= GunBurstCount)
                {
                    bulletBurstCount = 0;
                    bulletReloadTime = GunReloadTimeSeconds;
                    return DroneState.Reloading;
                }
                else if (bulletReloadTime <= 0f)
                {
                    shooter.Shoot(turret.position + turret.TransformDirection(Vector3.forward*2f), turret.rotation);
                    bulletBurstCount++;
                    bulletReloadTime += .25f;
                    return DroneState.Firing;
                }
                else
                {
                    bulletReloadTime -= Time.deltaTime;
                    return DroneState.Firing;
                }
            }
        },
        {
            DroneState.Reloading, () =>
            {
                //turret.LookAt(player.position);
                if (bulletReloadTime <= 0f) {
                    return DroneState.Firing;
                } else {
                    float distanceFromPlayer = Vector3.Distance(body.position, player.position);

                    if (distanceFromPlayer > SensorDistance){
                        threatenTimer = 0f;
                        return DroneState.Targeting;
                    } else {
                        bulletReloadTime -= Time.deltaTime;
                        return DroneState.Reloading;
                    }
                }
            }
        }
        };
    }

    private float threatenTimer = 0f; 
    private float bobAmount;
	// Update is called once per frame
	void Update () {
		if (state.State != DroneState.Dead)
        {
            foreach(Transform blade in blades)
            {
                blade.Rotate(Vector3.forward, 20f);
            }

            body.Translate(Vector3.up * BobDistance * Mathf.Sin(Mathf.Lerp(0, Mathf.PI * 2, bobAmount)), Space.Self);
            bobAmount += Time.deltaTime;
            if (bobAmount > 1)
                bobAmount = 0;

            state.Update();
        }
	}

    private int health = 25;
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            state.State = DroneState.Dead;
            shadow.gameObject.SetActive(false);
            this.GetComponent<Collider>().enabled = false;
            var newBody = DeathCollider.transform.parent.gameObject.AddComponent<Rigidbody>();
            newBody.mass = 100;
            var ex = GameObject.Instantiate(ExplosionPrefab, DeathCollider.transform.position, Quaternion.identity);
            ex.localScale = Vector3.one * .25f;
        }
    }
}
