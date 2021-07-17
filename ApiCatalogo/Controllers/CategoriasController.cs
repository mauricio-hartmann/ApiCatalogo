﻿using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias
                .AsNoTracking()
                .ToList();
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias
                .Include(c => c.Produtos)
                .ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _context.Categorias
                .AsNoTracking()
                .FirstOrDefault(p => p.CategoriaId == id);
            if (categoria == null)
                return NotFound();
            return categoria;
        }

        [HttpPost]
        public ActionResult Incluir([FromBody] Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Alterar(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest();
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            if (categoria == null)
                return NotFound();
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return categoria;
        }
    }
}