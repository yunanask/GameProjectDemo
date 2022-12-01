using System;
using System.Collections.Generic;
using System.Linq;


public class Global
{
    private static int size_x = 10;
    private static int size_y = 10;
    private static int[,] MapLandform = new int[size_x, size_y];
    private static int[,] MapPlayer = new int[size_x, size_y];
    private static int[,] MapElement = new int[size_x, size_y];
    private static int[,] MapSelect = new int[size_x, size_y];
    private static int[,] PlayerComeFrom = new int[size_x, size_y];
    private static int[,] PlayerAction = 
    {
        {1,1 },
        {0,-1 },
        {1,0 },
        {-1,0 },
        {0,1 },
        {-1,-1 }
    };
    public static void Init()
    {
        InitMap();
        MapElement[1, 1] = 1;
        MapElement[2, 2] = 2;
        MapElement[5, 2] = 3;
    }

    private static void InitMap()
    {
        string[] RawString = System.IO.File.ReadAllLines(@"Data\Map.txt");  //·��

        for (int i = 0; i < RawString.Length; i++)     //
        {
            string[] ss = RawString[i].Split(' ');     //�ض��ֽ�
            for(int j = 0; j < ss.Length; j++)
            {
                MapLandform[i, j] = (int)float.Parse(ss[j]);
            }
        }
    }

    public static void SaveMap()
    {
        string text = "";
        for(int i = 0;i< size_x; i++)
        {
            text += MapLandform[i, 0].ToString();
            for (int j = 1; j < size_y; j++)
            {
                text += " " + MapLandform[i, j].ToString();
            }
            text += "\r\n";
        }
        System.IO.File.WriteAllText(@"Data\Map.txt", text);
    }

    public static bool IfCellSelected = false;

    //��ʾ·��
    public static void SelectMap(int X, int Y, int dis)
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
                if (MapLandform[dX, dY] != 0) continue;
                if (MapPlayer[dX, dY] == 0)
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
        IfCellSelected = true;
    }

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
        IfCellSelected = false;
    }

    public static int GetMapLandform(int X,int Y)
    {
        return MapLandform[X, Y];
    }

    public static int GetMapElement(int X, int Y)
    {
        return MapElement[X, Y];
    }

    public static int GetSizeX()
    {
        return size_x;
    }

    public static int GetSizeY()
    {
        return size_y;
    }

    public static bool CellIfSelected(int X, int Y)
    {
        return MapSelect[X, Y] == 1;
    }

    public static void SetPlayer(int X,int Y,int Value)
    {
        MapPlayer[X, Y] = Value;
    }

    
    public static void HexcellUp(int X, int Y, int dis, int Addition)
    {
        for(int i = 0; i < size_x; i++)
        {
            for(int j = 0; j < size_y; j++)
            {
                if (Distance(i - X, j - Y) <= dis)
                {
                    MapLandform[i, j] += Addition;
                    if (MapLandform[i, j] > 4)
                    {
                        MapLandform[i, j] = -3;
                    }
                }
            }
        }
    }

    public static void SelectPlayer(int X, int Y, int dis)
    {
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                if (Distance(i - X, j - Y) <= dis)
                {
                    if(MapPlayer[i, j] == 1)
                    {
                        MapSelect[i, j] = 1;
                    }
                }
            }
        }
        MapSelect[X, Y] = 0;
        IfCellSelected = true;
    }

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
}