﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedMe.Data;
using ourProject.Models;

namespace FeedMe.Controllers
{
    public class DishesController : Controller
    {
        private readonly FeedMeContext _context;

        public DishesController(FeedMeContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            var feedMeContext = _context.Dish.Include(d => d.Restaurant);
            return View(await feedMeContext.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.Restaurant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "Name");
            ViewBag.FoodType = new SelectList(Enum.GetNames(typeof(FoodType)));
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,DishImage,Description,FoodType,Price,Restaurant")] Dish dish, int restaurant)
        {
            if (ModelState.IsValid)
            {
                //dish.Restaurant = new Restaurant();
                //dish.Restaurant = _context.Restaurant.Any(e => e.ID == restaurant);
                //dish.Restaurant = _context.Restaurant.FirstOrDefaultAsync(m => m.ID == restaurant);
                //dish.Restaurant = new Restaurant();
                //dish.Restaurant.Name = _context.Restaurant.Where(x => dish.Contains(x.RestaurantID));
                //restaurant.Categories = new List<Category>();
                //restaurant.Categories.AddRange(_context.Category.Where(x => categories.Contains(x.ID)));

                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "Address", dish.RestaurantID);
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "Address", dish.RestaurantID);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,DishImage,Description,FoodType,Price,RestaurantID")] Dish dish)
        {
            if (id != dish.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.ID))
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
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "Address", dish.RestaurantID);
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.Restaurant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.ID == id);
        }
    }
}
