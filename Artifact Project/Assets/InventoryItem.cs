using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Voxel
{
    public class InventoryItem : MonoBehaviour
    {
        public int position;
        int blockID;
        int quantity;
        Inventory inventory;
        public bool isEmpty;

        void Start()
        {
            blockID = -1;
            inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
            inventory.AddItemButton(position, this);
        }

        public int BlockID
        {
            get
            {
                return blockID;
            }

            set
            {
                blockID = value;
                if(value != -1)
                    transform.GetChild(1).GetComponent<Image>().sprite = inventory.blockThumbs[blockID];
                transform.GetChild(1).gameObject.SetActive(blockID != -1);
            }
        }

        public int Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                if (value == 0)
                {
                    transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
                    isEmpty = true;
                    BlockID = -1;
                    inventory.IDs.Remove(position);
                }
                else
                {
                    transform.GetChild(0).gameObject.GetComponent<Text>().text = quantity.ToString();
                    isEmpty = false;
                }
            }
        }
    }
}