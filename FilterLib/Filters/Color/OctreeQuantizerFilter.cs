using FilterLib.Reporting;
using FilterLib.Util;
using System.Collections.Generic;
using System.Linq;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Reduce colors (quantize) to a palette of given size, using an octree (a tree
    /// with 8 children per node). Colors are grouped by their bits (per channel)
    /// starting from the most significant to the least significant. Each color gets
    /// a leaf in the tree and then nodes are merged bottom-up until the desired
    /// number is reached.
    /// </summary>
    [Filter]
    public sealed class OctreeQuantizerFilter : FilterInPlaceBase
    {
        private int levels;
        
        /// <summary>
        /// Number of colors in the resulting palette.
        /// </summary>
        [FilterParam]
        [FilterParamMin(1)]
        [FilterParamMax(256 * 256 * 256)]
        public int Levels
        {
            get { return levels; }
            set { levels = value.Clamp(1, 256 * 256 * 256); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of colors in the resulting palette</param>
        public OctreeQuantizerFilter(int levels = 256) => Levels = levels;

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            Octree tree = new();
            fixed (byte* start = image)
            {
                // Step 1: fill tree with colors
                byte* ptr = start;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        tree.AddColor(ptr[0], ptr[1], ptr[2]);
                        ptr += 3;
                    }
                    reporter?.Report(y + 1, 0, 2 * image.Height);
                }
                // Step 2: build reduced color palette
                tree.Reduce(Levels);
                // Step 3: replace each color with representative from palette
                ptr = start;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        (ptr[0], ptr[1], ptr[2]) = tree.GetPaletteColor(ptr[0], ptr[1], ptr[2]);
                        ptr += 3;
                    }
                    reporter?.Report(image.Height + y + 1, 0, 2 * image.Height);
                }
            }
            reporter?.Done();
        }

        // This should be equal to the number of bits per channel,
        // or less for faster, but approximate results
        private const int MAX_DEPTH = 8;

        // A node in the tree
        private sealed class Node
        {
            // Sum of R, G, B components (0 for non-leaf nodes)
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }

            // Number of pixels having this color
            public int PixelCount { get; set; }

            // Child nodes (can be null)
            public Node[] Children { get; set; }

            // Number of total pixels in the subtree, used for sorting
            // colors based on importance
            public int TotalPixelCount { get; set; }

            // Initialize a new node
            public Node(int level, Octree tree)
            {
                R = G = B = 0;
                PixelCount = 0;
                TotalPixelCount = 0;
                Children = new Node[8];
                tree.AddNode(level, this);
            }

            // Is the node a leaf
            public bool IsLeaf { get { return PixelCount > 0; } }

            // Get all leaf nodes in the subtree
            public IEnumerable<Node> GetLeaves()
            {
                if (IsLeaf) return new[] { this };
                else return Children.Where(c => c != null).SelectMany(c => c.GetLeaves());
            }

            // Add a color into this subtree, descending down to the proper path
            // and creating intermediate nodes as needed
            public void AddColor(byte r, byte g, byte b, int level, Octree tree)
            {
                TotalPixelCount++;
                // We reached a leaf, add color to current node
                if (level >= MAX_DEPTH)
                {
                    R += r;
                    G += g;
                    B += b;
                    PixelCount++;
                }
                // Non-leaf, recurse in one of the child nodes (creating a new if needed)
                else
                {
                    int index = GetIndex(r, g, b, level);
                    System.Diagnostics.Debug.Assert(0 <= index && index < Children.Length);
                    if (Children[index] == null) Children[index] = new Node(level, tree);
                    Children[index].AddColor(r, g, b, level + 1, tree);
                }
            }

            // Get the color in the final palette color by traversing the tree and finding
            // the leaf node corresponding to the given original color
            public (byte, byte, byte) GetPaletteColor(int r, int g, int b, int level)
            {
                // Leaf found
                if (IsLeaf) return GetAvgColor();
                // Otherwise recurse
                int index = GetIndex(r, g, b, level);
                System.Diagnostics.Debug.Assert(Children[index] != null);
                return Children[index].GetPaletteColor(r, g, b, level + 1);
            }

            // Merge child nodes into the node (adding their colors
            // and pixel counts). Not recursive, should only be called
            // in a bottom up fashion (i.e. the children should be leaves).
            // Returns how many nodes disappeared.
            public int MergeChildren()
            {
                // Nothing to do with a leaf
                if (IsLeaf) return 0;
                // Non-leaf, merge all existing children
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

            // Get the index of the color at a given level based on its components.
            // From each component, the level determines which bit to take (from
            // the most significant to the least. These 3 bits are then joined
            // together to form the index (0 to 7).
            public static int GetIndex(int r, int g, int b, int level)
            {
                int idx = 0;
                int mask = 0x80 >> level;
                if ((r & mask) != 0) idx |= 4;
                if ((g & mask) != 0) idx |= 2;
                if ((b & mask) != 0) idx |= 1;
                return idx;
            }

            // Get the average color corresponding to this node: without reduction
            // in the palette this is the exact same color. With reduction, it's the
            // average of the merged subtree that was below this node.
            public (byte, byte, byte) GetAvgColor() =>
                ((R / PixelCount).ClampToByte(), (G / PixelCount).ClampToByte(), (B / PixelCount).ClampToByte());

        }

        // Represents the whole tree
        private sealed class Octree
        {
            // Helper list to keep track of nodes at each level, except the last
            private readonly List<Node>[] levels;
            // Root node which is at level -1
            private readonly Node root;
            // Has the tree been already reduced
            private bool reduced;

            public Octree()
            {
                levels = new List<Node>[MAX_DEPTH];
                for (int i = 0; i < levels.Length; ++i) levels[i] = new();
                root = new Node(-1, this);
                reduced = false;
            }

            // Get all leaves, after compression, this list corresponds to the
            // resulting palette
            public IEnumerable<Node> GetLeaves() => root.GetLeaves();

            // Add a node in the tree, we ignore the last level (leaves are not interesting)
            // but the root node at level -1 is needed so we shift the index by 1.
            public void AddNode(int level, Node node)
            {
                if (level < MAX_DEPTH - 1)
                    levels[level + 1].Add(node);
            }

            // Add a color recursively to the tree
            public void AddColor(byte r, byte g, byte b)
            {
                if (reduced) throw new System.InvalidOperationException("Octree has already been reduced.");
                root.AddColor(r, g, b, 0, this);
            }

            // Make the reduced palette by merging nodes bottom up until the limit is reached
            public void Reduce(int max)
            {
                // Get where we start from
                int leafCount = GetLeaves().Count();
                // Go bottom-up, starting from the first level containing inner nodes
                for (int level = MAX_DEPTH - 1; level >= 0 && leafCount > max; --level)
                {
                    // If we are far enough from the limit, all nodes in this level will be merged
                    // so no need to sort them. Otherwise sort by importance (total pixel count).
                    if (leafCount <= max + levels[level].Count * 7)
                        levels[level].Sort((n1, n2) => n1.TotalPixelCount.CompareTo(n2.TotalPixelCount));
                    // Do the merge until hitting the limit
                    for (int i = 0; i < levels[level].Count && leafCount > max; ++i)
                    {
                        leafCount -= levels[level][i].MergeChildren();
                    }
                }

                System.Diagnostics.Debug.Assert(GetLeaves().Count() == leafCount);
                System.Diagnostics.Debug.Assert(leafCount <= max);
                reduced = true;
            }

            // For a given color, get the representative color in the final palette
            public (byte, byte, byte) GetPaletteColor(byte r, byte g, byte b) => root.GetPaletteColor(r, g, b, 0);
        }
    }
}
