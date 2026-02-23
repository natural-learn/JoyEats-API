using StackExchange.Redis;

namespace JoyEats.Common.Helpers.Redis
{
    /// <summary>
    /// Redis操作帮助接口
    /// </summary>
    public interface IRedisHelper
    {
        #region 字符串(String)操作

        /// <summary>
        /// 设置指定键的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// 获取指定键的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        Task<string> StringGetAsync(string key);

        /// <summary>
        /// 递增数值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">递增的值，默认1</param>
        /// <returns>递增后的值</returns>
        Task<long> StringIncrementAsync(string key, long value = 1);

        /// <summary>
        /// 递减数值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">递减的值，默认1</param>
        /// <returns>递减后的值</returns>
        Task<long> StringDecrementAsync(string key, long value = 1);
        #endregion

        #region 哈希(Hash)操作

        /// <summary>
        /// 设置哈希字段值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        Task<bool> HashSetAsync(string key, string field, string value);

        /// <summary>
        /// 批量设置哈希字段值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">字段值字典</param>
        Task HashSetAsync(string key, Dictionary<string, string> values);

        /// <summary>
        /// 获取哈希字段值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>值</returns>
        Task<string> HashGetAsync(string key, string field);

        /// <summary>
        /// 获取哈希所有字段和值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字段值字典</returns>
        Task<Dictionary<string, string>> HashGetAllAsync(string key);

        /// <summary>
        /// 删除哈希字段
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="fields">字段</param>
        /// <returns>删除的字段数量</returns>
        Task<long> HashDeleteAsync(string key, params string[] fields);

        /// <summary>
        /// 判断哈希字段是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>是否存在</returns>
        Task<bool> HashExistsAsync(string key, string field);

        #endregion

        #region 列表(List)操作

        /// <summary>
        /// 左侧插入元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值</param>
        /// <returns>列表长度</returns>
        Task<long> ListLeftPushAsync(string key, params string[] values);

        /// <summary>
        /// 右侧弹出元素
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>弹出的元素</returns>
        Task<string> ListRightPopAsync(string key);

        /// <summary>
        /// 阻塞式右侧弹出元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>弹出的元素</returns>
        //Task<string> ListRightPopAsync(string key, TimeSpan timeout);

        /// <summary>
        /// 获取列表范围元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引，-1表示最后一个元素</param>
        /// <returns>元素列表</returns>
        Task<List<string>> ListRangeAsync(string key, long start = 0, long stop = -1);

        #endregion

        #region 集合(Set)操作

        /// <summary>
        /// 添加集合元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值</param>
        /// <returns>添加的元素数量</returns>
        Task<long> SetAddAsync(string key, params string[] values);

        /// <summary>
        /// 获取集合所有元素
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>元素列表</returns>
        Task<List<string>> SetMembersAsync(string key);

        /// <summary>
        /// 移除集合元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值</param>
        /// <returns>移除的元素数量</returns>
        Task<long> SetRemoveAsync(string key, params string[] values);

        /// <summary>
        /// 判断元素是否在集合中
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否存在</returns>
        Task<bool> SetContainsAsync(string key, string value);

        #endregion

        #region 有序集合(SortedSet)操作

        /// <summary>
        /// 添加有序集合元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="score">分数</param>
        /// <returns>是否添加成功</returns>
        Task<bool> SortedSetAddAsync(string key, string value, double score);

        /// <summary>
        /// 获取有序集合范围元素（按排名）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="start">开始排名</param>
        /// <param name="stop">结束排名</param>
        /// <param name="order">排序方式</param>
        /// <returns>元素列表</returns>
        Task<List<string>> SortedSetRangeByRankAsync(string key, long start = 0, long stop = -1, Order order = Order.Ascending);

        /// <summary>
        /// 获取元素在有序集合中的排名
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="order">排序方式</param>
        /// <returns>排名，从0开始</returns>
        Task<long?> SortedSetRankAsync(string key, string value, Order order = Order.Ascending);

        /// <summary>
        /// 增加有序集合元素的分数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="value">增加的分数</param>
        /// <returns>更新后的分数</returns>
        Task<double> SortedSetIncrementAsync(string key, string value, double increment = 1);

        #endregion

        #region 键(Key)管理

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否删除成功</returns>
        Task<bool> KeyDeleteAsync(string key);

        /// <summary>
        /// 判断键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        Task<bool> KeyExistsAsync(string key);

        /// <summary>
        /// 设置键过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否设置成功</returns>
        Task<bool> KeyExpireAsync(string key, TimeSpan expiry);

        /// <summary>
        /// 查找匹配的键
        /// </summary>
        /// <param name="pattern">匹配模式，如user:*</param>
        /// <returns>键列表</returns>
        IEnumerable<string> Keys(string pattern);

        #endregion

        #region 分布式锁

        /// <summary>
        /// 获取分布式锁
        /// </summary>
        /// <param name="lockKey">锁键</param>
        /// <param name="expireTime">锁过期时间</param>
        /// <returns>锁标识，释放锁时需要传入</returns>
        Task<string> AcquireLockAsync(string lockKey, TimeSpan expireTime);

        /// <summary>
        /// 释放分布式锁
        /// </summary>
        /// <param name="lockKey">锁键</param>
        /// <param name="lockValue">锁标识</param>
        /// <returns>是否释放成功</returns>
        Task<bool> ReleaseLockAsync(string lockKey, string lockValue);

        #endregion

        #region 发布订阅

        /// <summary>
        /// 订阅频道
        /// </summary>
        /// <param name="channel">频道名称</param>
        /// <param name="handler">消息处理函数</param>
        Task SubscribeAsync(string channel, Action<string, string> handler);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">频道名称</param>
        /// <param name="message">消息内容</param>
        /// <returns>接收消息的客户端数量</returns>
        Task<long> PublishAsync(string channel, string message);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="channel">频道名称</param>
        Task UnsubscribeAsync(string channel);

        #endregion
    }
}
