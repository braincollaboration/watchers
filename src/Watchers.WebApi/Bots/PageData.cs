namespace Watchers.WebApi.Bots
{
    public struct PageData
    {
        public PageData(string _uri, string _key)
        {
            URI = _uri;
            key = _key;
        }

        public string URI;
        public string key;
    }
}
