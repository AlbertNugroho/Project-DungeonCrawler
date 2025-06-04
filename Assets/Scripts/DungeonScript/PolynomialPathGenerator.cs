//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using MathNet.Numerics; // Ensure MathNet.Numerics is installed

//-----WIP----- kalo cukup waktu//

//public class PolynomialPathGenerator : MonoBehaviour
//{
//    public Vector2Int startPoint;
//    public Vector2Int endPoint;
//    public int numberOfIntermediatePoints = 5;
//    public int curveResolution = 20;

//    public HashSet<Vector2Int> StartPath(HashSet<Vector2Int> floorHashSet)
//    {
//        // Example: populate the floor (for demo purposes, you can remove or customize this)
//        for (int x = 0; x < 20; x++)
//        {
//            floorHashSet.Add(new Vector2Int(x, 0)); // Floor at y = 0
//        }

//        HashSet<Vector2Int> generatedPath = GenerateSmoothPath(startPoint, endPoint, numberOfIntermediatePoints, curveResolution, floorHashSet);
//        // Log points for debugging
//        foreach (var point in generatedPath)
//        {
//            Debug.Log(point);
//        }
//        return generatedPath;
//    }

//    HashSet<Vector2Int> GenerateSmoothPath(Vector2Int start, Vector2Int end, int numPoints, int resolution, HashSet<Vector2Int> floorHashSet)
//    {
//        List<double> xValues = new List<double>();
//        List<double> yValues = new List<double>();

//        // Add start point
//        xValues.Add(start.x);
//        yValues.Add(start.y);

//        // Generate intermediate points with random variations
//        System.Random rand = new System.Random();
//        for (int i = 1; i <= numPoints; i++)
//        {
//            float t = (float)i / (numPoints + 1);
//            float x = Mathf.Lerp(start.x, end.x, t) + rand.Next(-2, 3); // Small random offset
//            float y = Mathf.Lerp(start.y, end.y, t) + rand.Next(-2, 3);
//            xValues.Add(x);
//            yValues.Add(y);
//        }

//        // Add end point
//        xValues.Add(end.x);
//        yValues.Add(end.y);

//        // Fit a quadratic polynomial
//        double[] coefficients = Fit.Polynomial(xValues.ToArray(), yValues.ToArray(), 2);

//        // Generate smooth path using the polynomial
//        HashSet<Vector2Int> smoothPath = new HashSet<Vector2Int>();
//        for (int i = 0; i <= resolution; i++)
//        {
//            float t = (float)i / resolution;
//            float x = Mathf.Lerp(start.x, end.x, t);
//            float y = (float)(coefficients[0] + coefficients[1] * x + coefficients[2] * x * x);
//            Vector2Int point = new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y));

//            // Adjust the point upward until it is no longer on the floor.
//            while (floorHashSet.Contains(point))
//            {
//                point = new Vector2Int(point.x, point.y + 1);
//            }
//            smoothPath.Add(point);
//        }

//        return smoothPath;
//    }
//}
