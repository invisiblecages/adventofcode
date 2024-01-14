using System.Text;

class Universe
{
  private List<Galaxy> Galaxies { get; set; } = [];
  private List<int> EmptyRows { get; set; } = [];
  private List<int> EmptyCols { get; set; } = [];

  public Universe(string[] input)
  {
    ParseInput(input);
  }

  private void ParseInput(string[] input)
  {
    int rows = input.Length;
    int cols = input[0].Length;

    for (int i = 0; i < rows; i++)
    {
      bool emptyRow = true;
      bool emptyCol = true;

      for (int j = 0; j < cols; j++)
      {
        if (input[i][j] == '#')
        {
          Galaxies.Add(new Galaxy(i, j));
          emptyRow = false;
        }

        if (input[j][i] == '#')
        {
          emptyCol = false;
        }
      }

      if (emptyRow)
      {
        EmptyRows.Add(i);
      }

      if (emptyCol)
      {
        EmptyCols.Add(i);
      }
    }
  }

  public void Expand(int cosmicExpansion)
  {
    var newGalaxies = Galaxies.Select(g => new Galaxy(g.X, g.Y)).ToList();
    cosmicExpansion--;

    for (int i = 0; i < Galaxies.Count; i++)
    {
      foreach (int row in EmptyRows)
      {
        if (Galaxies[i].X >= row)
        {
          newGalaxies[i].X += cosmicExpansion;
        }
      }

      foreach (int col in EmptyCols)
      {
        if (Galaxies[i].Y >= col)
        {
          newGalaxies[i].Y += cosmicExpansion;
        }
      }
    }

    Galaxies = newGalaxies;
  }

  public ulong SumShortestPaths()
  {
    ulong sum = 0;

    for (int i = 0; i < Galaxies.Count; i++)
    {
      for (int j = i + 1; j < Galaxies.Count; j++)
      {
        (int x1, int y1) = (Galaxies[i].X, Galaxies[i].Y);
        (int x2, int y2) = (Galaxies[j].X, Galaxies[j].Y);

        sum += (ulong)(Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
      }
    }

    return sum;
  }

  public void PrintInfo()
  {
    Console.WriteLine($"Galaxies ({Galaxies.Count}): {string.Join(", ", Galaxies)}");
    Console.WriteLine($"Empty rows: {string.Join(", ", EmptyRows)}");
    Console.WriteLine($"Empty cols: {string.Join(", ", EmptyCols)}");
  }

  public void Print(bool showNumbers = false)
  {
    Dictionary<(int, int), string> galaxies = [];
    int rows = Galaxies.Select(g => g.X).Max() + 1;
    int cols = Galaxies.Select(g => g.Y).Max() + 1;

    for (int i = 0; i < Galaxies.Count; i++)
    {
      var key = (Galaxies[i].X, Galaxies[i].Y);
      galaxies[key] = showNumbers ? (i + 1).ToString() : "#";
    }

    for (int i = 0; i < rows; i++)
    {
      StringBuilder sb = new();

      for (int j = 0; j < cols; j++)
      {
        if (galaxies.TryGetValue((i, j), out var value))
        {
          sb.Append(value);
        }
        else
        {
          sb.Append('.');
        }
      }

      Console.WriteLine(sb.ToString());
    }
  }
}
