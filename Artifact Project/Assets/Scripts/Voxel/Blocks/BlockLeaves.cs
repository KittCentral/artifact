using UnityEngine;
using System.Collections;
using System;

namespace Voxel
{
    [Serializable]
    public class BlockLeaves : Block
    {
        public BlockLeaves() : base()
        {

        }

        public override int TypeNum()
        {
            return 3;
        }

        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 0;
            tile.y = 1;
            return tile;
        }

        public override bool IsSolid(Direction direction)
        {
            return true;
        }
    }
}
