namespace Kloud21.DataModel
{
    public interface IIdentityKey<TKey>
    {
        TKey Id { get; set; }
    }
}
