using UnityEngine;
using System.Collections;
using System;

namespace Voxel
{
    [Serializable]
    public class BlockStone : Block
    {
        public BlockStone() : base()
        {

        }

        public override int TypeNum()
        {
            return 1;
        }
    }
}
