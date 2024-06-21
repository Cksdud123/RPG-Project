using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int money;
    public float health;
    public float[] position;

    public PlayerData(PlayerStatus playerStatus)
    {
        level = playerStatus.experienceManager.currentLevel;
        money = playerStatus.PlayerMoney;
        health = playerStatus.healthBar.healthSlider.value;

        position = new float[3];
        position[0] = playerStatus.transform.position.x;
        position[1] = playerStatus.transform.position.y;
        position[2] = playerStatus.transform.position.z;
    }
}
