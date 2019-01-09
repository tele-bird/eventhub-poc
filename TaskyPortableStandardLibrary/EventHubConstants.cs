namespace Tasky.PortableStandardLibrary
{
    internal class EventHubConstants
    {
        internal const string EventHubConnectionString = "Endpoint=sb://path";
        internal const string EventHubName = "my-eventhub";
        internal const string StorageContainerName = "mystorageaccountcontainer";
        internal const string StorageAccountName = "mustorageaccount";
        internal const string StorageAccountKey = "areallylongunreadablestringofcharactersthatisprovidedbyazureportal939rlkf+*$lklkwjr939rlkf+*$lklkwjr939rlkf+*$lklkwjr939rlkf+*$lklkwjr939rlkf+*$lklkwjr";
        internal static readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);
    }
}
