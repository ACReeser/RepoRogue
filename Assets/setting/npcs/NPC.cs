using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : ContextualTarget {
    public GameCanvas canvas;

	// Use this for initialization
	public override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
    }

    public override void DoInteract()
    {
        canvas.DisplayConversation(new Profile()
        {
            Age = 26,
            NetWorth = 100,
            Name = "Ana Sokolov"
        },
        new string[]{
            "> Some jerk keeps trying to get me to feel his chest.",
            "> He says he just got an eHeart installed, but I know he doesn't pull down enough creds to afford one.",
            "> I hope Baba Yaga cuts it out of his чертовский torso."
        });
    }
}
