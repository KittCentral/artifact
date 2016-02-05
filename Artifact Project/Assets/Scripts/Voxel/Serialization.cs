using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Voxel
{
    public static class Serialization
    {
        public static string saveFolderName = "voxelGameSaves";

        public static string SaveLocation(string worldName)
        {
            string saveLocation = saveFolderName + "/" + worldName + "/";
            if (!Directory.Exists(saveLocation))
                Directory.CreateDirectory(saveLocation);
            return saveLocation;
        }

        public static string FileName(WorldPos posC)
        {
            string fileName = posC.x + "," + posC.y + "," + posC.z + ".bin";
            return fileName;
        }
        
        public static void SaveChunk(Chunk chunk)
        {
            Save save = new Save(chunk);
            if (save.blocks.Count == 0)
                return;
            string saveFile = SaveLocation(chunk.world.name);
            saveFile += FileName(chunk.posC);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, save);
            stream.Close();
        }

        public static bool Load(Chunk chunk)
        {
            string saveFile = SaveLocation(chunk.world.name);
            saveFile += FileName(chunk.posC);
            if (!File.Exists(saveFile))
                return false;
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFile, FileMode.Open);
            Save save = (Save)formatter.Deserialize(stream);
            foreach(var block in save.blocks)
            {
                chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
            }
            stream.Close();
            return true;
        }
    }
}
