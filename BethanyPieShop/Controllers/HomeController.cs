﻿using BethanyPieShop.Models;
using BethanyPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanyPieShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPieRepository _pieRepository;

        public HomeController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }
        public IActionResult Index()
        {
            var piesOfTheWeek = _pieRepository.PiesOfTheWeek;
            var homeViewModel = new HomeViewModel(piesOfTheWeek);

            return View(homeViewModel);
        }
    }
}
