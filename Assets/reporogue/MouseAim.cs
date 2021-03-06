﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[Serializable]
public class ZoomInfo
{
    public float ClosestZoomSize = 2f;
    public float FurthestZoomSize = 10f;
    public float CurrentZoomSize = 8f;
    public float ZoomSpeed = 75f;
}

public class MouseAim : MonoBehaviour {
    public float floorHeight;
    public Transform mouseVisualization, muzzle;
    public Animator animator;
    public ThirdPersonCharacter character;
    public BulletShooter shooter;
    public ZoomInfo Zoom;

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // create a plane at 0,0,0 whose normal points to +Y:
        Plane hPlane = new Plane(Vector3.up, Vector3.zero*floorHeight);
        // Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
        float distance = 0;
        // if the ray hits the plane...
        if (hPlane.Raycast(ray, out distance))
        {
            // get the hit point:
            mouseVisualization.transform.position = ray.GetPoint(distance)-ray.direction.normalized;
        }
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Aim", true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("Aim", false);
        }
        if (Input.GetMouseButtonUp(0) && Input.GetMouseButton(1))
        {
            shooter.Shoot(muzzle.position, muzzle.rotation);
            animator.SetTrigger("SniperFire");
        }

        Zoom.CurrentZoomSize -= Time.deltaTime * Zoom.ZoomSpeed * Input.GetAxis("Mouse ScrollWheel");
        Zoom.CurrentZoomSize = Mathf.Clamp(Zoom.CurrentZoomSize, Zoom.ClosestZoomSize, Zoom.FurthestZoomSize);
        Camera.main.orthographicSize = Zoom.CurrentZoomSize;
    }
	
	// Update is called once per frame
	void OnAnimatorIK() {
        //Vector3 v = mouseVisualization.transform.position - this.transform.position;

        //animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.LookRotation(v, Vector3.up));

        //Quaternion q = animator.GetBoneTransform(HumanBodyBones.Spine).rotation;
        //Quaternion q2 = animator.GetBoneTransform(HumanBodyBones.Hips).rotation;

        //float angle = Quaternion.Angle(q2, q);

        //Debug.Log(angle);

        //if (angle > 45)
        //{
        //    animator.SetBoneLocalRotation(HumanBodyBones.Hips, Quaternion.Slerp(q, q2, Time.deltaTime));
        //}
        if (animator.GetBool("Aim"))
        {
            animator.SetLookAtPosition(mouseVisualization.transform.position);
        
            animator.SetLookAtWeight(1, character.IsGrounded ? 1f : 0f);
        }
        //Transform spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        //Vector3 forward = (mouseVisualization.transform.position - spine.position).normalized;
        //Vector3 up = Vector3.Cross(forward, transform.right);
        //Quaternion rotation = Quaternion.Inverse(transform.rotation) * Quaternion.LookRotation(forward, up);
        //animator.SetBoneLocalRotation(HumanBodyBones.Spine, rotation);
    }
}
