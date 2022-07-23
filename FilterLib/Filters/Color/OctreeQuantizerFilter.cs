using FilterLib.Reporting;
using FilterLib.Util;
using System.Collections.Generic;
using System.Linq;

namespace FilterLib.Filters.Color
{
    [Filter]
    public sealed class OctreeQuantizerFilter : FilterInPlaceBase
    {
        private int levels;

        [FilterParam]
        [FilterParamMin(1)]
        [FilterParamMax(256 * 256 * 256)]
        public int Levels
        {
            get { return levels; }
            set { levels = value.Clamp(1, 256 * 256 * 256); }
        }

        public OctreeQuantizerFilter(int levels = 256) => Levels = levels;

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            Octree tree = new();
            fixed (byte* start = image)
            {
                byte* ptr = start;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        tree.AddColor(ptr[0], ptr[1], ptr[2]);
                        ptr += 3;
                    }
                }
                List<(byte, byte, byte)> palette = tree.MakePalette(Levels);
                ptr = start;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        int idx = tree.GetPaletteIndex(ptr[0], ptr[1], ptr[2]);
                        (ptr[0], ptr[1], ptr[2]) = palette[idx];
                        ptr += 3;
                    }
                }
            }
        }

        private const int MAX_DEPTH = 8;

        private sealed class Node
        {
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            public int PixelCount { get; set; }
            public int PaletteIdx { get; set; }
            public Node[] Children { get; set; }
            public int TotalPixelCount { get; set; }

            public Node(int level, Octree tree)
            {
                R = G = B = 0;
                PixelCount = 0;
                TotalPixelCount = 0;
                PaletteIdx = -1;
                Children = new Node[8];
                tree.AddNode(level, this);
            }

            public bool IsLeaf { get { return PixelCount > 0; } }

            public IEnumerable<Node> GetLeaves()
            {
                if (IsLeaf) return new[] { this };
                else return Children.Where(c => c != null).SelectMany(c => c.GetLeaves());
            }

            public void AddColor(byte r, byte g, byte b, int level, Octree tree)
            {
                TotalPixelCount++;
                if (level >= MAX_DEPTH)
                {
                    R += r;
                    G += g;
                    B += b;
                    PixelCount++;
                }
                else
                {
                    int index = GetIndex(r, g, b, level);
                    System.Diagnostics.Debug.Assert(0 <= index && index < Children.Length);
                    if (Children[index] == null) Children[index] = new Node(level, tree);
                    Children[index].AddColor(r, g, b, level + 1, tree);
                }
            }

            public int GetPaletteIndex(int r, int g, int b, int level)
            {
                if (IsLeaf) return PaletteIdx;
                int index = GetIndex(r, g, b, level);
                System.Diagnostics.Debug.Assert(Children[index] != null);
                return Children[index].GetPaletteIndex(r, g, b, level + 1);
            }

            public int MergeChildren()
            {
                if (IsLeaf) return 0;
                int n = 0;
                for (int i = 0; i < Children.Length; i++)
                {
                    if (Children[i] != null)
                    {
                        R += Children[i].R;
                        G += Children[i].G;
                        B += Children[i].B;
                        PixelCount += Children[i].PixelCount;
                        ++n;
                        Children[i] = null;
                    }
                }
                return n - 1; // -1 because the current node becomes a leaf
            }

            public static int GetIndex(int r, int g, int b, int level)
            {
                int idx = 0;
                int mask = 0x80 >> level;
                if ((r & mask) != 0) idx |= 4;
                if ((g & mask) != 0) idx |= 2;
                if ((b & mask) != 0) idx |= 1;
                return idx;
            }

            public (byte, byte, byte) GetAvgColor() =>
                ((R / PixelCount).ClampToByte(), (G / PixelCount).ClampToByte(), (B / PixelCount).ClampToByte());
            
        }

        private sealed class Octree
        {
            private List<Node>[] levels;
            private Node root;

            public Octree()
            {
                levels = new List<Node>[MAX_DEPTH];
                for (int i = 0; i < levels.Length; ++i) levels[i] = new();
                root = new Node(-1, this);
            }

            public IEnumerable<Node> GetLeaves() =>  root.GetLeaves();

            public void AddNode(int level, Node node)
            {
                if (level < MAX_DEPTH - 1)
                    levels[level + 1].Add(node);
            }

            public void AddColor(byte r, byte g, byte b) => root.AddColor(r, g, b, 0, this);

            public List<(byte, byte, byte)> MakePalette(int max)
            {
                int leafCount = GetLeaves().Count();
                for (int level = MAX_DEPTH - 1; level >= 0 && leafCount > max; --level)
                {
                    if (leafCount <= max + levels[level].Count * 7)
                        levels[level].Sort((n1, n2) => n1.TotalPixelCount.CompareTo(n2.TotalPixelCount));
                    for (int i = 0; i < levels[level].Count && leafCount > max; ++i)
                    {
                        leafCount -= levels[level][i].MergeChildren();
                    }
                }

                System.Diagnostics.Debug.Assert(GetLeaves().Count() == leafCount);
                System.Diagnostics.Debug.Assert(leafCount <= max);

                List<(byte, byte, byte)> palette = new();
                int paletteIdx = 0;
                foreach (Node n in GetLeaves())
                {
                    palette.Add(n.GetAvgColor());
                    n.PaletteIdx = paletteIdx++;
                }
                return palette;
            }

            public int GetPaletteIndex(byte r, byte g, byte b) => root.GetPaletteIndex(r, g, b, 0);
        }
    }
}
