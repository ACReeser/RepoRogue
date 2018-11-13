using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour {
    private Dictionary<int, MeshRenderer> hiddenBuildings = new Dictionary<int, MeshRenderer>();
    public void OnTriggerEnter(Collider collider)
    {
        if (collider != null &&
            collider.transform.childCount > 0 &&
            collider.transform.GetChild(0).CompareTag("invisible_fence") &&
            !hiddenBuildings.ContainsKey(collider.GetInstanceID()))
        {
            var buildingRenderer = collider.GetComponent<MeshRenderer>();
            hiddenBuildings.Add(collider.GetInstanceID(), buildingRenderer);
            buildingRenderer.enabled = false;
            collider.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            //buildingRenderer.gameObject.isStatic = false;
        }            
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider != null &&
            hiddenBuildings.ContainsKey(collider.GetInstanceID()))
        {
            var buildingRenderer = collider.GetComponent<MeshRenderer>();
            hiddenBuildings.Remove(collider.GetInstanceID());
            buildingRenderer.enabled = true;
            collider.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
