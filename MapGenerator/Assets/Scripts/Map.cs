using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map : MonoBehaviour
{
	public static int width;
	public static int height;
	public static int scanRadius;
	public static Tile[,] tiles;

	public static void createMap(int w, int h)
    {
		width = w;
		height = h;
		scanRadius = (int)Math.Floor(0.05 * width);

		GameObject tileSet = new GameObject("Tiles");
		tiles = new Tile[width, height];
		
		//this is the code for loading the demo map
		/*
		var filePath = @"Elevation.csv";
		var data = File.ReadLines(filePath);
		int[,] exampleMapElevation = new int[height, width];
		int indexi = 0;
		int indexj = 0;
		foreach (string row in data)
		{
			indexj = 0;
			string[] items = row.Split(',');
			foreach (string s in items)
			{
				int n;
				if (s == "")
				{
					n = 0;
				}
				else
				{
					n = Convert.ToInt32(s);
				}
				exampleMapElevation[indexi, indexj] = n;
				indexj++;
			}
			indexi++;
		}
		filePath = @"Precipitation.csv";
		data = File.ReadLines(filePath);
		int[,] exampleMapPrecipitation = new int[height, width];
		indexi = 0;
		indexj = 0;
		foreach (string row in data)
		{
			indexj = 0;
			string[] items = row.Split(',');
			foreach (string s in items)
			{
				int n = Convert.ToInt32(s);
				exampleMapPrecipitation[indexi, indexj] = n;
				indexj++;
			}
			indexi++;
		}


		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				tiles[i, j] = new Tile(height, i, j, tileSet.transform);
				tiles[i, j].Elevation = exampleMapElevation[i, j];
				tiles[i, j].Precipitation = exampleMapPrecipitation[i, j];
				tiles[i, j].calculateBiome();
			}
		}
		*/
		//creating initial tiles
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				tiles[j, i] = new Tile(j, i, tileSet.transform);
				tiles[j, i].calculateBiome();
			}
		}
		//setting lefts and rights and ups and downs
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				if (j > 0)
				{
					tiles[j, i].left = tiles[j - 1, i];
				}
				else
				{
					tiles[j, i].left = tiles[width - 1, i];
				}

				if (j < width - 1)
				{
					tiles[j, i].right = tiles[j + 1, i];
				}
				else
				{
					tiles[j, i].right = tiles[0, i];
				}

				if (i > 0)
				{
					tiles[j, i].up = tiles[j, i - 1];
				}

				if (i < height - 1)
				{
					tiles[j, i].down = tiles[j, i + 1];
				}
			}
		}

		
		List<Tile> unsetTiles = CreateTerrain.createInitialMountains();
		while(unsetTiles.Count > 0){
			CreateTerrain.setElevations(unsetTiles);
		}
		
		for(int i=0; i < 5; i++){
			CreateTerrain.unclutterOcean();
		}
		
		float[,] heights = new float[height, width];
		for(int j=0; j<height; j++)
        {
            for (int i=0; i<width; i++)
            {
				//Debug.Log(j.ToString() + ", " + i.ToString());
				tiles[i, j].calculateBiome();
				heights[j, i] = (tiles[i, j].Elevation)/80;
            }
        }
		/*
		GameObject g = GameObject.Find("Terrain");
		Terrain t = g.GetComponent<Terrain>();
		t.terrainData.size = new Vector3(width, 5, height);
		Debug.Log("size: " + t.terrainData.size);
		t.terrainData.heightmapResolution = 2048;
		t.terrainData.SetDetailResolution(2048, 16);
		t.terrainData.SetHeights((int)t.terrainData.size[0]/2, (int)t.terrainData.size[2]/2, heights);
		Material oceanMat = Resources.Load("Ocean", typeof(Material)) as Material;
		t.terrainData.heightmapTexture = oceanMat.mainTexture;
		Debug.Log("reso: " + t.terrainData.baseMapResolution);*/
		}
		Tile.CalculateAllValues();
	}
}
