using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AirDrop1 : AirDrop
{
    private float x0;
    private float birthTime;
    private float waveFreq = 2;
    public float waveWidth = 4;

    // Start is called before the first frame update
    void Start()
    {
        x0 = pos.x;
        birthTime = Time.time;
    }

    public override void Move()
    {
        Vector3 tempPos = pos;
        float age = Time.time - birthTime; // How much time since the start of game?
        float theta = Mathf.PI * 2 * age / waveFreq;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        base.Move();
    }
}
