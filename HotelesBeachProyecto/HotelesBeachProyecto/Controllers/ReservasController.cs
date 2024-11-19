﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelesBeachProyecto.Data; // Reemplaza con tu contexto de base de datos
using HotelesBeachProyecto.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelesBeachProyecto.Controllers
{
    public class ReservasController : Controller
    {
        private readonly DbContextHotel _context;

        // Constructor: Recibe el contexto de la base de datos
        public ReservasController(DbContextHotel context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Paquete)
                .Include(r => r.FormaPago)
                .Include(r => r.Usuario)
                .ToListAsync();
            return View(reservas);
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Paquete)
                .Include(r => r.FormaPago)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["PaqueteId"] = new SelectList(_context.Paquetes, "Id", "Nombre"); // Llena opciones de Paquete
            ViewData["FormaPagoId"] = new SelectList(_context.FormasPago, "Id", "Nombre"); // Llena opciones de FormaPago
            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CantidadNoches,CantidadPersonas,Descuento,MontoRebajado,MontoFinal,PaqueteId,FormaPagoId,ClienteCedula")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                // Calcula los montos automáticamente
                reserva.MontoRebajado = CalculaMontoRebajado(reserva.CantidadNoches, reserva.PaqueteId, reserva.Descuento);
                reserva.MontoFinal = CalculaMontoFinal(reserva.MontoRebajado, reserva.Descuento);

                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PaqueteId"] = new SelectList(_context.Paquetes, "Id", "Nombre", reserva.PaqueteId);
            ViewData["FormaPagoId"] = new SelectList(_context.FormasPago, "Id", "Nombre", reserva.FormaPagoId);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            ViewData["PaqueteId"] = new SelectList(_context.Paquetes, "Id", "Nombre", reserva.PaqueteId);
            ViewData["FormaPagoId"] = new SelectList(_context.FormasPago, "Id", "Nombre", reserva.FormaPagoId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CantidadNoches,CantidadPersonas,Descuento,MontoRebajado,MontoFinal,PaqueteId,FormaPagoId,ClienteCedula")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    reserva.MontoRebajado = CalculaMontoRebajado(reserva.CantidadNoches, reserva.PaqueteId, reserva.Descuento);
                    reserva.MontoFinal = CalculaMontoFinal(reserva.MontoRebajado, reserva.Descuento);

                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["PaqueteId"] = new SelectList(_context.Paquetes, "Id", "Nombre", reserva.PaqueteId);
            ViewData["FormaPagoId"] = new SelectList(_context.FormasPago, "Id", "Nombre", reserva.FormaPagoId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Paquete)
                .Include(r => r.FormaPago)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Métodos Auxiliares
        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }

        private decimal CalculaMontoRebajado(int cantidadNoches, int paqueteId, decimal descuento)
        {
            // Implementa la lógica para calcular el monto rebajado
            decimal precioBase = _context.Paquetes.Find(paqueteId)?.Costo ?? 0;
            return cantidadNoches * precioBase * (1 - (descuento / 100));
        }

        private decimal CalculaMontoFinal(decimal montoRebajado, decimal descuento)
        {
            // Implementa lógica adicional si es necesario
            return montoRebajado;
        }
    }
}