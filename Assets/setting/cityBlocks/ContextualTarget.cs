using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContextTarget
{
    Intersection linkedIntersection { get; set; }
    float contextRadius { get; set; }
    int contextTextIndex { get; set; }
    Transform Player { get; }
    ContextPool Pool { get; }
    Vector3 TextOffset { get; }
    void DoInteract();
}

public class ContextualTarget : MonoBehaviour, IContextTarget {
    public Intersection linkedIntersection { get; set; }
    public float contextRadius { get; set; }
    public int contextTextIndex { get; set; }
    public Transform player;
    public Transform Player { get { return player; } }
    public Vector3 textOffset;
    public Vector3 TextOffset { get { return textOffset; } }
    public ContextPool pool;
    public ContextPool Pool { get { return pool; } }

    // Use this for initialization
    public virtual void Start () {
        if (contextRadius <= 0f)
        {
            contextRadius = 5f;
        }
	    if (linkedIntersection == null)
        {
            this.linkedIntersection = this.transform.root.GetComponent<Intersection>();
        }
        if (linkedIntersection != null)
        {
            linkedIntersection.OnPlayerEnterIntersection += LinkedIntersection_OnPlayerEnterIntersection;
            linkedIntersection.OnPlayerExitIntersection += LinkedIntersection_OnPlayerExitIntersection;
        }
	}

    private void LinkedIntersection_OnPlayerExitIntersection()
    {
        this.enabled = false;
    }

    private void LinkedIntersection_OnPlayerEnterIntersection()
    {
        this.enabled = true;
    }

    // Update is called once per frame
    public virtual void Update () {
        if (Player != null)
        {
            float distanceFromPlayer = Vector3.Distance(this.transform.position, Player.position);
            if (distanceFromPlayer < contextRadius)
            {
                if (!wasPlayerWithinRange)
                {
                    OnPlayerWalkIntoRange(distanceFromPlayer);
                }
            } else if (wasPlayerWithinRange)
            {
                OnPlayerWalkOutOfRange();
            }
        }		
	}

    private bool wasPlayerWithinRange = false;
    private void OnPlayerWalkIntoRange(float range)
    {
        wasPlayerWithinRange = true;
        this.contextTextIndex = Pool.ShowContext(transform.position+TextOffset);
    }
    private void OnPlayerWalkOutOfRange()
    {
        wasPlayerWithinRange = false;
        Pool.HideContext(this.contextTextIndex);
    }

    public void DoInteract()
    {

    }
}
