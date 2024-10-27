using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AirDrop : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 3f;   // The movement speed is 10m/s
    public float fireRate = 0.3f;  // Seconds/shot (Unused)
    public int score = 2;   // Points earned for destroying this

    [Header("Dynamic")]
    public TextMeshPro uitValue; // Value of its crate/Damage needed to destroy this.
    //public ParticleSystem onDeathParticles; // GameObject containing a ParticleSystem to play on successful crate destruction
    //public AudioSource onSuccessAudio; 
    public int crateValue;

    private BoundsCheck bndCheck;

    void Awake()
    {                                                            // c
        bndCheck = GetComponent<BoundsCheck>();

        // Generates a random crate value between 4 and 7 (inclusive)
        crateValue = Random.Range(4, 8); // 8 is exclusive

        // Dynamically get the child gameobject called NumberText, then get its TextMeshPro Component [Needed because its a prefab?].
        uitValue = transform.Find("NumberText").gameObject.GetComponent<TextMeshPro>();

        // Dynamically get the child gameobject called onDeathParticles, then get its ParticleSystem Component [Needed because its a prefab?].
        // Dynamically get the AudioSource Component.
        //onSuccessAudio = GetComponent<AudioSource>();
    }

    // This is a Property: A method that acts like a field
    public Vector3 pos
    {                                                       // a
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        Move();

        // Check whether this Crate has gone off the bottom of the screen
        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown))
        {             // a
            Destroy(gameObject);

            Main.S.ON_DESTROY(-score); // Deduct points

            if (Main.S.remainingCrates > 0)
            {
                Main.S.remainingCrates--;
            }

            // Acknowledge crate destruction
            Main.S.numDestroyedCrates++;
        }

        // uit UPDATE
        uitValue.text = crateValue.ToString();
    }

    public virtual void Move()
    { // c
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnTriggerEnter(Collider coll)
    {
        GameObject otherGO = coll.gameObject;                                  // a
        Projectile script = otherGO.GetComponent<Projectile>();
        if (script != null) // Check if other is a projectile (has script attached)
        {
            crateValue -= script.value;
            Destroy(otherGO);      // Destroy the Projectile GameObject

            if (crateValue <= 0)
            {
                // Instantiate a particlesystem at the crates position
                GameObject effect = Instantiate(Main.S.onDeathParticles, transform.transform.Find("Crate").gameObject.transform.position, Quaternion.identity);
                ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                ps.Play(); // Play the ParticleSystem
                Destroy(effect, 2f); // Destroy Particle System in 2 secs

                if (crateValue == 0)
                {
                    Main.S.ON_DESTROY(score); // Grant points
                    Main.S.numCorrectlyDestroyedCrates++; // Acknowledge correctly destroyed crate

                    //onSuccessAudio.PlayOneShot(Main.S.onSuccessClip); // Play success audio clip
                }
                else
                {
                    Main.S.ON_DESTROY(-score); // Deduct points

                    // Play fail audio clip
                }


                Destroy(gameObject);   // Destroy this Crate GameObject 

                if (Main.S.remainingCrates > 0)
                {
                    Main.S.remainingCrates--;
                }

                // Acknowledge crate destruction
                Main.S.numDestroyedCrates++;
            }
        }
        else
        {
            Debug.Log("Enemy hit by non-Projectile: " + otherGO.name);  // c
        }
    }
}