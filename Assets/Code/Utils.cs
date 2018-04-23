using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Utils
{
    private static Texture2D texture = new Texture2D(1, 1);
    public static void DrawScreenRect(Rect rect, Color color)
    {
        if (texture.GetPixel(0, 0) != color)
        {
            texture.SetPixel(0, 0, color);
        }
        GUI.color = color;
        GUI.DrawTexture(rect, texture);
        GUI.color = Color.white;
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static Vector3 ParseVector3(string vector)
    {
        string[] coords = vector.Split(',');
        return new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
    }

    public static Vector3[] ParseVectorArray(string[] vectors)
    {
        Vector3[] output = new Vector3[vectors.Length];

        for (int i = 0; i < vectors.Length; i++)
            output[i] = ParseVector3(vectors[i]);
        return output;
    }

    public static string VectorArrayToString(Vector3[] vectors)
    {
        StringBuilder sb = new StringBuilder();

        Vector3 v;
        for (int i = 0; i < vectors.Length; i++)
        {
            v = vectors[i];
            sb.Append("|" + v.x + "," + v.y + "," + v.z);
        }
        return sb.ToString();
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static void DrawScreenRectFilled(Rect rect, float thickness, Color borderColor, Color fillColor)
    {
        DrawScreenRect(rect, fillColor);
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), borderColor);
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), borderColor);
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), borderColor);
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), borderColor);
    }
}