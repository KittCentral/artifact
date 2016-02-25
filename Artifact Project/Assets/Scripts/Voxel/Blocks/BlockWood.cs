using UnityEngine;
using System.Collections;
using System;

namespace Voxel
{
    public class BlockWood : Block
    {
        public BlockWood() : base()
        {

        }

        public override int TypeNum()
        {
            return 2;
        }

        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            switch (direction)
            {
                case Direction.up:
                    tile.x = 2;
                    tile.y = 1;
                    return tile;
                case Direction.down:
                    tile.x = 2;
                    tile.y = 1;
                    return tile;
                default:
                    tile.x = 1;
                    tile.y = 1;
                    return tile;
            }
        }
    }
}
