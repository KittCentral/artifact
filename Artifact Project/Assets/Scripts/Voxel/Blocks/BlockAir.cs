using UnityEngine;
using System.Collections;
using System;

namespace Voxel
{
    [Serializable]
    public class BlockAir : Block
    {
        public BlockAir() : base()
        {

        }

        public override int TypeNum()
        {
            return -1;
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
