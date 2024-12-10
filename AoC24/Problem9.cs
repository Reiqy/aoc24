namespace AoC24;

public class Problem9
{
    public long SolveA()
    {
        var input = File.ReadAllText("input/aoc24_9.txt").Trim();
        var filesystemBuilder = new List<int>();

        int fileIndex = 0;
        for (int i = 0; i < input.Length; i += 2)
        {
            var fileLength = input[i] - '0';

            var freeSpaceLength = 0;
            if ((i + 1) < input.Length)
            {
                freeSpaceLength = input[i + 1] - '0';
            }

            filesystemBuilder.AddRange(Enumerable.Repeat(fileIndex, fileLength).Concat(Enumerable.Repeat(-1, freeSpaceLength)));
            fileIndex++;
        }

        var filesystem = filesystemBuilder.ToArray();

        var leftIndex = 0;
        var rightIndex = filesystem.Length - 1;
        while (leftIndex < rightIndex)
        {
            if (filesystem[leftIndex] >= 0)
            {
                leftIndex++;
                continue;
            }

            if (filesystem[rightIndex] < 0)
            {
                rightIndex--;
                continue;
            }

            filesystem[leftIndex] = filesystem[rightIndex];
            filesystem[rightIndex] = -1;
            rightIndex--;
        }

        return this.ComputeChecksum(filesystem);
    }

    public long SolveB()
    {
        var input = File.ReadAllText("input/aoc24_9.txt").Trim();
        var filesystem = new List<Block>();

        int fileIndex = 0;
        for (int i = 0; i < input.Length; i += 2)
        {
            var fileLength = input[i] - '0';

            filesystem.Add(new Block(fileIndex, fileLength));

            if ((i + 1) < input.Length)
            {
                var freeSpaceLength = input[i + 1] - '0';
                filesystem.Add(new Block(-1, freeSpaceLength));
            }

            fileIndex++;
        }

        int allowedFileIndex = fileIndex - 1;
        for (int i = filesystem.Count - 1; i >= 0; i--)
        {
            var movedFileIndex = filesystem[i].Index;
            if (movedFileIndex < 0 || movedFileIndex > allowedFileIndex)
            {
                continue;
            }

            for (int j = 0; j < i; j++)
            {
                if (filesystem[j].Index >= 0)
                {
                    continue;
                }

                if (filesystem[j].Size < filesystem[i].Size)
                {
                    continue;
                }

                allowedFileIndex = movedFileIndex - 1;
                var movedFileSize = filesystem[i].Size;
                var remainingSpace = filesystem[j].Size - movedFileSize;
                filesystem.RemoveAt(i);
                if (i < filesystem.Count && filesystem[i].Index < 0)
                {
                    var iSize = filesystem[i].Size;
                    filesystem.RemoveAt(i);
                    filesystem.Insert(i, new Block(-1, iSize + movedFileSize));
                }
                else
                {
                    filesystem.Insert(i, new Block(-1, movedFileSize));
                }

                filesystem.RemoveAt(j);
                filesystem.Insert(j, new Block(movedFileIndex, movedFileSize));

                if (remainingSpace > 0)
                {
                    if (filesystem[j + 1].Index < 0)
                    {
                        var nextSpace = filesystem[j + 1].Size;
                        filesystem.RemoveAt(j + 1);
                        filesystem.Insert(j + 1, new Block(-1, nextSpace + remainingSpace));
                    }
                    else
                    {
                        filesystem.Insert(j + 1, new Block(-1, remainingSpace));
                        i++;
                    }
                }

                break;
            }
        }

        var filesystemActualBuilder = new List<int>();
        foreach (var block in filesystem)
        {
            filesystemActualBuilder.AddRange(Enumerable.Repeat(block.Index, block.Size));
        }

        return this.ComputeChecksum(filesystemActualBuilder.ToArray());
    }

    private record Block(int Index, int Size);

    private long ComputeChecksum(int[] filesystem)
    {
        long checksum = 0L;
        for (int i = 0; i < filesystem.Length; i++)
        {
            if (filesystem[i] < 0)
            {
                continue;
            }

            checksum += i * filesystem[i];
        }

        return checksum;
    }
}
