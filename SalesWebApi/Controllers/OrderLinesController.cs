using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApi.Models;

namespace SalesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderLinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderLinesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderLine>>> GetOrderLine()
        {
            return await _context.OrderLine.Include(x => x.Order).ToListAsync();
        }

        // GET: api/OrderLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderLine>> GetOrderLine(int id)
        {
            var orderLine = await _context.OrderLine.Include(x => x.Order).SingleOrDefaultAsync(x => x.Id == id);

            if (orderLine == null)
            {
                return NotFound();
            }

            return orderLine;
        }

        //Find all quantity x price for each orderline and update the order.Total with the value. The method
        //accepts an order ID as a parameter of the order update
        [HttpPut("update/{orderId}")] //tells it which put method to call
        public async Task<IActionResult> UpdateOrderLine(int orderId) //return type is same, method name is different,
                                                                      //var name must match parameter name in {}
        {
            var order = await _context.Orders.FindAsync(orderId); //find on order ID they pass in, put result in order
            if (order == null)
            {
                NotFound();
            }
            order.Total = (from ol in _context.OrderLine //load total property of order just read, all order lines
                           where ol.OrderId == orderId //where order ID matches order ID passed in
                           select new //subset of columns
                           {
                               LineTotal = ol.Quantity * ol.Price //for each line, take QxP and create new prop called line total
                           }) //wrap query syntax in () so can add aggregate linq method below
                           .Sum(x => x.LineTotal); //then sum linetotal, get one value and stored in "total"
            await _context.SaveChangesAsync(); //now total column in order has been updated and save changes
            return Ok();
        }

        // PUT: api/OrderLines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderLine(int id, OrderLine orderLine)
        {
            if (id != orderLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderLineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrderLines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderLine>> PostOrderLine(OrderLine orderLine)
        {
            _context.OrderLine.Add(orderLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderLine", new { id = orderLine.Id }, orderLine);
        }

        // DELETE: api/OrderLines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderLine(int id)
        {
            var orderLine = await _context.OrderLine.FindAsync(id);
            if (orderLine == null)
            {
                return NotFound();
            }

            _context.OrderLine.Remove(orderLine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderLineExists(int id)
        {
            return _context.OrderLine.Any(e => e.Id == id);
        }
    }
}
