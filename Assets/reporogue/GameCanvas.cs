using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile
{
    public string Name { get; set; }
    public int NetWorth { get; set; }
    public int Age { get; set; }
    public string Implants { get; set; }
}

[Serializable]
public class ProfileUI
{
    public RectTransform ProfilePanel ;
    public Text VitalText ;
    public Text NameText ;
    public Text ConversationText ;
}

[Serializable]
public class BarDisplay
{
    public Text Label;
    public Image Bar;
}
public class GameCanvas : MonoBehaviour {

    public BarDisplay HealthBar;
    public ProfileUI ProfileUI;

	// Use this for initialization
	void Start ()
    {
        ProfileUI.ProfilePanel.gameObject.SetActive(false);
        //StartCoroutine(DisplayConversation(new Profile(), new string[]
        //{
        //    "> I have a job for you, if you have the stomach.",
        //    "> A narcpusher just bragcasted about his shiny new eHeart. I put a trackerapp in his tech last week hoping he'd do something dumb. I'm streaming you his realtime location now.",
        //    "> Track him down and take his heart and I'll pay handsomely."
        //}));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHealthBar(int health, int maxHealth = 100)
    {
        HealthBar.Label.text = health.ToString() + "/"+maxHealth.ToString();
        HealthBar.Bar.fillAmount = (health+0f) / (maxHealth+0f);
    }

    public void DisplayConversation(Profile from, string[] segment)
    {
        StartCoroutine(_DisplayConversation(from, segment));
    }

    private IEnumerator _DisplayConversation(Profile from, string[] segment)
    {
        ProfileUI.ProfilePanel.gameObject.SetActive(true);
        ProfileUI.ConversationText.text = "";
        foreach (string seg in segment)
        {
            yield return TypeText(ProfileUI.ConversationText, seg);
        }
    }

    private IEnumerator TypeText(Text source, string segment)
    {
        foreach(char c in segment)
        {
            source.text += c;
            yield return new WaitForSeconds(0.01f);
        }
        source.text += '\n';
    }

    public void ConversationSelect(int index)
    {
        if (index < 0)
            this.ConversationExit();
    }
    public void ConversationExit()
    {
        ProfileUI.ProfilePanel.gameObject.SetActive(false);
    }
}
