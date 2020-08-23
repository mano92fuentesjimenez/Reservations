using System.Runtime.Serialization;

namespace WebApplication.Utils.Models
{
  [DataContract]
  public class ListFilter
  {
    private Paging _paging;
    private Ordering _ordering;
    
    [DataMember]
    public Ordering Ordering {
      get
      {
        if(this._ordering == null)
          this._ordering = new Ordering(){ column = "" };

        return this._ordering;
      }
      set
      {
        this._ordering = value;
      } }
    
    [DataMember]
    public Paging Paging {
      get
      {
        if(this._paging == null)
          this._paging = new Paging();

        return this._paging;
      }
      set
      {
        this._paging = value;
      } 
    }
  }
}