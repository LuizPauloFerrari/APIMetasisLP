using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIMetasisLP.Data;
using APIMetasisLP.Entities;
using APIMetasisLP.DTO;
using System.Net.Http;
using System.Net;
using Canducci.Pagination;
using Canducci.Pagination.Bases;

namespace APIMetasisLP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly APIMetasisLPContext _context;

        public ProdutoController(APIMetasisLPContext context)
        {
            _context = context;
        }

        // GET: api/Produto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProduto()
        {
            return await _context.Produto.ToListAsync();
        }
        
        // GET: api/Produto
        [HttpGet("page/{page?}")]
        //public async Task<ActionResult> GetProdutoPaginated(int? page)
        public async Task<ActionResult> GetProdutoPaginated(int? page)
        {
            page ??= 1;
            if (page <= 0) page = 1;

            var result = await _context
               .Produto
               .AsNoTracking()
               .OrderBy(c => c.ProdutoId)
               .ToPaginatedRestAsync(page.Value, 2);
            return Ok(result);

            //return await _context.Produto.ToListAsync();
        }
        
        [HttpGet("page2/{page?}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutoPaginated2(int? page)
        {
            page ??= 1;
            if (page <= 0) page = 1;

            var result = await _context
               .Produto
               .AsNoTracking()
               .OrderBy(c => c.ProdutoId)
               .ToPaginatedRestAsync(page.Value, 2);
            return Ok(result);
        }
        
        [HttpGet("Filter", Name = nameof(GetProdutoFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutoFilter([FromQuery] ProdutoDTO produto)
        {
            return await _context.Produto
             .Where(a => a.Descricao.Contains(produto.Descricao) || String.IsNullOrEmpty(produto.Descricao) )
             .Where(a => a.ProdutoId == produto.ProdutoId || produto.ProdutoId == 0)
             .Where(a => a.Preco >= produto.PrecoIni || produto.PrecoIni == 0)
             .Where(a => a.Preco <= produto.PrecoFim || produto.PrecoFim == 0)
             .OrderBy(a => a.ProdutoId)
             //.Select(a => ProdutoToDTO(a))
             .ToListAsync();
        }
        
        // POST: api/Produto
        [HttpPost("Filter")]
        public async Task<ActionResult<IEnumerable<Produto>>> PostProdutoFilter(ProdutoDTO produto)
        {
            //return await _context.Produto.ToListAsync();
            return await _context.Produto
              .Where(a => a.Descricao.Contains(produto.Descricao) || String.IsNullOrEmpty(produto.Descricao))
              .Where(a => a.ProdutoId == produto.ProdutoId || produto.ProdutoId == 0)
              .Where(a => a.Preco >= produto.PrecoIni || produto.PrecoIni == 0)
              .Where(a => a.Preco <= produto.PrecoFim || produto.PrecoFim == 0)
              .OrderBy(a => a.ProdutoId)
              //.Select(a => ProdutoToDTO(a))
              .ToListAsync();
        }

        // POST: api/Produto/FilterPage/{page}
        [HttpPost("FilterPage")]
        public async Task<ActionResult> PostProdutoFilterPage(int? size, int? page, ProdutoDTO produto)
        {
            page ??= 1;
            size ??= 2;
            if (page <= 0) page = 1;
            if (size <= 0) size = 2;

            var result = await _context.Produto
              .Where(a => a.Descricao.Contains(produto.Descricao) || String.IsNullOrEmpty(produto.Descricao))
              .Where(a => a.ProdutoId == produto.ProdutoId || produto.ProdutoId == 0)
              .Where(a => a.Preco >= produto.PrecoIni || produto.PrecoIni == 0)
              .Where(a => a.Preco <= produto.PrecoFim || produto.PrecoFim == 0)
              .AsNoTracking()
              .OrderBy(a => a.ProdutoId)
              .ToPaginatedRestAsync(page.Value, size.Value);
            return Ok(result);
        }

        // GET: api/Produto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            //await Task.Delay(5000);

            var produto = await _context.Produto.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // PUT: api/Produto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(produto.Descricao))
            {
                throw new ArgumentException("Descrição não informada.");
            }
            if (produto.Preco <= 0)
            {
                throw new ArgumentException("Preço não informado.");
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
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

        // POST: api/Produto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        [HttpPost(Name = nameof(PostProduto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            produto.ProdutoId = 0;
            if (String.IsNullOrEmpty(produto.Descricao))
            {
                throw new ArgumentException("Descrição não informada.");
            }
            if (produto.Preco <= 0)
            {
                throw new ArgumentException("Preço não informado.");
            }
            
            _context.Produto.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduto", new { id = produto.ProdutoId }, produto);
        }

        // DELETE: api/Produto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.ProdutoId == id);
        }

        //private static ProdutoDTO ProdutoToDTO(Produto produto) => new ProdutoDTO
        //{
        //    ProdutoId = produto.ProdutoId,
        //    Descricao = produto.Descricao,
        //    PrecoIni = produto.Preco,
        //    PrecoFim = produto.Preco
        //};
    }
}
