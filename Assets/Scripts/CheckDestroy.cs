using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alarm;

public class CheckDestroy : MonoBehaviour
{

    #region Private Members
    private Timer destroyTimer;
    private GameObject origin;
    private Standard_Shoot standardShoot;
    #endregion

    void Awake()
    {
        destroyTimer = new Timer(1);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        Vector3 targetVector = (standardShoot.target.position - origin.transform.position).normalized;

        origin = FindObjectOfType<Shoot_Trig>().origin; //Will find the gameObject that shoot the bullet out
        standardShoot = FindObjectOfType<Standard_Shoot>();
        transform.position = origin.transform.position;
        GetComponent<GetOrignatedSpawnPoint>().originatedSpawnPoint = origin;

        //This is what I wanted
        transform.rotation = Quaternion.FromToRotation(standardShoot.target.position, transform.position);

        GetComponent<Rigidbody2D>().AddForce(targetVector * standardShoot.speed * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer.StartTimer(0);
        if (destroyTimer.currentTime[0] > 10)
        {
            gameObject.SetActive(false);
        }
    }
}
