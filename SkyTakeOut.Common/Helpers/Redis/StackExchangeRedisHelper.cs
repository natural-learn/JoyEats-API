using StackExchange.Redis;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;

namespace SkyTakeOut.Common.Helpers.Redis
{
    public class StackExchangeRedisHelper
    {
        private static readonly IDatabase _db;
        private static readonly Lazy<IConnectionMultiplexer> _lazyMultiplexer;
        private static readonly int _database;
        private static readonly JsonSerializerOptions _jsonSerializerOptions;

        static StackExchangeRedisHelper()
        {
            string defaultConnectionString = "localhost:6379"; // 默认连接字符串
            _database = 0; // 默认数据库索引
            _lazyMultiplexer = new Lazy<IConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(defaultConnectionString);
            });
            _db = _lazyMultiplexer.Value.GetDatabase(_database);
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
            };
        }

        #region 字符串(String)操作

        /// <summary>
        /// 设置指定键的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否设置成功</returns>
        public static async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await _db.StringSetAsync(key, value, expiry);
        }

        public static async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);
            return await _db.StringSetAsync(key, json, expiry);
        }

        /// <summary>
        /// 获取指定键的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static async Task<string?> StringGetAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public static async Task<T> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNull)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value, _jsonSerializerOptions);
        }

        /// <summary>
        /// 递增数值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">递增的值，默认1</param>
        /// <returns>递增后的值</returns>
        public static async Task<long> StringIncrementAsync(string key, long value = 1)
        {
            return await _db.StringIncrementAsync(key, value);
        }

        /// <summary>
        /// 递减数值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">递减的值，默认1</param>
        /// <returns>递减后的值</returns>
        public static async Task<long> StringDecrementAsync(string key, long value = 1)
        {
            return await _db.StringDecrementAsync(key, value);
        }

        #endregion

        #region 哈希(Hash)操作

        /// <summary>
        /// 设置哈希字段值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public static async Task<bool> HashSetAsync(string key, string field, string value)
        {
            return await _db.HashSetAsync(key, field, value);
        }

        /// <summary>
        /// 批量设置哈希字段值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">字段值字典</param>
        public static async Task HashSetAsync(string key, Dictionary<string, string> values)
        {
            var entries = values.Select(kvp => new HashEntry(kvp.Key, kvp.Value)).ToArray();
            await _db.HashSetAsync(key, entries);
        }

        /// <summary>
        /// 获取哈希字段值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>值</returns>
        public static async Task<string> HashGetAsync(string key, string field)
        {
            return await _db.HashGetAsync(key, field);
        }

        /// <summary>
        /// 获取哈希所有字段和值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字段值字典</returns>
        public static async Task<Dictionary<string, string>> HashGetAllAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key), "哈希键不能为null或空字符串");
            }

            HashEntry[] hashEntries = await _db.HashGetAllAsync(key);
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var entry in hashEntries)
            {
                string field = entry.Name.ToString();
                string value = entry.Value.ToString();
                result[field] = value;
            }
            return result;
        }

        /// <summary>
        /// 删除哈希字段
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="fields">字段</param>
        /// <returns>删除的字段数量</returns>
        public static async Task<long> HashDeleteAsync(string key, params string[] fields)
        {
            var redisFields = fields.Select(f => (RedisValue)f).ToArray();
            return await _db.HashDeleteAsync(key, redisFields);
        }

        /// <summary>
        /// 判断哈希字段是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>是否存在</returns>
        public static async Task<bool> HashExistsAsync(string key, string field)
        {
            return await _db.HashExistsAsync(key, field);
        }

        #endregion

        #region 列表(List)操作

        /// <summary>
        /// 左侧插入元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值</param>
        /// <returns>列表长度</returns>
        public static async Task<long> ListLeftPushAsync(string key, params string[] values)
        {
            var redisValues = values.Select(v => (RedisValue)v).ToArray();
            return await _db.ListLeftPushAsync(key, redisValues);
        }

        /// <summary>
        /// 右侧弹出元素
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>弹出的元素</returns>
        public static async Task<string> ListRightPopAsync(string key)
        {
            return await _db.ListRightPopAsync(key);
        }

        /// <summary>
        /// 阻塞式右侧弹出元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>弹出的元素</returns>
        //public async Task<string> ListRightPopAsync(string key, TimeSpan timeout)
        //{
        //    return await _db.ListRightPopAsync(key, timeout);
        //}

        /// <summary>
        /// 获取列表范围元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引，-1表示最后一个元素</param>
        /// <returns>元素列表</returns>
        public static async Task<List<string>> ListRangeAsync(string key, long start = 0, long stop = -1)
        {
            var values = await _db.ListRangeAsync(key, start, stop);
            return values.Select(v => v.ToString()).ToList();
        }

        #endregion

        #region 集合(Set)操作

        /// <summary>
        /// 添加集合元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值</param>
        /// <returns>添加的元素数量</returns>
        public static async Task<long> SetAddAsync(string key, params string[] values)
        {
            var redisValues = values.Select(v => (RedisValue)v).ToArray();
            return await _db.SetAddAsync(key, redisValues);
        }

        /// <summary>
        /// 获取集合所有元素
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>元素列表</returns>
        public static async Task<List<string>> SetMembersAsync(string key)
        {
            var values = await _db.SetMembersAsync(key);
            return values.Select(v => v.ToString()).ToList();
        }

        /// <summary>
        /// 移除集合元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值</param>
        /// <returns>移除的元素数量</returns>
        public static async Task<long> SetRemoveAsync(string key, params string[] values)
        {
            var redisValues = values.Select(v => (RedisValue)v).ToArray();
            return await _db.SetRemoveAsync(key, redisValues);
        }

        /// <summary>
        /// 判断元素是否在集合中
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否存在</returns>
        public static async Task<bool> SetContainsAsync(string key, string value)
        {
            return await _db.SetContainsAsync(key, value);
        }

        #endregion

        #region 有序集合(SortedSet)操作

        /// <summary>
        /// 添加有序集合元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="score">分数</param>
        /// <returns>是否添加成功</returns>
        public static async Task<bool> SortedSetAddAsync(string key, string value, double score)
        {
            return await _db.SortedSetAddAsync(key, value, score);
        }

        /// <summary>
        /// 获取有序集合范围元素（按排名）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="start">开始排名</param>
        /// <param name="stop">结束排名</param>
        /// <param name="order">排序方式</param>
        /// <returns>元素列表</returns>
        public static async Task<List<string>> SortedSetRangeByRankAsync(string key, long start = 0, long stop = -1, Order order = Order.Ascending)
        {
            var values = await _db.SortedSetRangeByRankAsync(key, start, stop, order);
            return values.Select(v => v.ToString()).ToList();
        }

        /// <summary>
        /// 获取元素在有序集合中的排名
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="order">排序方式</param>
        /// <returns>排名，从0开始</returns>
        public static async Task<long?> SortedSetRankAsync(string key, string value, Order order = Order.Ascending)
        {
            return await _db.SortedSetRankAsync(key, value, order);
        }

        /// <summary>
        /// 增加有序集合元素的分数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="value">增加的分数</param>
        /// <returns>更新后的分数</returns>
        public static async Task<double> SortedSetIncrementAsync(string key, string value, double increment = 1)
        {
            return await _db.SortedSetIncrementAsync(key, value, increment);
        }

        #endregion

        #region 键(Key)管理

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否删除成功</returns>
        public static async Task<bool> KeyDeleteAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 判断键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public static async Task<bool> KeyExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        /// <summary>
        /// 设置键过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否设置成功</returns>
        public static async Task<bool> KeyExpireAsync(string key, TimeSpan expiry)
        {
            return await _db.KeyExpireAsync(key, expiry);
        }

        /// <summary>
        /// 查找匹配的键
        /// </summary>
        /// <param name="pattern">匹配模式，如user:*</param>
        /// <returns>键列表</returns>
        public static async IAsyncEnumerable<string> KeysAsync(string pattern)
        {
            foreach (var endpoint in _lazyMultiplexer.Value.GetEndPoints())
            {
                var server = _lazyMultiplexer.Value.GetServer(endpoint);
                await foreach (var key in server.KeysAsync(pattern: pattern))
                {
                    yield return key.ToString();
                }
            }
        }

        /// <summary>
        /// 清理所有匹配的键
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static async Task CleanCacheAsync(string pattern)
        {
            var keysBatch = new List<RedisKey>();
            await foreach (var key in KeysAsync(pattern))
            {
                keysBatch.Add(key);
                if(keysBatch.Count >= 100)
                {
                    await _db.KeyDeleteAsync(keysBatch.ToArray());
                    keysBatch.Clear();
                }
            }
            if (keysBatch.Count > 0)
            {
                await _db.KeyDeleteAsync(keysBatch.ToArray());
            }
        }

        #endregion

        #region 分布式锁

        /// <summary>
        /// 获取分布式锁
        /// </summary>
        /// <param name="lockKey">锁键</param>
        /// <param name="expireTime">锁过期时间</param>
        /// <returns>锁标识，释放锁时需要传入</returns>
        public static async Task<string> AcquireLockAsync(string lockKey, TimeSpan expireTime)
        {
            if (string.IsNullOrEmpty(lockKey))
                throw new ArgumentNullException(nameof(lockKey));

            var lockValue = Guid.NewGuid().ToString();
            var acquired = await _db.StringSetAsync(lockKey, lockValue, expireTime, When.NotExists);

            return acquired ? lockValue : null;
        }

        /// <summary>
        /// 释放分布式锁
        /// </summary>
        /// <param name="lockKey">锁键</param>
        /// <param name="lockValue">锁标识</param>
        /// <returns>是否释放成功</returns>
        public static async Task<bool> ReleaseLockAsync(string lockKey, string lockValue)
        {
            if (string.IsNullOrEmpty(lockKey))
                throw new ArgumentNullException(nameof(lockKey));
            if (string.IsNullOrEmpty(lockValue))
                throw new ArgumentNullException(nameof(lockValue));

            // 使用Lua脚本保证原子性
            const string script = @"
                if redis.call('get', KEYS[1]) == ARGV[1] then
                    return redis.call('del', KEYS[1])
                else
                    return 0
                end
            ";

            var result = await _db.ScriptEvaluateAsync(script, new[] { (RedisKey)lockKey }, new[] { (RedisValue)lockValue });
            return (long)result == 1;
        }

        #endregion

        #region 发布订阅

        /// <summary>
        /// 订阅频道
        /// </summary>
        /// <param name="channel">频道名称</param>
        /// <param name="handler">消息处理函数</param>
        public static async Task SubscribeAsync(string channel, Action<string, string> handler)
        {
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException(nameof(channel));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var subscriber = _lazyMultiplexer.Value.GetSubscriber();
            await subscriber.SubscribeAsync(channel, (redisChannel, value) =>
            {
                handler(redisChannel.ToString(), value.ToString());
            });
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">频道名称</param>
        /// <param name="message">消息内容</param>
        /// <returns>接收消息的客户端数量</returns>
        public static async Task<long> PublishAsync(string channel, string message)
        {
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException(nameof(channel));

            var subscriber = _lazyMultiplexer.Value.GetSubscriber();
            return await subscriber.PublishAsync(channel, message);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="channel">频道名称</param>
        public static async Task UnsubscribeAsync(string channel)
        {
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException(nameof(channel));

            var subscriber = _lazyMultiplexer.Value.GetSubscriber();
            await subscriber.UnsubscribeAsync(channel);
        }

        #endregion
    }
}
