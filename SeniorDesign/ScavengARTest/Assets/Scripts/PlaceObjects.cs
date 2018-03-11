using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    public AbstractMap Map;
    public List<string> Coordinates;

    void Start()
    {
        int num_objects = 1;
        Coordinates.Add("38.948484,-95.2390829");
        Coordinates.Add("38.954145,-95.2549679");
        Coordinates.Add("38.9603444,-95.246729");
        Coordinates.Add("38.8443333,-94.6524908");
        foreach (var item in Coordinates)
        {
            var latLonSplit = item.Split(',');
            var llpos = new Vector2d(double.Parse(latLonSplit[0]), double.Parse(latLonSplit[1]));
            var pos = Conversions.GeoToWorldPosition(llpos, Map.CenterMercator, Map.WorldRelativeScale);
            var gg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gg.transform.position = new Vector3((float)pos.x, 0, (float)pos.y);
            gg.transform.localScale += new Vector3(4,0,4);
            gg.name = "Object " + num_objects;
            num_objects++;
        }
    }
}