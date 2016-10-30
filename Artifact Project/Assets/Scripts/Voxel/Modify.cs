using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Voxel
{
    public class Modify : MonoBehaviour
    {
        Vector2 rot;
        public float speed;
        public float jumpHeight;

        Rigidbody body;
        Inventory inventory;
        bool isGrounded;
        int currentSlot;

        void Start()
        {
            body = GetComponent<Rigidbody>();
            inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            Movement();
            Rotation();
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                Jump();
            if (Input.GetButtonDown("Fire1"))
                DestroyBlock();
            if (Input.GetButtonDown("Fire2"))
                BuildBlock();
            if (Input.GetKeyDown(KeyCode.Escape))
                Cursor.lockState = CursorLockMode.None;
            InventoryChangeCheck();

            RaycastHit groundHit;
            isGrounded = Physics.Raycast(transform.position, -transform.up, out groundHit, 1.1f);
        }

        void Jump()
        {
            body.AddForce(transform.up * jumpHeight);
        }

        void Movement()
        {
            body.AddForce(transform.forward * Input.GetAxis("Vertical") * speed);
            body.AddForce(transform.right * Input.GetAxis("Horizontal") * speed);
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
                body.AddForce(new Vector3(body.velocity.x, 0f, body.velocity.z) * -1 * speed);
        }

        void Rotation()
        {
            rot = new Vector2(rot.x + Input.GetAxis("Mouse X") * 3, rot.y + Input.GetAxis("Mouse Y") * 3);
            rot.y = rot.y <= -90 ? -90 : rot.y;
            rot.y = rot.y >= 90 ? 90 : rot.y;
            transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
            transform.GetChild(0).localRotation = Quaternion.AngleAxis(rot.y, Vector3.left);
        }

        void DestroyBlock()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward, out hit, 10))
            {
                if(EditTerrain.GetBlock(hit).TypeNum() != -1)
                    inventory.AddItem(EditTerrain.GetBlock(hit).TypeNum());
                EditTerrain.SetBlock(hit, new BlockAir());
            }
        }

        void BuildBlock()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward, out hit, 10))
            {
                int blockID;
                if (inventory.RemoveItem(currentSlot, out blockID))
                {
                    switch (blockID)
                    {
                        case 0:
                            EditTerrain.SetBlock(hit, new BlockGrass(), true);
                            break;
                        case 1:
                            EditTerrain.SetBlock(hit, new BlockStone(), true);
                            break;
                        case 2:
                            EditTerrain.SetBlock(hit, new BlockWood(), true);
                            break;
                        case 3:
                            EditTerrain.SetBlock(hit, new BlockLeaves(), true);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void InventoryChangeCheck()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Keypad1))
                currentSlot = 0;
            if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Keypad2))
                currentSlot = 1;
            if (Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Keypad3))
                currentSlot = 2;
            if (Input.GetKeyUp(KeyCode.Alpha4) || Input.GetKeyUp(KeyCode.Keypad4))
                currentSlot = 3;
            if (Input.GetKeyUp(KeyCode.Alpha5) || Input.GetKeyUp(KeyCode.Keypad5))
                currentSlot = 4;
            if (Input.GetKeyUp(KeyCode.Alpha6) || Input.GetKeyUp(KeyCode.Keypad6))
                currentSlot = 5;
            if (Input.GetKeyUp(KeyCode.Alpha7) || Input.GetKeyUp(KeyCode.Keypad7))
                currentSlot = 6;
            if (Input.GetKeyUp(KeyCode.Alpha8) || Input.GetKeyUp(KeyCode.Keypad8))
                currentSlot = 7;
            if (Input.GetKeyUp(KeyCode.Alpha9) || Input.GetKeyUp(KeyCode.Keypad9))
                currentSlot = 8;
            if (Input.GetKeyUp(KeyCode.Alpha0) || Input.GetKeyUp(KeyCode.Keypad0))
                currentSlot = 9;

            for (int i = 0; i < 10; i++)
            {
                if (i == currentSlot)
                    inventory.transform.GetChild(i).GetComponent<Image>().color = Color.red;
                else
                    inventory.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
        }
    }
}
