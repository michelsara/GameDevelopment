using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    //bullet 
    [SerializeField] private GameObject bullet;

    //bullet force
    [SerializeField] private float shootForce;

    //Gun stats
    [SerializeField] private float timeBetweenShooting, reloadTime;
    [SerializeField] private int magazineSize;
    [SerializeField] private bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    [SerializeField] private Transform attackPoint;

    //Graphics
    [SerializeField] private TextMeshProUGUI ammunitionDisplay;

    //bug fixing :D
    [SerializeField] private bool allowInvoke = true;

    private uint _shotCounter = 0;

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Start()
    {
    }

    private void Update()
    {
        MyInput();

        //Set ammo display, if it exists :D
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse1);
        else shooting = Input.GetKeyDown(KeyCode.Mouse1);

        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        Shot shotInstance = Instantiate(bullet, attackPoint.position, Quaternion.identity).GetComponent<Shot>();
        // assign your values to the Shot component
        shotInstance.Speed = shootForce;
        shotInstance.Direction = Camera.main.transform.forward;
        shotInstance.gameObject.name = $"SHOT_{_shotCounter:D3}";
        _shotCounter++;

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if (allowInvoke)
        {
            allowInvoke = false;
            Invoke("ResetShot", timeBetweenShooting);
        }
    }
    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
    }
    private void ReloadFinished()
    {
        //Fill magazine
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
