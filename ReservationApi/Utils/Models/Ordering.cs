using System.Linq;
using System.Runtime.Serialization;
using WebApplication.Utils.Extensions;

namespace WebApplication.Utils.Models
{
  [DataContract]
  public class Ordering
  {
    [DataMember]
    public string column { get; set; }
    
  }

  public static class OrderingExtensions
  {
    public static bool IsOrdering(this Ordering ordering)
    {
      return ordering.column != "";
    }

    public static bool IsDescending(this Ordering ordering)
    {
      return ordering.column.StartsWith('-');
    }

    public static string GetOrderingColumn(this Ordering ordering)
    {
      if (ordering.IsDescending())
        return ordering.column.Substring(1);
      return ordering.column;
    }

    public static IQueryable SetOrderQuery<T>(this Ordering ordering, IQueryable<T> query)
    {
      if (ordering.IsOrdering())
      {
        var order = ordering.GetOrderingColumn();
        return query.OrderBy( order, ordering.IsDescending());
      }
      return query;
    }
  }
}