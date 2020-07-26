using System.Collections.Generic;
using UnityEngine;
using Alarm;
using Random = UnityEngine.Random;
using System;

public class SpawningTrailEmitter : Emitter
{

    /*One step closer to our dreams...
     This Emitter, upon spawning, will spawn in increments.
    It can either a trail in x or y coordinates every step, either a 
    trail that's been set, or a trail aiming at the player.
    Has Rotation Capablilities.
    Homing capabilities*/

    float angle = 0f;

    bool changeAngles = true;

    private void OnEnable()
    {
        initialPosition = gameObject.transform.position;
    }

    protected override void Start()
    {
        loopTimer = new Timer(3); //Reintergrated timer!
        existingProjectiles = new List<GameObject>();
        originObject = gameObject;
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
        if (rotationFocus < rotationFocusLimit)
            rotationFocus += rotationFocusIncrementVal;

        //Update rotation intensity
        if (rotationIntensity < rotationIntensityLimit)
            rotationIntensity += rotationIntensityIncrementVal;

        //Assing those values to our basic algorithm

        float angleStep = 360f / (_numberOfProjectiles * rotationFocus); //n scales the area in which bullets are spawn
                                                                         //You want to concentrate only on one side, but spread them, n is the one
        if (changeAngles)
            angle = g_angle * rotationIntensity;

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
                if (bulletInitialSpeed < bulletSpeedLimit) bulletInitialSpeed += incrementVal;
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

        if (currentInteval <= intervalCountLimit)
        {
            //It will not change angles during the spawning of the trail
            AngleLock();

            for (int i = 0; i <= _numberOfProjectiles - 1; i++)
            {

                GameObject tmpObj = ObjectPooler.GetMember(bulletMember);

                if (!tmpObj.activeInHierarchy)
                {
                    tmpObj.SetActive(true);
                    tmpObj.transform.position = initialPosition;
                    tmpObj.transform.rotation = Quaternion.Euler(0f, 0f, -angle);

                    if (tmpObj.GetComponent<GraphicAnimation>() != null)
                    {
                        GraphicAnimation graphicAnimation = tmpObj.GetComponent<GraphicAnimation>();
                        graphicAnimation.Animate(true);
                    }


                    if (tmpObj.GetComponent<Projectile>() != null)
                    {
                        Projectile projectile = tmpObj.GetComponent<Projectile>();
                        projectile.SetLifeTime(projectileLifeTime);
                        if (animateOnDestroy)
                        {
                            Debug.Log("Animation Set");
                            projectile.AnimateOnDestroy();
                        }
                        else
                        {
                            Debug.Log("No animation");
                            projectile.AnimateOnDestroy();
                        }
                    }

                    //Assign projectile priority from origin
                    tmpObj.GetComponent<GetOrignatedSpawnPoint>().priority = ParentPawn.priority;

                    //From here, we tell our temporary object where it came from
                    tmpObj.GetComponent<GetOrignatedSpawnPoint>().originatedSpawnPoint = originObject;

                    //tmpObj.GetComponent<Rigidbody2D>().AddForce(new Vector3(projectileMoveDir.x, projectileMoveDir.y, 0) * Time.fixedDeltaTime);
                    //Instead of adding force, we want to just move by increasing currentInterval every time
                    tmpObj.transform.Translate(new Vector3(currentInteval * xInterval, currentInteval * yInterval));
                }
                angle += angleStep;
            }

            currentInteval++;
        }
        else
        {
            //We are now allow to change the angle
            AllowAngleChange();
        }
    }

    void AngleLock()
    {
        changeAngles = false;
    }

    void AllowAngleChange()
    {
        changeAngles = true;
    }

    protected override void Loop()
    {
        if (loopTimer.SetFor(loopSpeed, 0))
        {
            g_angle += (int)rotation;
            SpawnBullets(numberOfProjectiles, bulletMember);
        }
    }

    protected override void UpdateStartPoint()
    {
        initialPosition = originObject.transform.position;
    }
}
