using System.Collections.Generic;
using UnityEngine;
using Alarm;
using Random = UnityEngine.Random;
//This is a set up for the near future
//This will be important for creating this game
//That has existing mechanics such as these
//I mean a DAN-MA-KU! DAMMAKA

public class RotationEmitter : Emitter
{
    void OnEnable()
    {
        initialPosition = gameObject.transform.position;
    }

    protected override void Start()
    {
        loopTimer = new Timer(3); //Reintergrated timer!
        existingProjectiles = new List<GameObject>();
        originObject = gameObject;
        pawnOriginObject = GetComponent<Pawn>();
    }

    void FixedUpdate()
    {
        UpdateStartPoint();

        if (Mathf.Abs(g_angle) > 359) g_angle = 0; //We do this to eliminate the risk of overflowing

        if (loop)
        {
            loopTimer.StartTimer(0);
            Loop();
        }
        else
        {
            loopTimer.SetToZero(0, true);
        }
    }

    public override void SpawnBullets(int _numberOfProjectiles, string bulletMember)
    {
        //Update rotation focus
        if(rotationFocus < rotationFocusLimit)
            rotationFocus += rotationFocusIncrementVal;

        //Update rotation intensity
        if(rotationIntensity < rotationIntensityLimit)
            rotationIntensity += rotationIntensityIncrementVal;

        //Assing those values to our basic algorithm

        float angleStep = 360f / (_numberOfProjectiles * rotationFocus); //n scales the area in which bullets are spawn
                                                             //You want to concentrate only on one side, but spread them, n is the one.
        float angle = g_angle * rotationIntensity;

        switch (distribution)
        {
            case DistributionType.Uniformed:
                //Do Nothing
                break;

            case DistributionType.Biformed:
                if (distStep == 0)
                {
                    bulletInitialSpeed *= 2;
                    distStep++;
                }
                else if (distStep == 1)
                {
                    bulletInitialSpeed /= 2;
                    distStep--;
                }
                break;

            case DistributionType.UniformedIncrement:
                if(bulletInitialSpeed<bulletSpeedLimit) bulletInitialSpeed += incrementVal;
                break;

            case DistributionType.BiformedIncrement:
                if (distStep == 0)
                {
                    bulletInitialSpeed *= 2 + (bulletInitialSpeed < bulletSpeedLimit ? incrementVal : 0);
                    distStep++;
                }
                else if (distStep == 1)
                {
                    bulletInitialSpeed /= 2 + (bulletInitialSpeed < bulletSpeedLimit ? incrementVal : 0);
                    distStep--;
                }
                break;

            case DistributionType.Scattered:
                bulletInitialSpeed = Random.Range(bulletInitialSpeed, bulletSpeedLimit + 500);
                break;

            default:
                break;
        }


        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {

            float projectileAngleX = initialPosition.x  + (Mathf.Sin((angle * Mathf.PI) / 180f)) * radius;
            float projectileAngleY = initialPosition.y + (Mathf.Cos((angle * Mathf.PI) / 180f)) * radius;

            Vector3 projectileVector = new Vector3(projectileAngleX, projectileAngleY, 0);
            Vector3 projectileMoveDir = (projectileVector - initialPosition).normalized * bulletInitialSpeed;

            //GameObject tmpObj = Instantiate(bullet[_index], startPoint, Quaternion.Euler(0f, 0f, -angle));
            GameObject tmpObj = ObjectPooler.GetMember(bulletMember);
            if (!tmpObj.activeInHierarchy)
            {
                tmpObj.SetActive(true);
                tmpObj.transform.position = initialPosition;
                tmpObj.transform.rotation = Quaternion.Euler(0f, 0f, -angle);

                try
                {
                    //Assign projectile priority from origin
                    tmpObj.GetComponent<GetOrignatedSpawnPoint>().priority = pawnOriginObject.priority;

                    //From here, we tell our temporary object where it came from
                    tmpObj.GetComponent<GetOrignatedSpawnPoint>().originatedSpawnPoint = originObject;

                } catch { /*No origin exception */}
                tmpObj.GetComponent<Rigidbody2D>().AddForce(new Vector3(projectileMoveDir.x, projectileMoveDir.y, 0) * Time.fixedDeltaTime);
            } 
            angle += angleStep;
        }
    }

    protected override void Loop()
    {

        if (loopTimer.SetFor(loopSpeed, 0))
        {
            g_angle += (int)rotation;
            SpawnBullets(numberOfProjectiles, bulletMember);
        }
    }

    public virtual void Remove(GameObject obj)
    {
        existingProjectiles.Remove(obj);
    }

   protected override void UpdateStartPoint()
    {
        initialPosition = originObject.transform.position;
    }
}
