using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace CachingTryOut
{
  public class MyCache
  {
    private readonly ObjectCache _cache;
    private const string Key = "my_items";
    private readonly object _cacheLock = new object();

    public MyCache()
    {
      _cache = MemoryCache.Default;
      CacheItemPolicy policy = GetPolicy();
      SetInitialItems(policy);
    }

    public List<Item> GetCachedItems()
    {
      lock (_cacheLock)
      {
        return _cache.Get(Key) as List<Item>;
      }
    }

    public void RefreshCachedItems()
    {
      SetUpdatedItems();
    }

    private void SetInitialItems(CacheItemPolicy policy)
    {
      var items = new List<Item>
      {
        new Item()
        {
          Id = 1, Name = "Item 1"
        },
        new Item()
        {
          Id = 2, Name = "Item 2"
        }
      };
      lock (_cacheLock)
      {
        _cache.Set(Key, items, policy);
      }
    }

    private void SetUpdatedItems()
    {
      lock (_cacheLock)
      {
        _cache.Remove(Key);
      }

      //Thread.Sleep(2000);

      var items = new List<Item>
      {
        new Item()
        {
          Id = 1, Name = "Item 1 - Updated"
        },
        new Item()
        {
          Id = 2, Name = "Item 2 - Updated"
        },
        new Item()
        {
          Id = 3, Name = "Item 3"
        },
        new Item()
        {
          Id = 4, Name = "Item 4"
        }
      };
      var policy = GetPolicy();
      lock (_cacheLock)
      {
        _cache.Set(Key, items, policy);
      }
    }

    private CacheItemPolicy GetPolicy()
    {
      var policy = new CacheItemPolicy();
      policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(1.0);
      policy.RemovedCallback = arguments => ShowCacheClearedMessage();
      return policy;
    }

    private void ShowCacheClearedMessage()
    {
      Console.WriteLine("Cache has been cleared");
    }
  }
}
