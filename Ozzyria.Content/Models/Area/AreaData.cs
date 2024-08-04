using Ozzyria.Content.Util;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Ozzyria.Content.Models.Area
{
    public class AreaData
    {
        public AreaMetaData AreaMetaData { get; set; }
        public TileData TileData { get; set; }
        public WallData WallData { get; set; }
        public PrefabData PrefabData { get; set; }

        public static string[] RetrieveAreaIds()
        {
            return Directory.EnumerateDirectories(GetRootDirectory())
                .Select(d => Path.GetFileNameWithoutExtension(d))
                .ToArray();
        }

        public static AreaData Retrieve(string areaId)
        {
            return new AreaData
            {
                AreaMetaData = RetrieveData<AreaMetaData>(areaId, "metadata"),
                TileData = RetrieveData<TileData>(areaId, "tiledata"),
                WallData = RetrieveData<WallData>(areaId, "walldata"),
                PrefabData = RetrieveData<PrefabData>(areaId, "prefabdata"),
            };
        }

        public void Store(string areaId)
        {
            if(AreaMetaData == null)
            {
                AreaMetaData = new AreaMetaData
                {
                    AreaId = areaId,
                    CreatedAt = DateTime.Now,
                };
            }

            AreaMetaData.UpdatedAt = DateTime.Now;
            StoreData(areaId, "metadata", AreaMetaData);

            if(TileData != null)
            {
                StoreData(areaId, "tiledata", TileData);
            }

            if (WallData != null)
            {
                StoreData(areaId, "walldata", WallData);
            }

            if(PrefabData != null)
            {
                StoreData(areaId, "prefabdata", PrefabData);
            }
        }


        protected static T RetrieveData<T>(string areaId, string dataKey)
        {
            var filePath = GetAreaDirectory(areaId) + "/" + dataKey + ".ozz";
            if (File.Exists(filePath))
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(filePath), JsonOptionsFactory.GetOptions());
            }

            return default(T);
        }

        protected static void StoreData<T>(string areaId, string dataKey, T data)
        {
            File.WriteAllText(GetAreaDirectory(areaId) + "/" + dataKey + ".ozz", JsonSerializer.Serialize(data, JsonOptionsFactory.GetOptions()));
        }

        public static bool Exists(string areaId)
        {
            return Directory.Exists(GetRootDirectory() + "/" + areaId);
        }

        protected static string GetAreaDirectory(string areaId)
        {
            var baseDirectory = GetRootDirectory() + "/" + areaId;
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }

            return baseDirectory;
        }

        protected static string GetRootDirectory()
        {
            var baseDirectory = Loader.Root() + "/Areas";
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }

            return baseDirectory;
        }
    }
}
