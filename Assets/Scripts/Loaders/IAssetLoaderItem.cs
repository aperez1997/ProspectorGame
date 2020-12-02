
/// <summary>
/// Asset that can be used with AssetLoader. Must have a unique key
/// </summary>
/// <see cref="AssetLoader{Type}"/>
public interface IAssetLoaderItem
{
    /// <summary>
    /// Get the ID for the item
    /// </summary>
    string GetKey();
}