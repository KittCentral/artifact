using UnityEngine;
using System.Collections;

namespace Voxel
{
    public class BlockAir : Block
    {
        public BlockAir() : base()
        {

        }

        public override MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            return meshData;
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }
    }
}
