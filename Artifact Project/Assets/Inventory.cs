using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Voxel
{
    public class Inventory : MonoBehaviour
    {
        public Sprite[] blockThumbs;
        public InventoryItem[] items = new InventoryItem[10];
        public Dictionary<int, int> IDs = new Dictionary<int, int>(); //position, ID

        public void AddItemButton(int position, InventoryItem item)
        {
            items[position] = item;
        }

        public Block GetBlock(int ID)
        {
            int block = transform.GetChild(ID).gameObject.GetComponent<InventoryItem>().BlockID;
            switch (block)
            {
                case 0:
                    return new BlockGrass();
                case 1:
                    return new BlockStone();
                case 2:
                    return new BlockWood();
                case 3:
                    return new BlockLeaves();
                default:
                    return null;
            }
        }

        public void AddItem(int ID)
        {
            int position = 0;
            if (IDs.ContainsValue(ID))
            {
                position = IDs.FirstOrDefault(x => x.Value == ID).Key;
                items[position].Quantity++;
                return;
            }
            else
            {
                for (int i = 0; i < items.GetLength(0); i++)
                {
                    if (items[i].BlockID == -1)
                    {
                        items[i].BlockID = ID;
                        items[i].Quantity++;
                        IDs.Add(IDs.Count,ID);
                        return;
                    }
                }
            }
        }

        public bool RemoveItem(int position, out int blockID)
        {
            blockID = -1;
            for (int i = 0; i < items.GetLength(0); i++)
            {
                if (IDs.TryGetValue(position, out blockID))
                {
                    if (blockID != -1)
                    {
                        items[position].Quantity--;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
