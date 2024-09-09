using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.InputSystem;

public class ShootingScript : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    [SerializeField]
    public GameObject bullet;
    [SerializeField]
    public Transform bulletExitTransform;
    private bool canFire = true;
    private float timer;
    public float timeBetweenFiring;

    private bool canUseTab = true;
    private float tabTimer = 0;
    public float timeBetweenTab = 5;

    private bool canUseD = true;
    private float dTimer = 0;
    public float timeBetweenD = 1;

    [SerializeField]
    InputHandler input;

    public float force = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Paused
        if (Time.timeScale == 0)
        {
            return;
        }
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (input.fire && canFire)
        {
            canFire = false;

            //float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            //rigid.rotation = Quaternion.Euler(0, 0, rot);
            fireBullet();
        }

        if (!canUseTab)
        {
            tabTimer += Time.deltaTime;
            if (tabTimer > timeBetweenTab)
            {
                canUseTab = true;
                tabTimer = 0;
            }

        }
        if (input.tabPressed && canUseTab)
        {
            canUseTab = false;
            StartCoroutine(fireD());
        }


        if (!canUseD)
        {
            dTimer += Time.deltaTime;
            if (dTimer > timeBetweenD)
            {
                canUseD = true;
                dTimer = 0;
            }

        }
        if (input.dPressed && canUseD)
        {
            canUseD = false;
            GlobalEffectsManager.doubleStacksOfFrothy();
        }
    }

    IEnumerator fireD()
    {
        for (int i = 0; i < 6; i++)
        {
            fireBullet();
            yield return new WaitForSeconds(0.1f);
        }

    }

    void fireBullet()
    {
        var bulletInstance = Instantiate(bullet, bulletExitTransform.position, Quaternion.identity);
        var projectileBehavior = bulletInstance.GetComponent<ProjectileBehavior>();
        projectileBehavior.explosionPosition = mousePos;

        var rigidBody = projectileBehavior.rigidBody;

        Vector3 direction = mousePos - transform.position;
        rigidBody.velocity = new Vector2(direction.x, direction.y).normalized * force;
    }
}
