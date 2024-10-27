using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelControl : MonoBehaviour
{
    float angle = 0;
    Vector3 launchPos = new Vector3(0, -4, 0);
    //Vector3 mouseDelta;

    [Header("Inscribed")]                                                       
    public GameObject projectilePrefab; // Projectile to instantiate
    public int projectileSpeed = 1;
    public AudioSource CannonShot;

    // Update is called once per frame
    void Update()
    {
        // If the game is paused, dont process input
        if (PauseMenu.isPaused) { return; }

        // Move the cannon 
        Vector3 mousePos2D = Input.mousePosition; // z is 0
        mousePos2D.z = -Camera.main.transform.position.z; // Find z
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 direction = mousePos3D - transform.position;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //angle += .1f;
        //print(angle);

        if (Mathf.Abs(angle - 90) < 85)
            transform.rotation = Quaternion.Euler(0, 0, angle-90);


        // Allow the barrel to shoot
        if (Input.GetKeyDown(KeyCode.S) && Main.S.remainingSBullets[Main.S.level] > 0) // S (1 point shot)
        {
            if (ChangeSound.isMusicOn)
                CannonShot.Play();

            GameObject projGO = Instantiate<GameObject>(projectilePrefab);
            projGO.GetComponent<Projectile>().value = 1; // Assign a strength of 1 to this projectile
            projGO.GetComponent<Projectile>().printNumber(); // Show the number of the projectile on the object
            //projGO.transform.position = transform.position; if you use position instead
            projGO.transform.position = launchPos;
            Rigidbody rigidB = projGO.GetComponent<Rigidbody>();

            // Find the delta from the launchPos to the mousePos3D
            //mouseDelta = mousePos3D - launchPos;
            rigidB.isKinematic = false;
            rigidB.velocity = direction * projectileSpeed;

            // Shot fired - Decrement '1 point (S)'.
            Main.S.SHOT_FIRED(projGO.GetComponent<Projectile>().value);
        }

        else if (Input.GetKeyDown(KeyCode.D) && Main.S.remainingDBullets[Main.S.level] > 0) // D (2 point shot)
        {
            if (ChangeSound.isMusicOn)
                CannonShot.Play();

            GameObject projGO = Instantiate<GameObject>(projectilePrefab);
            projGO.GetComponent<Projectile>().value = 2;
            projGO.GetComponent<Projectile>().printNumber(); // Show the number of the projectile on the object
            //projGO.transform.position = transform.position; if you use position instead
            projGO.transform.position = launchPos;
            Rigidbody rigidB = projGO.GetComponent<Rigidbody>();

            // Find the delta from the launchPos to the mousePos3D
            //mouseDelta = mousePos3D - launchPos;
            rigidB.isKinematic = false;
            rigidB.velocity = direction * projectileSpeed;

            // Shot fired - Decrement '2 points (D)'.
            Main.S.SHOT_FIRED(projGO.GetComponent<Projectile>().value);
        }

        else if (Input.GetKeyDown(KeyCode.F) && Main.S.remainingFBullets[Main.S.level] > 0) // F (3 point shot)
        {
            if (ChangeSound.isMusicOn)
                CannonShot.Play(); 

            GameObject projGO = Instantiate<GameObject>(projectilePrefab);
            projGO.GetComponent<Projectile>().value = 3;
            projGO.GetComponent<Projectile>().printNumber(); // Show the number of the projectile on the object
            //projGO.transform.position = transform.position; if you use position instead
            projGO.transform.position = launchPos;
            Rigidbody rigidB = projGO.GetComponent<Rigidbody>();

            // Find the delta from the launchPos to the mousePos3D
            //mouseDelta = mousePos3D - launchPos;
            rigidB.isKinematic = false;
            rigidB.velocity = direction * projectileSpeed;

            // Shot fired - Decrement '3 points (F)'.
            Main.S.SHOT_FIRED(projGO.GetComponent<Projectile>().value);
        }
    }
}
