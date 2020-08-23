using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Utils
{
  public static class ControllerBaseExtensions
  {
    public static bool IsOrderingValid<T>(this ControllerBase controllerBase, Ordering ordering)
    {
      if (ordering.IsOrdering())
      {
        var reservationType = typeof(T);
        var orderingColumn = ordering.GetOrderingColumn();
        return reservationType.GetProperty(orderingColumn) != null;
      }

      return true;
    }
  }
}