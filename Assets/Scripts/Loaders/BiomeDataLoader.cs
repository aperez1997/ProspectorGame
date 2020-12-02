public class BiomeDataLoader : AssetLoader<BiomeData>
{
    private static BiomeDataLoader instance = null;
    private static readonly object padlock = new object();

    public static BiomeDataLoader Instance {
        get {
            lock (padlock) {
                if (instance == null){ instance = new BiomeDataLoader(); }
                return instance;
            }
        }
    }

    public static BiomeData LoadBiomeDataByType(BiomeType type)
    {
        return Instance.LoadByKey(type.ToString());
    }
}
