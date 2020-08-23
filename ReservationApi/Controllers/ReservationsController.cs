using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Utils;

namespace WebApplication.Controllers
{
  [Route("api/reservations")]
  [ApiController]
  public class ReservationsController : ControllerBase
  {
    private readonly ReservationContext _reservationContext;

    public ReservationsController(ReservationContext reservationContext)
    {
      this._reservationContext = reservationContext;
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<Reservation>>> GetReservationsList([FromQuery] ListFilter filters)
    {
      var paging = filters.Paging;
      var ordering = filters.Ordering;

      if (!this.IsOrderingValid<Reservation>(ordering))
        return BadRequest("Column \"" + ordering.GetOrderingColumn() + "\" doesn't exist");
      
      var count = await this._reservationContext.Reservations.CountAsync();
      var query = this._reservationContext.Reservations
        .Skip(paging.OffSet())
        .Take(paging.PerPage);

      query = (IQueryable<Reservation>)ordering.SetOrderQuery(query);
      var results = await query.ToListAsync();

      return new PagedResponse<Reservation>()
      {
        Data = results,
        Ordering = filters.Ordering,
        Paging = paging.CreatePaging(count),
      };
    }
  }
}