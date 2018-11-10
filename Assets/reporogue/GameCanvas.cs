using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarDisplay
{
    public Text Label;
    public Image Bar;
}
public class GameCanvas : MonoBehaviour {

    public BarDisplay HealthBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHealthBar(int health, int maxHealth = 100)
    {
        HealthBar.Label.text = health.ToString() + "/"+maxHealth.ToString();
        HealthBar.Bar.fillAmount = health / maxHealth;
    }
}
