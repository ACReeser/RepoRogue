using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnPlayerEnterIntersection();
public delegate void OnPlayerExitIntersection();

public class Intersection : MonoBehaviour {

    public event OnPlayerEnterIntersection OnPlayerEnterIntersection;
    public event OnPlayerExitIntersection OnPlayerExitIntersection;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
