using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoundsCheck))]
public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private TextMeshPro uitValue;

    [Header("Dynamic")]
    public int value; // Strength of proj against a crate, set when shooting based on input.

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();

        uitValue = transform.Find("NumberText").gameObject.GetComponent<TextMeshPro>();
    }

    void Update()
    {
        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp))
        {          // a
            Destroy(gameObject);        
        }
    }
    
    // Set the color and text
    public void printNumber()
    {
        if (value == 1) // S/'1'
            uitValue.color = Color.cyan;
        else if (value == 2) // D/'2'
            uitValue.color = Color.magenta;
        else if (value == 3) // F/'3'
            uitValue.color = Color.red;

        uitValue.text = value.ToString();
    }
}
