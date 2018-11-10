using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    void TakeDamage(int amount);
}

public class Player : MonoBehaviour, ITarget {
    public GameCanvas GameCanvas;

    private int health = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;
        GameCanvas.UpdateHealthBar(health);
    }
}
