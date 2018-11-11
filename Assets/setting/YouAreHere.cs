using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouAreHere : MonoBehaviour {
    public RectTransform YouAreHereSprite;
    public Transform Player;

    private Vector2 newPosition = Vector2.zero;

	// Use this for initialization
	void Start () {
        //StartCoroutine(Follow());
	}

    private float time;
    void Update()
    {
        //YouAreHereSprite.anchoredPosition = Vector2.Lerp(YouAreHereSprite.anchoredPosition, newPosition, Time.deltaTime);
        //time += Time.deltaTime;
        YouAreHereSprite.anchoredPosition  = new Vector2(Player.position.x / 69.8f * 100, Player.position.z / 69.8f * 100);
        YouAreHereSprite.SetAsLastSibling();
    }

    private IEnumerator Follow()
    {
        while (isActiveAndEnabled)
        {
            newPosition = new Vector2(Player.position.x / 69.8f * 100, Player.position.z / 69.8f * 100);
            time = 0;
            YouAreHereSprite.SetAsLastSibling();
            yield return new WaitForSeconds(.5f);
        }
    }
    
}
