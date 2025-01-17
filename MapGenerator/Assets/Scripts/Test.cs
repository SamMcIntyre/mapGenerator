using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Camera cam;
    int width = 402;	//x
    int height = 268;	//y

    // Start is called before the first frame update
    void Start()
    {
        width = Mathf.RoundToInt(width*UIData.sizeMultiplier);
        height = Mathf.RoundToInt(height*UIData.sizeMultiplier);
        if(width <= 120 || height == 80)
        {
            width = 120;
            height = 80;
        }

        //used for perspective Camera
        cam.transform.position = new Vector3(width / 2, height, height / 2);

        var watch = System.Diagnostics.Stopwatch.StartNew();
        Map.createMap(width, height);
        watch.Stop();
        Debug.Log("Time to create all tiles is:" + watch.ElapsedMilliseconds + "ms");

        //Simple Border creation
        watch = System.Diagnostics.Stopwatch.StartNew();
        if(UIData.borderMultiplier>0)
        {
            Border.generateBorders(Map.tiles, Mathf.RoundToInt(Map.width*0.01f+UIData.borderMultiplier));
            Border.SetTileCountries();
        }
        watch.Stop();
        Debug.Log("Time to create all tiles is:" + watch.ElapsedMilliseconds + "ms");

        //very simplistic city creation
        //(currently only checks 8 nearby tiles to get tile's creation value)

        watch.Restart();
        if(UIData.cityMultiplier>0)
        {
            City.GenerateCapitals();
            City.GenerateCities(Mathf.RoundToInt(5 * Map.scanRadius * UIData.cityMultiplier));
        }
        watch.Stop();
		Debug.Log("Time to create cities is:" + watch.ElapsedMilliseconds + "ms");


        watch.Restart();
        if (UIData.roadMultiplier > 0)
        {
            int numRoads = Mathf.RoundToInt(UIData.roadMultiplier * 2*City.cityList.Count);
            for (int i = 0; i < numRoads; i++)
            {
                City c = City.cityList[RandomNum.r.Next(0, City.cityList.Count)];

                if(c.Food < c.Water)
                {
                    if(c.Lumber<c.Food)
                    {
                        //Road for lumber
                        City.TradeRouteLumber(c);
                    }
                    else
                    {
                        //Road for food
                        City.TradeRouteFood(c);
                    }
                }
                else
                {
                    if(c.Lumber<c.Water)
                    {
                        //Road for lumber
                        City.TradeRouteLumber(c);
                    }
                    else
                    {
                        //Road for water
                        City.TradeRouteWater(c);
                    }
                }
            }
        }
        
        watch.Stop();
        Debug.Log("Time to create roads is:" + watch.ElapsedMilliseconds + "ms");

        watch.Restart();

        //HUGE FPS savers
        Map.BorderTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.BorealForestTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.DesertTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.MountainTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.OceanTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.PrairieTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.RainforestTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.SavannaTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.ShrublandTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.TemperateForestTiles.GetComponent<MeshCombiner>().CombineMeshes();
        Map.TundraTiles.GetComponent<MeshCombiner>().CombineMeshes();
        watch.Stop();
        Debug.Log("Time to combine meshes is:" + watch.ElapsedMilliseconds + "ms");
    }
}
