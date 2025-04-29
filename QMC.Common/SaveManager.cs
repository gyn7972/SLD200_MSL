using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class SaveManager
    {
        private static bool VerifyFile(string strPath)
        {
            FileInfo fi = new FileInfo(strPath);
            return fi.Exists;
        }

        private static void CreateFileDirectory(string strFilePath)
        {
            int nLastIndex = strFilePath.LastIndexOf("\\");
            string strPath = strFilePath.Substring(0, nLastIndex);
            if(!string.IsNullOrEmpty(strPath))
                Directory.CreateDirectory(strPath);
            
        }
        public static void Load<T>(string strFilePath, out T t)
        {
            if (VerifyFile(strFilePath))
            {
                using (FileStream fs = new FileStream(strFilePath, FileMode.Open))
                {
                    SaveManager.BinaryDeserialize<T>(fs, out t);
                }
            }
            else
            {
                t = default(T);
            }
        }

        public static void Save(string strFilePath, object t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinarySerialize(ms, t);

                FileInfo fi = new FileInfo(strFilePath);
                if (!fi.Exists)
                {
                    CreateFileDirectory(strFilePath);
                }

                using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate))
                {
                    ms.WriteTo(fs);
                }
            }
        }

        public static void Save(string strFilePath, string strBackupFilePath, object t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinarySerialize(ms, t);

                FileInfo fi = new FileInfo(strFilePath);
                if (fi.Exists)
                {
                    CreateFileDirectory(strBackupFilePath);
                    fi.MoveTo(strBackupFilePath);
                }
                else
                {
                    CreateFileDirectory(strFilePath);
                }
                

                using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate))
                {
                    ms.WriteTo(fs);
                }
            }
        }

        public static void LoadRecipe(out RecipeInfoCollection recipes)
        {
            RecipeInfoCollection loadRecipes = new RecipeInfoCollection();
            RecipeHeader header = loadRecipes.Header;
            Load<RecipeHeader>(ConfigManager.GetRecipeFilePath(), out header);
            if(header == null)
            {
                header = new RecipeHeader();
            }

            loadRecipes.Header = header;
            loadRecipes.Clear();
            foreach (string name in header.RecipeList)
            {
                RecipeInfo recipe;
                Load<RecipeInfo>(ConfigManager.GetRecipeFilePath(name), out recipe);
                loadRecipes.Add(recipe);
            }

            recipes = loadRecipes;
        }
        public static void SaveRecipe(RecipeInfoCollection recipes)
        {
            string strBackupPath = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            Save(ConfigManager.GetRecipeFilePath(), ConfigManager.GetRecipeBackupFilePath(strBackupPath), recipes.Header);
            foreach(RecipeInfo recipe in recipes)
            {
                Save(ConfigManager.GetRecipeFilePath(recipe.Name), ConfigManager.GetRecipeBackupFilePath(recipe.Name, strBackupPath), recipe);
            }
        }
        public static int BinarySerialize(Stream stream, object t)
        {
            int ret = 0;
            try
            {
                if (stream != null)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(stream, t);
                }
                else
                {
                    ret = -1;
                }
            }catch(Exception ex)
            {
                Log.Write(ex);
            }
            
            
            return ret;
        }

        public static int BinaryDeserialize<T>(Stream stream, out T t)
        {
            int ret = 0;

            if(stream != null)
            {
                BinaryFormatter bf = new BinaryFormatter();
                t = (T)bf.Deserialize(stream);
            }
            else
            {
                ret = -1;
                t = default(T);
            }


            return ret;
        }

        public static void BinarySerialize(ref byte[] bytes, object graph)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                BinarySerialize(memoryStream, graph);
                bytes = memoryStream.ToArray();
            }
            finally
            {
                if(memoryStream != null)
                    ((IDisposable)memoryStream).Dispose();
            }
        }

        public static int BinaryDeserialize<T>(byte[] bytes, out T t)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);
            try
            {
                return BinaryDeserialize<T>(memoryStream, out t);
            }
            finally
            {
                if (memoryStream != null)
                {
                   ((IDisposable)memoryStream).Dispose();
                }
            }
        }


    }
}
