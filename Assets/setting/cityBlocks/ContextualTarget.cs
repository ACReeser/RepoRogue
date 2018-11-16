using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContextTarget
{
    Intersection linkedIntersection { get; set; }
    float contextRadius { get; set; }
    Transform Player { get; set; }
    void DoInteract();
}

public class ContextualTarget : MonoBehaviour, IContextTarget {
    public Intersection linkedIntersection { get; set; }
    public float contextRadius { get; set; }
    public Transform Player { get; set; }

	// Use this for initialization
	void Start () {
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
    void Update () {
        if (Player != null)
        {
            float distanceFromPlayer = Vector3.Distance(this.transform.position, Player.position);
            if (distanceFromPlayer < contextRadius && !wasPlayerWithinRange)
            {
                OnPlayerWalkIntoRange(distanceFromPlayer);
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
    }
    private void OnPlayerWalkOutOfRange()
    {
        wasPlayerWithinRange = false;
    }

    public void DoInteract()
    {

    }
}
