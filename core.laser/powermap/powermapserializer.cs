using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using QMC.Core;

namespace QMC.Core.Laser
{

    /// <summary>
    /// Json Serializer 바인더
    /// 외부 타입 변환용
    /// </summary>
    internal sealed class PowerMapSerializationBinder : SerializationBinder
    {
        public string TypeFormat { get; private set; }
        public PowerMapSerializationBinder(string typeFormat)
        {
            TypeFormat = typeFormat;
        }
        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
        public override Type BindToType(string assemblyName, string typeName)
        {
            //if (typeName.Equals("Vector2"))
            //{
            //    return typeof(System.Numerics.Vector2);
            //}
            //else
            if (typeName.Equals("Double[]"))
            {
                return typeof(double[]);
            }
            if (typeName.Equals("Single[]"))
            {
                return typeof(float[]);
            }
            if (typeName.Equals("Int[]"))
            {
                return typeof(int[]);
            }
            else
            {
                var resolvedTypeName = string.Format(TypeFormat, typeName);
                return Type.GetType(resolvedTypeName, true);
            }
        }
    }

    /// <summary>
    /// 파워 맵 serializer
    /// </summary>
    public class PowermapSerializer
    {
        public static IPowerMap Open(string fileName)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Binder = new PowerMapSerializationBinder("QMC.Core.Laser.{0}")
            };

            //Logger.Log(Logger.Module.Laser, Logger.Type.Info, $"trying to open powermap file: {fileName}");
            TextReader reader = null;
            try
            {
                reader = new StreamReader(fileName);
                var fileContents = reader.ReadToEnd();
                var job = JsonConvert.DeserializeObject<IPowerMap>(fileContents, settings);
                return job;
            }
            catch (Exception ex)
            {
                //Logger.Log(Logger.Module.Laser, ex);
            }
            finally
            {
                reader?.Close();
            }
            return null;
        }

        public static bool Save(IPowerMap map, string fileName)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Binder = new PowerMapSerializationBinder("QMC.Core.Laser.{0}")
            };
            //Logger.Log(Logger.Module.Laser, Logger.Type.Info, $"trying to save powermap file: {fileName}");
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(map, Formatting.Indented, settings);
                writer = new StreamWriter(fileName, false);
                writer.Write(contentsToWriteToFile);
            }
            catch (Exception ex)
            {
                //Logger.Log(Logger.Module.Laser, ex);
                return false;
            }
            finally
            {
                writer?.Close();
            }
            return true;
        }
    }
}
