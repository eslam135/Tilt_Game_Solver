using System.Collections.Generic;
using System.Diagnostics;
public class TreeNode
{
    public HashSet<int[]> currState { get; set; }
    public HashSet<int> State { get; set; }
    public char MoveDirection { get; set; }
    public bool isTarget { get; set; }
    public char OppositeMove { get; set; }
    public long sliders { get; set; }
    public List<char> currentPath { get; set; }
    public TreeNode(HashSet<int> state, char moveDirection, char oppositeMove, bool isTarget, long sliders, List<char> currentPath, HashSet<int[]> currState)
    {
        this.State = state;
        this.MoveDirection = moveDirection;
        this.OppositeMove = oppositeMove;
        this.isTarget = isTarget;
        this.sliders = sliders;
        this.currentPath = currentPath;
        this.currState = currState;
    }
}


public static class TiltGame
{
    public static int dimension = 0;
    public static HashSet<int[]> obstacles = new();
    public static HashSet<int> hashed_obstacles = new();
    public static bool flag = false;
    public static int TargetHash = 0;

    public static Tuple<HashSet<int>, HashSet<int[]>> ReadFile(string path)
    {
        string[] lines = File.ReadAllLines(path);

        HashSet<int> sliders = new();
        HashSet<int[]> currSliders = new();
        string[] loc = lines[^1].Split(", ");
        int x = int.Parse(loc[1]), y = int.Parse(loc[0]);
        dimension = int.Parse(lines[0]);
        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] symbols = lines[i].Split(", ");
            for (int j = 0; j < symbols.Length; j++)
            {
                int[] currenNode = { i - 1, j };
                int hash = 17 * (currenNode[0] + (currenNode[1]) * dimension + 1);
                if (i - 1 == x && j == y)
                {
                    TargetHash = hash;
                }
                else
                {
                    char symbol = symbols[j][0];
                    if (symbol == '#')
                    {
                        hashed_obstacles.Add(hash);
                        obstacles.Add(currenNode);
                    }
                    else if (symbol == 'o')
                    {
                        sliders.Add(hash);
                        currSliders.Add(currenNode);
                    }
                }
            }
        }
        flag = sliders.Count > dimension;
        //flag = true;
        Tuple<HashSet<int>, HashSet<int[]>> finalval = new(sliders, currSliders);
        return finalval;
    }

    public static Tuple<Tuple<bool, long>, Tuple<HashSet<int>, HashSet<int[]>?>> Move(char direction
        , HashSet<int>? hashed_sliders, HashSet<int[]>? previousSliders, bool write = false)
    {
        Tuple<Tuple<bool, long>, Tuple<HashSet<int>, HashSet<int[]>>> x;
        bool solved = false;
        long final_hash = 17;
        HashSet<int[]> newSliders = new();
        if (flag)
        {
            HashSet<int> checkingSliders = new(hashed_sliders);
            if (direction == 'L')
            {
                int[] currentPos = { 0, 0 };
                int[] pointer = { 0, 0 };

                for (int i = 0; i < dimension; i++)
                {
                    if (solved && !write)
                    {
                        break;
                    }
                    for (int j = 0; j < dimension; j++)
                    {
                        currentPos[0] = i;
                        currentPos[1] = j;
                        int currhash = 17 * (currentPos[0] + (currentPos[1]) * dimension + 1);
                        int ptr_hash = 17 * (pointer[0] + (pointer[1]) * dimension + 1);
                        if (hashed_obstacles.Contains(currhash) && (currentPos[1] != dimension - 1))
                        {
                            pointer[1] = j + 1;
                        }
                        else if (checkingSliders.Contains(currhash) && !(ptr_hash == currhash))
                        {
                            if (ptr_hash == TargetHash)
                            {
                                solved = true;
                                if (!write)
                                    break;
                            }
                            checkingSliders.Remove(currhash);
                            checkingSliders.Add(ptr_hash);
                            pointer[1] += 1;
                            final_hash = final_hash * 31 + ptr_hash;
                        }
                        else if (checkingSliders.Contains(currhash) && (ptr_hash == currhash))
                        {
                            pointer[1] += 1;
                        }
                    }
                    pointer[0] += 1;
                    pointer[1] = 0;
                }
            }
            else if (direction == 'R')
            {

                int[] pointer = { 0, dimension - 1 };
                int[] currentPos = { 0, 0 };
                for (int i = 0; i < dimension; i++)
                {
                    if (solved && !write)
                    {
                        break;
                    }
                    for (int j = dimension - 1; j >= 0; j--)
                    {
                        currentPos[0] = i;
                        currentPos[1] = j;
                        int currhash = 17 * (currentPos[0] + (currentPos[1]) * dimension + 1);
                        int ptr_hash = 17 * (pointer[0] + (pointer[1]) * dimension + 1);

                        if (hashed_obstacles.Contains(currhash) && (currentPos[1] != 0))
                        {
                            pointer[1] = j - 1;
                        }
                        else if (checkingSliders.Contains(currhash) && !(ptr_hash == currhash))
                        {
                            if (ptr_hash == TargetHash)
                            {
                                solved = true;
                                if (!write)
                                    break;
                            }
                            checkingSliders.Remove(currhash);
                            checkingSliders.Add(ptr_hash);
                            pointer[1] -= 1;
                            final_hash = final_hash * 31 + ptr_hash;
                        }
                        else if (checkingSliders.Contains(currhash) && (ptr_hash == currhash))
                        {
                            pointer[1] = j - 1;
                        }
                    }
                    pointer[0] += 1;
                    pointer[1] = dimension - 1;
                }
            }
            else if (direction == 'D')
            {

                int[] currentPos = { dimension - 1, 0 };
                int[] pointer = { dimension - 1, 0 };
                for (int i = 0; i < dimension; i++)
                {
                    if (solved && !write)
                    {
                        break;
                    }
                    for (int j = dimension - 1; j >= 0; j--)
                    {
                        currentPos[0] = j;
                        currentPos[1] = i;
                        int currhash = 17 * (currentPos[0] + (currentPos[1]) * dimension + 1);
                        int ptr_hash = 17 * (pointer[0] + (pointer[1]) * dimension + 1);
                        if (hashed_obstacles.Contains(currhash) && (currentPos[0] != 0))
                        {
                            pointer[0] = j - 1;
                        }
                        else if (checkingSliders.Contains(currhash) && !(ptr_hash == currhash))
                        {
                            if (ptr_hash == TargetHash)
                            {
                                solved = true;
                                if (!write)
                                    break;
                            }
                            checkingSliders.Remove(currhash);
                            checkingSliders.Add(ptr_hash);
                            pointer[0] -= 1;
                            final_hash = final_hash * 31 + ptr_hash;
                        }
                        else if (checkingSliders.Contains(currhash) && (ptr_hash == currhash))
                        {
                            pointer[0] = j - 1;
                        }
                    }
                    pointer[0] = dimension - 1;
                    pointer[1] += 1;
                }
            }
            else if (direction == 'U')
            {
                int[] currentPos = { 0, 0 };
                int[] pointer = { 0, 0 };
                for (int i = 0; i < dimension; i++)
                {
                    if (solved && !write)
                    {
                        break;
                    }
                    for (int j = 0; j < dimension; j++)
                    {
                        currentPos[0] = j;
                        currentPos[1] = i;
                        int currhash = 17 * (currentPos[0] + (currentPos[1]) * dimension + 1);
                        int ptr_hash = 17 * (pointer[0] + (pointer[1]) * dimension + 1);
                        if (hashed_obstacles.Contains(currhash) && (currentPos[0] != dimension - 1))
                        {
                            pointer[0] = j + 1;
                        }
                        else if (checkingSliders.Contains(currhash) && !(ptr_hash == currhash))
                        {
                            if (ptr_hash == TargetHash)
                            {
                                solved = true;
                                if (!write)
                                    break;
                            }
                            checkingSliders.Remove(currhash);
                            checkingSliders.Add(ptr_hash);
                            pointer[0] += 1;
                            final_hash = final_hash * 31 + ptr_hash;
                        }
                        else if (checkingSliders.Contains(currhash) && (ptr_hash == currhash))
                        {
                            pointer[0] = j + 1;
                        }
                    }
                    pointer[0] = 0;
                    pointer[1] += 1;
                }
            }
            return new(new(solved, final_hash), new(checkingSliders, null));
        }
        else
        {
            HashSet<int> new_HashedSliders = new();
            int di = 0, dj = 0;
            List<int[]> sorted_sliders = new(previousSliders);
            switch (direction)
            {
                case 'L': dj = -1; break;
                case 'R': dj = 1; break;
                case 'U': di = -1; break;
                case 'D': di = 1; break;
            }


            switch (direction)

            {
                case 'L':
                    sorted_sliders.Sort((arr1, arr2) => arr1[1].CompareTo(arr2[1]));
                    break;
                case 'R':
                    sorted_sliders.Sort((arr1, arr2) => arr2[1].CompareTo(arr1[1]));
                    break;
                case 'U':
                    sorted_sliders.Sort((arr1, arr2) => arr1[0].CompareTo(arr2[0]));
                    break;
                case 'D':
                    sorted_sliders.Sort((arr1, arr2) => arr2[0].CompareTo(arr1[0]));
                    break;
            }

            foreach (var slider in sorted_sliders)
            {
                int currenti = slider[0], currentj = slider[1];

                while (1 == 1)
                {

                    currenti += di;
                    currentj += dj;
                    int currHash = 17 * (currenti + (currentj) * dimension + 1);

                    if (currenti < 0 || currentj < 0 || currenti > dimension - 1 || currentj > dimension - 1)
                    {
                        currenti -= di;
                        currentj -= dj;
                        int oldHash = 17 * (slider[0] + (slider[1]) * dimension + 1);
                        currHash = 17 * (currenti + (currentj) * dimension + 1);
                        new_HashedSliders.Remove(oldHash);
                        new_HashedSliders.Add(currHash);
                        final_hash = final_hash * 31 + currHash;
                        int[] arr = { currenti, currentj };
                        newSliders.Add(arr);

                        if (currHash == TargetHash)
                        {
                            solved = true;
                        }

                        break;
                    }

                    if (hashed_obstacles.Contains(currHash) || new_HashedSliders.Contains(currHash))
                    {
                        currenti -= di;
                        currentj -= dj;
                        int oldHash = 17 * (slider[0] + (slider[1]) * dimension + 1);
                        currHash = 17 * (currenti + (currentj) * dimension + 1);

                        new_HashedSliders.Remove(oldHash);
                        new_HashedSliders.Add(currHash);
                        final_hash = final_hash * 31 + currHash;
                        int[] arr = { currenti, currentj };
                        newSliders.Add(arr);

                        if (currHash == TargetHash)
                        {
                            solved = true;
                        }
                        break;
                    }
                }
            }
            return new(new(solved, final_hash), new(new(new_HashedSliders), newSliders));
        }
    }
    public static List<TreeNode> GenerateChildren(TreeNode parent)
    {
        List<TreeNode> children = new();
        char[] directions = { 'U', 'D', 'R', 'L' };

        foreach (char character in directions)
        {
            if (!(character == parent.OppositeMove || character == parent.MoveDirection))
            {
                Tuple<Tuple<bool, long>, Tuple<HashSet<int>, HashSet<int[]>?>> val = Move(character, parent.State, parent.currState);
                List<char> path = new(parent.currentPath)
                {
                    character
                };
                TreeNode child = new(val.Item2.Item1, character, getOpositeDirection(character), val.Item1.Item1, val.Item1.Item2, path, val.Item2.Item2);
                children.Add(child);
            }
        }
        return children;
    }
    public static char getOpositeDirection(char direction)
    {
        if (direction == 'U')
            return 'D';
        if (direction == 'D')
            return 'U';
        if (direction == 'R')
            return 'L';
        return 'R';
    }


    public static List<char> BFS(HashSet<int> first_sliders, HashSet<int[]> currState)
    {
        Queue<TreeNode> queue = new();
        HashSet<long> visited = new();
        long final_hash = 17;
        TreeNode root;
        if (flag)
        {
            foreach (long slider in first_sliders)
            {
                final_hash = final_hash * 31 + slider;
            }
            root = new(first_sliders, '\0', '\0', false, final_hash, new('0'), currState);
        }
        else
        {
            root = new(first_sliders, '\0', '\0', false, 0, new('0'), currState);
        }
        queue.Enqueue(root);
        TreeNode currentNode;
        bool Solved = false;
        TreeNode? finalNode = null;
        List<char> path = new();
        while (queue.Count > 0 && !Solved)
        {
            currentNode = queue.Dequeue();
            List<TreeNode> children = GenerateChildren(currentNode);
            foreach (TreeNode child in children)
            {
                if (child.isTarget)
                {
                    Solved = true;
                    finalNode = child;
                    break;
                }
                long childHashCode = child.sliders;
                if (!visited.Contains(childHashCode))
                {
                    queue.Enqueue(child);
                    visited.Add(childHashCode);
                }
            }
        }
        if (Solved)
        {
            return finalNode.currentPath;
        }
        else
        {
            path.Add('0');
        }
        return path;
    }
    public static void restart()
    {
        TiltGame.obstacles = new();
        TiltGame.hashed_obstacles = new();
    }
}
class Program
{
    public static void Output_file(int fileIndex, List<char> path, HashSet<int> sliders, HashSet<int> obstacles, int d)
    {

        string filePath = "output\\" + "Case" + fileIndex + ".txt";

        if (path[0] == '0')
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Unsolvable");
                return;
            }
        }

        TiltGame.flag = true;
        List<HashSet<int>> states = new();
        states.Add(sliders);
        for (int i = 0; i < path.Count; i++)
        {
            var state = TiltGame.Move(path[i], states[i], null, true).Item2.Item1;
            states.Add(state);
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Solvable");
            writer.WriteLine("Min number of moves: " + path.Count);
            writer.Write("Sequence of moves : ");
            for (int p = 0; p < path.Count; p++)
            {
                switch (path[p])
                {
                    case 'L': writer.Write("left, "); break;
                    case 'R': writer.Write("right, "); break;
                    case 'U': writer.Write("up, "); break;
                    case 'D': writer.Write("down, "); break;
                }
            }

            writer.WriteLine();
            writer.WriteLine("Initial");
            int[] ptr = { 0, 0 };
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    ptr[0] = i;
                    ptr[1] = j;
                    int Hash = 17 * (ptr[0] + (ptr[1]) * d + 1);
                    if (sliders.Contains(Hash))
                    {
                        writer.Write("o, ");
                    }
                    else if (obstacles.Contains(Hash))
                    {
                        writer.Write("#, ");
                    }
                    else
                    {
                        writer.Write("., ");
                    }
                }
                writer.WriteLine();
            }

            for (int p = 0; p < path.Count; p++)
            {
                writer.WriteLine();
                var state = states[p + 1];
                switch (path[p])
                {
                    case 'L': writer.WriteLine("left"); break;
                    case 'R': writer.WriteLine("right"); break;
                    case 'U': writer.WriteLine("up"); break;
                    case 'D': writer.WriteLine("down"); break;
                }
                for (int i = 0; i < d; i++)
                {
                    for (int j = 0; j < d; j++)
                    {
                        ptr[0] = i;
                        ptr[1] = j;
                        int Hash = 17 * (ptr[0] + (ptr[1]) * d + 1);
                        if (state.Contains(Hash))
                        {
                            writer.Write("o, ");
                        }
                        else if (obstacles.Contains(Hash))
                        {
                            writer.Write("#, ");
                        }
                        else
                        {
                            writer.Write("., ");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }
    }
    public static void samples()
    {
        Console.WriteLine("Sample Cases START!!!");
        Console.WriteLine("---------------------");
        string normalPath = "D:\\University\\Algorithms !!!!!\\6 Project\\[4] Tilt Game\\Test Cases\\Sample Tests\\";
        for (int i = 1; i <= 6; i++)
        {
            Directory.CreateDirectory("output");
            string Stringpath = normalPath + "Case" + i + ".txt";
            Console.WriteLine("Case " + i + " : ");
            Tuple<HashSet<int>, HashSet<int[]>> currentMap = TiltGame.ReadFile(Stringpath);
            HashSet<int> sliders = currentMap.Item1;
            HashSet<int[]> currSliders = currentMap.Item2;

            Stopwatch bfsSw;
            bfsSw = Stopwatch.StartNew();
            List<char> path = TiltGame.BFS(sliders, currSliders);
            bfsSw.Stop();
            Console.Write("\t");
            string nice_moves;
            foreach (char charcter in path)
            {
                Console.Write(charcter + " ");
            }
            double bfsTimeInSeconds = bfsSw.ElapsedMilliseconds / 1000.0;
            Console.WriteLine();
            Console.WriteLine($"\tTime taken to execute BFS: {bfsTimeInSeconds} seconds");
            Console.Write("\n");

            Output_file(i, path, sliders, TiltGame.hashed_obstacles, TiltGame.dimension);

            TiltGame.restart();
        }
        Console.WriteLine("Sample Cases DONE!!!");
        Console.WriteLine();
        Console.WriteLine("=============================================================");
        Console.WriteLine();

    }

    static void Main(string[] args)
    {
        samples();
        string normalPath = "D:\\University\\Algorithms !!!!!\\6 Project\\[4] Tilt Game\\Test Cases\\Complete Tests\\";
        List<string> pathsSmall = new List<string>
        {
            normalPath + @"1 small\Case 1\Case1.txt",
            normalPath + @"1 small\Case 2\Case2.txt",
            normalPath + @"1 small\Case 3\Case3.txt"
        };

        List<string> pathsMed = new List<string>
        {
            normalPath + @"2 medium\Case 1\Case1.txt",
            normalPath + @"2 medium\Case 2\Case2.txt",
            normalPath + @"2 medium\Case 3\Case3.txt"
        };

        List<string> pathsLarge = new List<string>
        {
            normalPath + @"3 large\Case 1\Case1.txt",
            normalPath + @"3 large\Case 2\Case2.txt",
            normalPath + @"3 large\Case 3\Case6.txt"
        };
        int counter = 0;
        Console.WriteLine("Small Cases START!!!");
        Console.WriteLine("--------------------");
        foreach (string Stringpath in pathsSmall)
        {
            counter++;
            Console.WriteLine("Case " + counter + " : ");
            Tuple<HashSet<int>, HashSet<int[]>> currentMap = TiltGame.ReadFile(Stringpath);
            //Tuple<HashSet<int>, HashSet<int[]>> currentMap = TiltGame.ReadFile("D:\\University\\Algorithms !!!!!\\6 Project\\[4] Tilt Game\\Test Cases\\Sample Tests\\Case3.txt");
            HashSet<int> sliders = currentMap.Item1;
            HashSet<int[]> currSliders = currentMap.Item2;

            Stopwatch bfsSw;
            bfsSw = Stopwatch.StartNew();
            List<char> path = TiltGame.BFS(sliders, currSliders);
            bfsSw.Stop();
            Console.Write("\t");
            foreach (char charcter in path)
            {
                Console.Write(charcter + " ");
            }
            double bfsTimeInSeconds = bfsSw.ElapsedMilliseconds / 1000.0;
            Console.WriteLine();
            Console.WriteLine($"\tTime taken to execute BFS: {bfsTimeInSeconds} seconds");
            Console.Write("\n");
            TiltGame.restart();
        }
        counter = 0;
        Console.WriteLine("Small Cases DONE!!!");
        Console.WriteLine();
        Console.WriteLine("=============================================================");
        Console.WriteLine();
        Console.WriteLine("Medium Cases START!!!");
        Console.WriteLine("---------------------");
        foreach (string Stringpath in pathsMed)
        {
            counter++;
            Console.WriteLine("Case " + counter + " : ");
            Tuple<HashSet<int>, HashSet<int[]>> currentMap = TiltGame.ReadFile(Stringpath);
            //Tuple<HashSet<int>, HashSet<int[]>> currentMap = TiltGame.ReadFile("D:\\University\\Algorithms !!!!!\\6 Project\\[4] Tilt Game\\Test Cases\\Sample Tests\\Case3.txt");
            HashSet<int> sliders = currentMap.Item1;
            HashSet<int[]> currSliders = currentMap.Item2;

            Stopwatch bfsSw;
            bfsSw = Stopwatch.StartNew();
            List<char> path = TiltGame.BFS(sliders, currSliders);
            bfsSw.Stop();
            Console.Write("\t");
            foreach (char charcter in path)
            {
                Console.Write(charcter + " ");
            }
            double bfsTimeInSeconds = bfsSw.ElapsedMilliseconds / 1000.0;
            Console.WriteLine();
            Console.WriteLine($"\tTime taken to execute BFS: {bfsTimeInSeconds} seconds");
            Console.Write("\n");
            TiltGame.restart();
        }
        Console.WriteLine("Medium Cases DONE!!!");
        Console.WriteLine();
        Console.WriteLine("=============================================================");
        Console.WriteLine();
        Console.WriteLine("Large Cases START!!!");
        Console.WriteLine("--------------------");
        counter = 0;
        foreach (string Stringpath in pathsLarge)
        {
            counter++;
            Console.WriteLine("Case " + counter + " : ");
            Tuple<HashSet<int>, HashSet<int[]>> currentMap = TiltGame.ReadFile(Stringpath);
            //Tuple<HashSet<int>, HashSet<int[]>> currentMap = TiltGame.ReadFile("D:\\University\\Algorithms !!!!!\\6 Project\\[4] Tilt Game\\Test Cases\\Sample Tests\\Case3.txt");
            HashSet<int> sliders = currentMap.Item1;
            HashSet<int[]> currSliders = currentMap.Item2;

            Stopwatch bfsSw;
            bfsSw = Stopwatch.StartNew();
            List<char> path = TiltGame.BFS(sliders, currSliders);
            bfsSw.Stop();
            Console.Write("\t");
            foreach (char charcter in path)
            {
                Console.Write(charcter + " ");
            }
            double bfsTimeInSeconds = bfsSw.ElapsedMilliseconds / 1000.0;
            Console.WriteLine();
            Console.WriteLine($"\tTime taken to execute BFS: {bfsTimeInSeconds} seconds");
            Console.Write("\n");
            TiltGame.restart();
        }
        Console.WriteLine("Large Cases DONE!!!");
        Console.WriteLine();
        Console.WriteLine("=============================================================");

    }
}