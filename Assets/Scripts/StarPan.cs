using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPan : MonoBehaviour
{
    protected float screenHeight;

    public ParticleSystem firstSky;
    public ParticleSystem secondSky;

    public float panSpeed = 1f;

    protected bool secondOverFirst;

    void Start()
    {
        screenHeight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;

        float newPosY = (firstSky.transform.position.y + firstSky.shape.scale.y / 2) + secondSky.shape.scale.y / 2;
        secondSky.transform.position = new Vector3(firstSky.transform.position.x, newPosY, firstSky.transform.position.z);
        secondSky.gameObject.SetActive(false);
        secondSky.gameObject.SetActive(true);
        secondOverFirst = true;
    }

    void Update()
    {
        firstSky.transform.Translate(0, panSpeed * Time.deltaTime, 0);
        secondSky.transform.Translate(0, panSpeed * Time.deltaTime, 0);

        if (Camera.main.transform.position.y + screenHeight < (firstSky.transform.position.y + firstSky.shape.scale.y / 2)
            && Camera.main.transform.position.y - screenHeight > (firstSky.transform.position.y - firstSky.shape.scale.y / 2))
        {
            secondSky.gameObject.SetActive(false);
            return;
        }
        else
        {
            secondSky.gameObject.SetActive(true);
        }

        if (!secondOverFirst)
        {
            if (Camera.main.transform.position.y + screenHeight >= (firstSky.transform.position.y + firstSky.shape.scale.y / 2))
            {
                float newPosY = (firstSky.transform.position.y + firstSky.shape.scale.y / 2) + secondSky.shape.scale.y / 2;
                secondSky.transform.position = new Vector3(firstSky.transform.position.x, newPosY, firstSky.transform.position.z);
                secondSky.gameObject.SetActive(false);
                secondSky.gameObject.SetActive(true);
                secondOverFirst = true;
            }
            else if (Camera.main.transform.position.y < (firstSky.transform.position.y - firstSky.shape.scale.y / 2))
            {
                ParticleSystem backupSky = firstSky;
                firstSky = secondSky;
                secondSky = backupSky;
                secondOverFirst = true;
            }
        }
        else
        {
            if (Camera.main.transform.position.y - screenHeight <= (firstSky.transform.position.y - firstSky.shape.scale.y / 2))
            {
                float newPosY = (firstSky.transform.position.y - firstSky.shape.scale.y / 2) - secondSky.shape.scale.y / 2;
                secondSky.transform.position = new Vector3(firstSky.transform.position.x, newPosY, firstSky.transform.position.z);
                secondSky.gameObject.SetActive(false);
                secondSky.gameObject.SetActive(true);
                secondOverFirst = false;
            }
            else if (Camera.main.transform.position.y > (firstSky.transform.position.y + firstSky.shape.scale.y / 2))
            {
                ParticleSystem backupSky = firstSky;
                firstSky = secondSky;
                secondSky = backupSky;
                secondOverFirst = false;
            }
        }
    }
}
