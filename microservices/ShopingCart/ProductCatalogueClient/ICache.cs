namespace ShopingCart
{
    public interface ICache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl">time to live - время жизни</param>
        void Add(string key, object value, TimeSpan ttl);
        object Get(string key);
    }
}