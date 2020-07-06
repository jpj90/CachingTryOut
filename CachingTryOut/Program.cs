using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingTryOut
{
  class Program
  {
    static void Main(string[] args)
    {
      var cache = new MyCache();

      //ReadAndUpdateCacheSimple(cache);

      for (int i = 0; i < 30; i++)
      {
        var taskId = i;
        Task.Factory.StartNew(() => ReadCacheAndCountItems(cache, taskId));
        //Console.WriteLine($"started task {taskId}");
      }

      Random rnd = new Random();
      int rnNum = rnd.Next(1, 20);
      Thread.Sleep(1000 * rnNum);

      cache.RefreshCachedItems();

      Console.Read();
    }

    public static void ReadCacheAndCountItems(MyCache cache, int i)
    {
      Random rnd = new Random();
      int rnNum = rnd.Next(1, 20);
      Thread.Sleep(200 * rnNum);

      var itemCount = cache.GetCachedItems().Count;
      Console.WriteLine($"Task {i}: found {itemCount} items in cache after sleeping for {200 * rnNum}");
    }

    static void ReadAndUpdateCacheSimple(MyCache cache)
    {
      var cachedItems = cache.GetCachedItems();
      foreach (var item in cachedItems)
      {
        Console.WriteLine($"found item with id {item.Id } and name {item.Name}");
      }

      cache.RefreshCachedItems();
      cachedItems = cache.GetCachedItems();

      foreach (var item in cachedItems)
      {
        Console.WriteLine($"found item with id {item.Id } and name {item.Name}");
      }
    }
  }
}
