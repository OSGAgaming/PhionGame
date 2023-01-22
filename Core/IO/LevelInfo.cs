using QueefCord.Content.UI;
using QueefCord.Core.Graphics;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Scenes;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using JsonNet.ContractResolvers;
using System.Runtime.CompilerServices;
using QueefCord.Core.Tiles;

namespace QueefCord.Core.IO
{
    public class ExcludeCalculatedResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            return props.Where(p => p.Writable).ToList();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = (m) => ShouldSerialize(member);

            return property;
        }

        internal static bool ShouldSerialize(MemberInfo memberInfo)
        {
            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo == null)
            {
                return true;
            }

            if (propertyInfo.SetMethod != null)
            {
                return true;
            }

            var getMethod = propertyInfo.GetMethod;
            return Attribute.GetCustomAttribute(getMethod, typeof(CompilerGeneratedAttribute)) != null;
        }
    }
    public class LevelInfo : ISerializable
    {
        public Dictionary<string, Layer> layers = new Dictionary<string, Layer>();
        public Scene scene { get; set; }

        public LevelInfo(Scene scene, Dictionary<string, Layer> layers)
        {
            this.scene = scene;
            this.layers = layers;
        }

        public LevelInfo() { }

        public static void SaveCurrentLevel(string path)
        {
            LevelInfo buffer = Activator.CreateInstance(typeof(LevelInfo)) as LevelInfo;
            Stream writeStream = File.OpenWrite(Utils.LocalWorldPath + path + ".mgsc");

            buffer.Write(new BinaryWriter(writeStream));
            //Debug.Write(jsonString);

        }

        public static void SaveCurrentLevelToJSON(string path)
        {
            string jsonString = JsonConvert.SerializeObject(
            SceneHolder.CurrentScene,
            new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new ExcludeCalculatedResolver()
            });

            File.WriteAllText(Utils.LocalWorldPath + path + ".mgsc", jsonString);
        }

        public static void LoadLevelFromJSON(string path)
        {
            Scene scene = JsonConvert.DeserializeObject<Scene>(File.ReadAllText(Utils.LocalWorldPath + path + ".mgsc"), new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new ExcludeCalculatedResolver()
            });

            IComponent[] components = new IComponent[scene.DistinctElements.Count];

            scene.DistinctElements.CopyTo(components, 0);

            scene.DistinctElements.Clear();

            foreach (IComponent component in components.ToArray())
                scene.AddEntity(component);

            scene.AddEntity(UIScreenManager.Instance);

            SceneHolder.StartScene(scene);
        }

        public static void LoadLevel(string path)
        {
            LevelInfo buffer = Activator.CreateInstance(typeof(LevelInfo)) as LevelInfo;
            Stream readStream = File.OpenRead(Utils.WorldPath + path + ".mgsc");

            buffer = buffer.Read(new BinaryReader(readStream)) as LevelInfo;

            SceneHolder.CurrentScene = buffer.scene;
            LayerHost.layers = buffer.layers;
        }

        public IComponent Read(BinaryReader br)
        {
            Scene scene = ContentWriter.Load<Scene>(br);
            Dictionary<string, Layer> layers = new Dictionary<string, Layer>();
            //TODO:Not hardcode
            scene.AddEntity(UIScreenManager.Instance);

            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Layer layer = ContentWriter.Load<Layer>(br);

                layers.Add(layer.ID, layer);
            }

            br.Close();

            return new LevelInfo(scene, layers);
        }

        public void Write(BinaryWriter bw)
        {
            SceneHolder.CurrentScene.Write(bw);
            Dictionary<string, Layer> layerDict = LayerHost.layers;

            bw.Write(layerDict.Count);
            foreach (KeyValuePair<string, Layer> layer in layerDict)
            {
                layer.Value.Write(bw);
            }

            bw.Close();
        }
    }
}