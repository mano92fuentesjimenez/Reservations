using System.Runtime.Serialization;

namespace WebApplication.Utils
{
  [DataContract]
  public class PagedResponse<T>
  {
    [DataMember]
    public Ordering Ordering { get; set; }
    
    [DataMember]
    public Paging Paging { get; set; }
    
    [DataMember]
    public T Data { get; set; }
  }
}