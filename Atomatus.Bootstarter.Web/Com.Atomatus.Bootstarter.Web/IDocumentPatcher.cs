namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// <para>
    /// Use this contract to enable usage of ApplyTo method to copy shared properties
    /// by name and type between the attached object and another one.
    /// </para>
    /// <para>
    /// Only the non-null property value will be copied to target object.
    /// Therefore this contract class is usable for REST PATCH method where the object is partially changed.
    /// </para>
    /// </summary>
    public interface IDocumentPatcher { }
}
