using System.Runtime.Serialization;

namespace WebApplication.Utils.Models
{
 
  [DataContract]
  public class Paging
  {
    private static int defaultPerPage = 30;
    
    private int _perPage;
    [DataMember]
    public int PerPage {
      get
      {
        return _perPage == 0 ? defaultPerPage: _perPage ;
      }
      set
      {
        _perPage = value;
      }
    }
    [DataMember]
    public int Total { get; set; }
    [DataMember]
    public int Page { get; set; } 
  }

  public static class PagingExtensions
  {
    public static int OffSet(this Paging paging)
    {
      return paging.PerPage * paging.Page;
    }

    public static Paging CreatePaging(this Paging paging, int total)
    {
      return new Paging()
      {
        PerPage = paging.PerPage,
        Total = total,
        Page = paging.Page
      };
    }
  }
}