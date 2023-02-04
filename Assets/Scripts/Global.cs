using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Global
{
    public static int PandN = 0;
    public static bool move = false;
    public static int MainSkill = 0;
    public static int size_x = 18;
    public static int size_y = 10;
    public static int[] num = new int[7];
    public static bool[,] shuidian = new bool[size_x, size_y];
    public static bool[,] huodian = new bool[size_x, size_y];
    //????
    private static int[,,] MapLandform = new int[size_x, size_y, 2];
    //????¦Ë??
    private static int[,,] MapPlayer = new int[size_x, size_y, 2];
    //???
    private static int[,,] MapElement = new int[size_x, size_y, 2];
    //?????
    private static int[,] MapSelect = new int[size_x, size_y];
    //?????????
    private static int[,] PlayerComeFrom = new int[size_x, size_y];
    //???????
    private static int[,] PlayerAction = 
    {
        {1,1 },
        {0,-1 },
        {1,0 },
        {-1,0 },
        {0,1 },
        {-1,-1 }
    };
    //?????
    public static void Init()
    {
        InitMap();
        SelectCancel();
    }

    private static void InitMap()
    {
        for (int i = 1; i <= 6; i++) num[i] = 0;
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                MapPlayer[i, j, 0] = 0;
                MapPlayer[i, j, 1] = 0;
            }
        }
        //??????
        string[] RawString = System.IO.File.ReadAllLines(@"Data\Map.txt");  //¡¤??

        for (int i = 0; i < RawString.Length; i++)     //
        {
            string[] ss = RawString[i].Split(' ');     //??????
            for(int j = 0; j < ss.Length; j++)
            {
                MapLandform[i, j, 0] = (int)float.Parse(ss[j]);
                MapLandform[i, j, 1] = -MapLandform[i, j, 0];
                if (MapLandform[i, j, 1] <= -4)
                    MapLandform[i, j, 1] = 4;
            }
        }

        RawString = System.IO.File.ReadAllLines(@"Data\Element.txt");  //¡¤??

        for (int i = 0; i*2 < RawString.Length; i++)     //
        {
            string[] ss = RawString[i].Split(' ');     //??????
            for (int j = 0; j < ss.Length; j++)
            {
                MapElement[i, j, 0] = (int)float.Parse(ss[j]);
            }
        }
        for (int i = 0; i*2 < RawString.Length; i++)     //
        {
            string[] ss = RawString[i + RawString.Length/2].Split(' ');     //??????
            for (int j = 0; j < ss.Length; j++)
            {
                MapElement[i, j, 1] = (int)float.Parse(ss[j]);
            }
        }
    }

    public static void SaveMap()
    {
        //?????????????
        string text = "";
        for(int i = 0;i< size_x; i++)
        {
            text += MapLandform[i, 0, 0].ToString();
            for (int j = 1; j < size_y; j++)
            {
                text += " " + MapLandform[i, j, 0].ToString();
            }
            text += "\r\n";
        }
        System.IO.File.WriteAllText(@"Data\Map.txt", text);
        text = "";
        for (int i = 0; i < size_x; i++)
        {
            text += MapElement[i, 0, 0].ToString();
            for (int j = 1; j < size_y; j++)
            {
                text += " " + MapElement[i, j, 0].ToString();
            }
            text += "\r\n";
        }
        for (int i = 0; i < size_x; i++)
        {
            text += MapElement[i, 0, 1].ToString();
            for (int j = 1; j < size_y; j++)
            {
                text += " " + MapElement[i, j, 1].ToString();
            }
            text += "\r\n";
        }
        System.IO.File.WriteAllText(@"Data\Element.txt", text);
    }

    public static int IfCellSelected = 0;

    //???¡¤??
    public static void SelectMap(int X, int Y, int dis, int landform)
    {
        Queue<Tuple<int, int>> q = new Queue<Tuple<int, int>>();
        q.Enqueue(new Tuple<int, int>(X, Y));
        MapSelect[X, Y] = dis + 1;
        for (; q.Count > 0;)
        {
            Tuple<int, int> now = q.First();
            q.Dequeue();
            int nowx = now.Item1;
            int nowy = now.Item2;
            for (int i = 0; i < 6; i++)
            {
                int dX = now.Item1 + PlayerAction[i, 0];
                int dY = now.Item2 + PlayerAction[i, 1];
                if (dX < 0) continue;
                if (dY < 0) continue;
                if (dX >= size_x) continue;
                if (dY >= size_y) continue;
                if (MapLandform[dX, dY, PandN] > landform) continue;
                if (MapLandform[dX, dY, PandN] < -landform) continue;
                if (MapPlayer[dX, dY, PandN] <= 0)
                {
                    if (MapSelect[dX, dY] == 0)
                    {
                        MapSelect[dX, dY] = MapSelect[nowx, nowy] - 1;
                        PlayerComeFrom[dX, dY] = 5 - i;
                        if (MapSelect[dX, dY] > 0)
                        {
                            q.Enqueue(new Tuple<int, int>(dX, dY));
                        }
                    }
                }
            }
            MapSelect[nowx, nowy] = 1;
        }
        MapSelect[X, Y] = 0;
        IfCellSelected = 1;
    }
    //???¡¤?????
    public static void SelectCancel()
    {
        for(int i = 0; i < size_x; i++)
        {
            for(int j = 0; j < size_y; j++)
            {
                MapSelect[i, j] = 0;
                PlayerComeFrom[i, j] = 0;
            }
        }
        IfCellSelected = 0;
    }
    //XY????
    public static int GetMapLandform(int X,int Y)
    {
        return MapLandform[X, Y, PandN];
    }
    //XY???
    public static int GetMapElement(int X, int Y)
    {
        return MapElement[X, Y, PandN];
    }
    //XY?????????
    public static int GetMapPlayer(int X, int Y)
    {
        return MapPlayer[X, Y, PandN];
    }
    //X_MAX
    public static int GetSizeX()
    {
        return size_x;
    }
    //Y_MAX
    public static int GetSizeY()
    {
        return size_y;
    }
    //???XY??????
    public static void ChangeSelected(int X,int Y,int Value)
    {
        MapSelect[X, Y] = Value;
    }
    //??????
    public static bool CellIfSelected(int X, int Y)
    {
        return MapSelect[X, Y] == 1;
    }
    //???XY?????????
    public static void SetPlayer(int X,int Y,int Value)
    {
        MapPlayer[X, Y, PandN] = Value;
    }
    //??????
    public static void SetElement(int X, int Y, int Value)
    {
        if (MapLandform[X, Y, PandN] != 4)
        {
            MapElement[X, Y, PandN] = Value;
        }
    }
    //???¦È??
    public static void HexcellUp(int X, int Y, int dis, int Addition)
    {
        Addition = (Addition % 7 + 7)%7;
        for(int i = 0; i < size_x; i++)
        {
            for(int j = 0; j < size_y; j++)
            {
                if (Distance(i - X, j - Y) <= dis)
                {
                    MapLandform[i, j, PandN] += Addition;
                    if (MapLandform[i, j, PandN] > 3)
                    {
                        MapLandform[i, j, PandN] = MapLandform[i, j, PandN] - 7;
                    }
                    if (MapLandform[i, j, PandN] < -3)
                    {
                        MapLandform[i, j, PandN] = MapLandform[i, j, PandN] + 7;
                    }
                    MapLandform[i, j, PandN ^ 1] = -MapLandform[i, j, PandN];
                    if (MapLandform[i, j, PandN] == 4)
                        MapLandform[i, j, PandN ^ 1] = 4;
                }
            }
        }
    }
    //???
    public static IEnumerator Water()
    {
        for (int k = 3; k > -3; k--)
        {
            for (int i = 0; i < size_x; i++)
            {
                for (int j = 0; j < size_y; j++)
                {
                    if (MapLandform[i, j, PandN] == k && MapElement[i, j, PandN] == 1)
                    {
                        for (int t = 0; t < 6; t++)
                        {
                            int dX = i + PlayerAction[t, 0];
                            int dY = j + PlayerAction[t, 1];
                            if (dX < 0) continue;
                            if (dY < 0) continue;
                            if (dX >= size_x) continue;
                            if (dY >= size_y) continue;
                            if (MapPlayer[dX, dY, PandN] > 0) continue;
                            if (MapLandform[dX, dY, PandN] < k)
                            {
                                if (MapElement[dX, dY, PandN] == 0)
                                {
                                    yield return new WaitForSeconds(1f);
                                    MapElement[dX, dY, PandN] = 1;
                                  
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    //?????????¦¶
    //2???????
    //3?????
    //4?????
    //5??????
    //6???¡À?
    //7?????
    public static void SelectPlayer(int X, int Y, int dis,int type)
    {
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                if (Distance(i - X, j - Y) <= dis && MapLandform[i, j, PandN] != 4) 
                {

                    if (type == 6)
                    {
                        if (MapPlayer[i, j, PandN] == 0 && MapLandform[i, j, PandN] == 0) 
                        {
                            MapSelect[i, j] = 1;
                        }
                    }
                    else
                    if (type >= 7)
                    {
                        if (MapPlayer[i, j, PandN] == 0 && MapElement[i, j, PandN] >= 0)
                        {
                            MapSelect[i, j] = 1;
                        }
                    }
                    else
                    {
                        if (MapPlayer[i, j, PandN] == 1 || MapElement[i, j, PandN] >= 0)
                        {
                            MapSelect[i, j] = 1;
                        }
                    }
                }
            }
        }
        if (type == 2 || type == 4 || type == 5 || type == 6) 
        {
            MapSelect[X, Y] = 0;
        }
        IfCellSelected = type;
    }
    //???????
    private static int Distance(int X,int Y)
    {
        if (X < 0)
        {
            X = -X;
            Y = -Y;
        }
        if (X == 0 && Y == 0) return 0;
        if (X >= 0 && Y > 0) return X > Y ? X : Y;
        return X - Y;

    }
    //??????????
    public static Queue<int> GetPoint(int X, int Y, int X0, int Y0)
    {
        if (X == X0 && Y == Y0)
        {
            return new Queue<int>();
        }
        Queue<int> q= GetPoint(X+ PlayerAction[PlayerComeFrom[X,Y],0],Y + PlayerAction[PlayerComeFrom[X, Y], 1], X0, Y0);
        q.Enqueue(5 - PlayerComeFrom[X, Y]);
        return q;
    }

    //?????????¦¶
    public static void SelectElement(int X, int Y, int element)
    {
        Queue<Tuple<int, int>> q = new Queue<Tuple<int, int>>();
        q.Enqueue(new Tuple<int, int>(X, Y));
        MapSelect[X, Y] = 1;
        for (; q.Count > 0;)
        {
            Tuple<int, int> now = q.First();
            q.Dequeue();
            int nowx = now.Item1;
            int nowy = now.Item2;
            for (int i = 0; i < 6; i++)
            {
                int dX = now.Item1 + PlayerAction[i, 0];
                int dY = now.Item2 + PlayerAction[i, 1];
                if (dX < 0) continue;
                if (dY < 0) continue;
                if (dX >= size_x) continue;
                if (dY >= size_y) continue;
                if (MapElement[dX, dY, PandN] != element) continue;
                if (MapSelect[dX, dY] == 0)
                {
                    MapSelect[dX, dY] = 1;
                    q.Enqueue(new Tuple<int, int>(dX, dY));
                }
            }
        }
    }
    //???????
    public static Tuple<int, int> randTreasure()
    {
        int X, Y,t=0;
        for(; t<=1000000;)
        {
            t = t + 1;
            X = UnityEngine.Random.Range(0, size_x - 1);
            Y = UnityEngine.Random.Range(0, size_y - 1);
            if (MapElement[X, Y, PandN] == 0 && MapLandform[X, Y, PandN] == 0 && MapPlayer[X, Y, PandN] == 0)
            {
                MapElement[X, Y, PandN] = -1;
                return new Tuple<int, int>(X, Y); 
            }
        }
        return new Tuple<int, int>(-1, -1);
    }
}
