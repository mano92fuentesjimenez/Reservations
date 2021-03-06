using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Utils.Extensions;
using WebApplication.Utils.Models;

namespace WebApplication.Controllers
{
  [Route("api/reservation")]
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

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
    {
      var errorMessage = await isValid(reservation);
      if (errorMessage != null)
        return BadRequest(errorMessage);

      await _reservationContext.Reservations.AddAsync(reservation);
      await _reservationContext.SaveChangesAsync();

      return CreatedAtAction(nameof(CreateReservation), reservation);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Reservation>> UpdateReservation(int id, Reservation reservation)
    {
      var reservationById = await _reservationContext.Reservations.FindAsync(id);
      var errorMessage = await isValid(reservation);

      if (errorMessage != null)
        return BadRequest(errorMessage);

      reservationById.ClientId = reservation.ClientId;
      reservationById.Description = reservation.Description;
      reservationById.Favorite = reservation.Favorite;
      reservationById.Name = reservation.Name;
      reservationById.CreationTime = reservation.CreationTime;
      reservationById.Ranking = reservation.Ranking;

      await _reservationContext.SaveChangesAsync();
      return Ok(reservationById);
    }

    private async Task<string?> isValid(Reservation reservation)
    {
      var client = await _reservationContext.Clients.FindAsync(reservation.ClientId);

      if (client == null)
        return "Linked client doesn't exist";
      return null;
    }
  }
}