using UnityEngine;
using System.Collections;

public class SlidingPlatform : Platform
{
    public Axis axis;
    float counter = 0;
    float speed;

    public float distance;
    private bool move;

    public bool Move
    {
        get { return move; }
        set { move = value; }
    }

    Vector3 platformStartPos;

    private void Start()
    {
        platformStartPos = transform.position;
        speed = 0.4f;
        move = true;
        StartCoroutine(StopMoving());
    }

    void Update()
    {
        if (move)
        {
            float movementDir = Mathf.Sin(counter) * distance;

            counter += Time.deltaTime * speed;

            switch (axis)
            {
                case Axis.X:
                    transform.position = ((transform.right * movementDir) + platformStartPos);
                    break;
                case Axis.Y:
                    transform.position = ((transform.up * movementDir) + platformStartPos);
                    break;
                case Axis.Z:
                    transform.position = ((transform.forward * movementDir) + platformStartPos);
                    break;
            }
        }
        else
        {
            return;
        }
    }

    IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.5f);
        move = false;
    }
}
