using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City
{
    //static fields
    public static List<City> cityList = new List<City>();

    //instance fields
    public int x, y;
    public int wealth;
    public float water;
    public float lumber;
    public float food;
    public int roads;

    private City(int xVal, int yVal)
    {
        x = xVal;
        y = yVal;
        cityList.Add(this);
        SetResources(this);
    }

    //create num number of cities
    public static void GenerateCities(int num)
    {
        while (num > 0)
        {
            GenerateCity();
            num--;
        }
    }

    //create a single city
    /*
    private static void GenerateCity()
    {
        Tile[,] tiles = Map.tiles;
        float bestVal = int.MinValue;
        ref Tile bestTile = ref tiles[0, 0];

        foreach (Tile tile in tiles)
        {
            if (tile.City != null || tile.Biome == Biome.Ocean)
            {
                continue;
            }

            float currentVal = GetValue(tile);
            if (currentVal > bestVal)
            {
                bestVal = currentVal;
                bestTile = tile;
            }
            else if (currentVal == bestVal)
            {
                if (Random.r.NextDouble() > 0.5)
                {
                    bestTile = tile;
                }
            }
        }

        //Debug.Log("Best City Location (X:" + bestTile.X + " Y:" + bestTile.Y + ")");
        AddCity(bestTile);
        GameObject city = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        city.gameObject.GetComponent<MeshRenderer>().material = cityMat;
        city.transform.position = new Vector3(bestTile.X, 6, bestTile.Y);
        City newCity = new City(bestTile.X, bestTile.Y);
        bestTile.City = newCity;
        Debug.Log("best val:" + bestVal);
    }
    */
    private static void GenerateCity()
    {
        Tile[,] tiles = Map.tiles;

        Tile randTile = tiles[RandomNum.r.Next(Map.width), RandomNum.r.Next(Map.height)];
        while (randTile.Biome == Biome.Ocean)
        {
            randTile = tiles[RandomNum.r.Next(Map.width), RandomNum.r.Next(Map.height)];
        }

        GameObject city = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("house"));
        city.transform.position = new Vector3(randTile.X, (randTile.Elevation / 10) + 1, randTile.Y);

        City newCity = new City(randTile.X, randTile.Y);
        randTile.City = newCity;
    }

    public static void TradeRouteFood(City start)
    {
        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach (City c in cityList)
        {
            if (c.food > bestGuessVal)
            {
                Debug.Log("food:" + c.food);
                bestGuessVal = c.food;
                bestGuessCity = c;
            }
        }
        Debug.Log("Best:" + bestGuessVal);

        Road.CreateRoad(start, bestGuessCity);
    }
    public static void TradeRouteWater(City start)
    {
        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach (City c in cityList)
        {
            if (c.water > bestGuessVal && c != start)
            {
                bestGuessVal = c.water;
                bestGuessCity = c;
            }
        }

        Road.CreateRoad(start, bestGuessCity);
    }
    public static void TradeRouteLumber(City start)
    {
        float bestGuessVal = 0;
        City bestGuessCity = null;
        foreach (City c in cityList)
        {
            if (c.lumber > bestGuessVal && c != start)
            {
                bestGuessVal = c.lumber;
                bestGuessCity = c;
            }
        }

        Road.CreateRoad(start, bestGuessCity);
    }

    /*public static float GetValue(Tile tile)
    {
        return tile.tileValue;
    }*/

    /* private static void AddCity(Tile tile)
     {
         Tile[,] tiles = Map.tiles;

         for (int i = -Map.scanRadius; i <= Map.scanRadius; i++)
         {
             for (int j = -Map.scanRadius; j <= Map.scanRadius; j++)
             {
                 if (tile.X + i < 0 || tile.Y + j < 0 || tile.X + i > Map.width - 1 || tile.Y + j > Map.height - 1)
                 {
                     continue;
                 }

                 Tile temp = tiles[tile.X + i, tile.Y + j];
                 temp.tileValue -= 50 * Map.scanRadius;
             }
         }
     }*/

    private static void SetResources(City city)
    {
        city.wealth += 1000000;

        Tile[,] tiles = Map.tiles;

        for (int i = -Map.scanRadius; i <= Map.scanRadius; i++)
        {
            for (int j = -Map.scanRadius; j <= Map.scanRadius; j++)
            {
                if (city.x + i < 0 || city.y + j < 0 || city.x + i > Map.width - 1 || city.y + j > Map.height - 1)
                {
                    continue;
                }

                Tile temp = tiles[city.x + i, city.y + j];

                switch (temp.Biome)
                {
                    case Biome.Ocean:
                        city.water += 50;
                        city.food += 20;
                        break;
                    case Biome.Mountain:
                        city.food += 5;
                        city.lumber += 10;
                        break;
                    case Biome.Tundra:
                        city.lumber += 10;
                        city.water += 10;
                        city.food += 5;
                        break;
                    case Biome.BorealForest:
                        city.water += 15;
                        city.food += 15;
                        city.lumber += 15;
                        break;
                    case Biome.Prairie:
                        city.food += 15;
                        city.water += 10;
                        break;
                    case Biome.Shrubland:
                        city.food += 15;
                        city.water += 10;
                        city.lumber += 5;
                        break;
                    case Biome.TemperateForest:
                        city.food += 15;
                        city.lumber += 25;
                        city.water += 10;
                        break;
                    case Biome.Desert:
                        city.food += 5;
                        break;
                    case Biome.Savanna:
                        city.water += 10;
                        city.lumber += 20;
                        city.food += 20;
                        break;
                    case Biome.Rainforest:
                        city.water += 30;
                        city.lumber += 20;
                        city.food += 15;
                        break;
                }
            }
        }
    }

    public static int BiomeValue(Tile tile)
    {
        switch (tile.Biome)
        {
            case Biome.BorealForest:
                return 20;
            case Biome.Desert:
                return -20;
            case Biome.Mountain:
                return -10;
            case Biome.Ocean:
                return 25;
            case Biome.Prairie:
                return 20;
            case Biome.Rainforest:
                return 25;
            case Biome.Savanna:
                return 20;
            case Biome.Shrubland:
                return 10;
            case Biome.TemperateForest:
                return 25;
            case Biome.Tundra:
                return 5;
            default:
                return 0;
        }
    }

    public static int HasCity(Tile tile)
    {
        if (tile.City != null)
        {
            return -50 * Map.scanRadius;
        }
        else
        {
            return 0;
        }
    }
}
