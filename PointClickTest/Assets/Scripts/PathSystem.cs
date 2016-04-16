using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Handles a path through the scene for player movement.
public class PathSystem : MonoBehaviour
{
    //VARIABLES
    //The path itself as vector 3 nodes
    public Vector3[] path;
	
    //Draw the path in the editor
    void OnDrawGizmos()
    {
        for(int i = 0; i < path.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(path[i], 0.3f);
            if(i+1 < path.Length)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
        }
    }
    
    //Find the point along the line between the two points
    public Vector3 PathPoint(float givenX)
    {
        //Find the point A(0) and point B(1) based on x;
        Vector3[] points = Points(givenX);
        //Debug.Log("A: " + pointA + " B: " + pointB);

        //Find the path point.
        Vector3 lineVec = points[1] - points[0];

        //Line equation is P(t) = Point1+LineVec x t
        float t;
        t = (givenX - points[0].x) / lineVec.x;
        Vector3 pathPoint = points[0] + lineVec * t;

        //error correction
        if(t < 0 || t > 1)
        {
            pathPoint = Vector3.zero;
        }

        return pathPoint;
    }

    //Compares X to our points to find which nodes we're between.
    Vector3[] Points(float givenX)
    {
        Vector3[] points = new Vector3[2];
        points[1] = path[path.Length - 1];
        points[0] = path[0];

        foreach (Vector3 p in path)
        {
            //find the nodes before and after the click
            if(p.x > givenX && p.x < points[1].x)
            {
                points[1] = p;
            }
            if (p.x < givenX && p.x > points[0].x)
            {
                points[0] = p;
            }
        }

        //returns both points, 0 being the one before and 1 the one previous
        return points;
    }
}
