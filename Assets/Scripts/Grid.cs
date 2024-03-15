using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Source https://github.com/lordjesus/Packt-Introduction-to-graph-algorithms-for-game-developers
/// </summary>


public class Point  // Represents a point in a 2D grid with X and Y coordinates.
{
    public int X { get; set; }
    public int Y { get; set; }

    // Constructor for the Point class, initializing X and Y coordinates.
    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    // Overrides the default Equals method to compare two points for equality.
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj is Point)
        {
            Point p = obj as Point;
            return this.X == p.X && this.Y == p.Y;
        }
        return false;
    }

    // Overrides the default GetHashCode method to generate a hash code for the point.
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 6949;
            hash = hash * 7907 + X.GetHashCode();
            hash = hash * 7907 + Y.GetHashCode();
            return hash;
        }
    }

    // Overrides the default ToString method to provide a string representation of the point.
    public override string ToString()
    {
        return "P(" + this.X + ", " + this.Y + ")";
    }
}


// Represents the type of a cell in the grid (Empty, Road, Structure, SpecialStructure, None).
public enum CellType
{
    Empty,
    Road,
    Structure,
    SpecialStructure,
    None
}

// Represents a 2D grid with cells of different types.
public class Grid
{
    private CellType[,] _grid;
    private int _width;
    public int Width { get { return _width; } }
    private int _height;
    public int Height { get { return _height; } }
    private float _cellSize;

    private List<Point> _roadList = new List<Point>();
    private List<Point> _specialStructure = new List<Point>();
    private List<Point> _houseStructure = new List<Point>();

    // Constructor for the Grid class, initializing the grid with the specified width and height.
    public Grid(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _grid = new CellType[width, height];

        for (int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){

                _grid[x, y] = CellType.Empty;

                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.black, 100);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.black, 100);
            }
        }

        Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.black, 100);
        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.black, 100);
    }


    public Vector3 GetWorldPos(int x, int y){
        return new Vector3(x, 0, y) * _cellSize;
    }

    // Adding index operator to our Grid class so that we can use grid[][] to access specific cell from our grid. 
    public CellType this[int i, int j]
    {
        get
        {
            return _grid[i, j];
        }
        set
        {
            if (value == CellType.Road)
            {
                _roadList.Add(new Point(i, j));
            }
            if (value == CellType.SpecialStructure)
            {
                _specialStructure.Add(new Point(i, j));
            }
            if (value == CellType.Structure)
            {
                _houseStructure.Add(new Point(i, j));
            }
            _grid[i, j] = value;
        }
    }

    // Static method that checks if a cell is walkable based on its type.
    public static bool IsCellWakable(CellType cellType, bool aiAgent = false)
    {
        if (aiAgent)
        {
            return cellType == CellType.Road;
        }
        return cellType == CellType.Empty || cellType == CellType.Road;
    }

    /// <summary>
    /// Returns a random point from the list of road points.
    /// </summary>
    /// <returns>Random road point or null if the list is empty.</returns>
    public Point GetRandomRoadPoint()
    {
        if (_roadList.Count == 0)
        {
            return null;
        }
        return _roadList[UnityEngine.Random.Range(0, _roadList.Count)];
    }

    public Point GetRandomSpecialStructurePoint()
    {
        if (_specialStructure.Count == 0)
        {
            return null;
        }
        return _specialStructure[UnityEngine.Random.Range(0, _specialStructure.Count)];
    }

    /// <summary>
    /// Returns a random point from the list of house structure points.
    /// </summary>
    /// <returns>Random house structure point or null if the list is empty.</returns>
    public Point GetRandomHouseStructurePoint()
    {
        if (_houseStructure.Count == 0)
        {
            return null;
        }
        return _houseStructure[UnityEngine.Random.Range(0, _houseStructure.Count)];
    }

    /// <summary>
    /// Returns a list of all house structure points.
    /// </summary>
    /// <returns>List of all house structure points.</returns>
    public List<Point> GetAllHouses()
    {
        return _houseStructure;
    }

    internal List<Point> GetAllSpecialStructure()
    {
        return _specialStructure;
    }

    /// <summary>
    /// Returns a list of walkable adjacent cells based on a given point and considering an agent.
    /// </summary>
    /// <param name="cell">The point for which to find adjacent cells.</param>
    /// <param name="isAgent">Flag indicating whether the agent is considered.</param>
    /// <returns>List of walkable adjacent cells.</returns>
    public List<Point> GetAdjacentCells(Point cell, bool isAgent)
    {
        return GetWakableAdjacentCells((int)cell.X, (int)cell.Y, isAgent);
    }

    /// <summary>
    /// Returns the cost of entering a cell. Currently, it always returns 1, indicating uniform cost.
    /// </summary>
    /// <param name="cell">The point representing the cell.</param>
    /// <returns>The cost of entering the cell.</returns>
    public float GetCostOfEnteringCell(Point cell)
    {
        return 1;
    }

    /// <summary>
    /// Returns a list of all adjacent cells to a given point.
    /// </summary>
    /// <param name="x">X-coordinate of the point.</param>
    /// <param name="y">Y-coordinate of the point.</param>
    /// <returns>List of adjacent cells.</returns>
    public List<Point> GetAllAdjacentCells(int x, int y)
    {
        List<Point> adjacentCells = new List<Point>();
        if (x > 0)
        {
            adjacentCells.Add(new Point(x - 1, y));
        }
        if (x < _width - 1)
        {
            adjacentCells.Add(new Point(x + 1, y));
        }
        if (y > 0)
        {
            adjacentCells.Add(new Point(x, y - 1));
        }
        if (y < _height - 1)
        {
            adjacentCells.Add(new Point(x, y + 1));
        }
        return adjacentCells;
    }


    /// <summary>
    /// Returns a list of walkable adjacent cells based on a given point and considering an agent.
    /// </summary>
    /// <param name="x">X-coordinate of the point.</param>
    /// <param name="y">Y-coordinate of the point.</param>
    /// <param name="isAgent">Flag indicating whether the agent is considered.</param>
    /// <returns>List of walkable adjacent cells.</returns>
    public List<Point> GetWakableAdjacentCells(int x, int y, bool isAgent)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (IsCellWakable(_grid[adjacentCells[i].X, adjacentCells[i].Y], isAgent) == false)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    /// <summary>
    /// Returns a list of adjacent cells of a specific type based on a given point.
    /// </summary>
    /// <param name="x">X-coordinate of the point.</param>
    /// <param name="y">Y-coordinate of the point.</param>
    /// <param name="type">Type of cell to filter.</param>
    /// <returns>List of adjacent cells of the specified type.</returns>
    public List<Point> GetAdjacentCellsOfType(int x, int y, CellType type)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (_grid[adjacentCells[i].X, adjacentCells[i].Y] != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    /// <summary>
    /// Returns array [Left neighbour, Top neighbour, Right neighbour, Down neighbour]
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public CellType[] GetAllAdjacentCellTypes(int x, int y)
    {
        CellType[] neighbours = { CellType.None, CellType.None, CellType.None, CellType.None };
        if (x > 0)
        {
            neighbours[0] = _grid[x - 1, y];
        }
        if (x < _width - 1)
        {
            neighbours[2] = _grid[x + 1, y];
        }
        if (y > 0)
        {
            neighbours[3] = _grid[x, y - 1];
        }
        if (y < _height - 1)
        {
            neighbours[1] = _grid[x, y + 1];
        }
        return neighbours;
    }
}
