using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarMovement : MonoBehaviour
{
    public List<Vector3> ourPath;

    //Move to the endpoint, get the direction to the next point, move to it, then remove it from the list.
    IEnumerator PathToPoint()
    {
        while(ourPath.Count > 0)
        {
            Vector3 dir = (ourPath[0] - transform.position).normalized;
            transform.Translate(dir * 2 * Time.smoothDeltaTime);
            if(Vector3.Distance(transform.position, ourPath[0]) < 0.2f)
            {
                ourPath.Remove(ourPath[0]);
            }
            yield return null;
        }
        yield return null;
    }

    //Creates a path based on the inbetween nodes and the end point and start the movement coroutine.
    public void SetPath(Vector3[] path, Vector3 pathPoint)
    {
        StopAllCoroutines();
        ourPath = new List<Vector3>();

        if(pathPoint.x < transform.position.x)
        {
            //going right to left
            for (int i = path.Length-1; i > 0; i--)
            {
                ourPath.Add(path[i]);
                Debug.Log(path[i]);
                if (path[i].x > transform.position.x || path[i].x < pathPoint.x)
                {
                    ourPath.Remove(path[i]);
                }
            }
        }
        else
        {
            //going left to right
            for (int i = 0; i < path.Length; i++)
            {
                ourPath.Add(path[i]);
                if (path[i].x < transform.position.x || path[i].x > pathPoint.x)
                {
                    ourPath.Remove(path[i]);
                }
            }
        }
        ourPath.Add(pathPoint);
        StartCoroutine("PathToPoint");
    }
}
