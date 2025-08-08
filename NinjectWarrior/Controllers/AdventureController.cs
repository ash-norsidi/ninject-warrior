using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NinjectWarrior.Models;
using NinjectWarrior.Services;
using NinjectWarrior.ViewModels;

namespace NinjectWarrior.Controllers
{
    public class AdventureController(IAdventureService adventureService) : Controller
    {
        private readonly IAdventureService _adventureService = adventureService;

		// GET: /Adventure/
		public ActionResult Index()
        {
            var player = _adventureService.GetCurrentPlayer();
            return View(player);
        }

		// Replace the Error action with null-check to fix CS8602
		[Route("Adventure/Error")]
		public IActionResult Error()
		{
			var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
			var exception = exceptionFeature?.Error;

			// Safely handle possible null exception
			var errorMessage = exception?.Message ?? "An unknown error occurred.";

			return View(new ErrorViewModel { ErrorMessage = errorMessage });
		}

		// POST: /Adventure/Attack
		[HttpPost]
        public ActionResult Attack(string enemyName, int weaponType)
        {
            var result = _adventureService.PerformBattle(enemyName, (WeaponType)weaponType);
            var player = _adventureService.GetCurrentPlayer();
            ViewBag.BattleResult = result;
            ViewBag.BattleOver = player.CurrentGameState != GameState.Battle;
            return PartialView("_BattleSection", player);
        }

        [HttpGet]
        public IActionResult BattleSection()
        {
            var player = _adventureService.GetCurrentPlayer();
            return PartialView("_BattleSection", player);
        }

        // POST: /Adventure/ProcessQuestChoice
        [HttpPost]
        public ActionResult ProcessQuestChoice(string questId, string choiceId)
        {
            _adventureService.ProcessQuestChoice(questId, choiceId);
            return RedirectToAction("Index");
        }

        // POST: /Adventure/SolvePuzzle
        [HttpPost]
        public ActionResult SolvePuzzle(string solution)
        {
            bool correct = _adventureService.SolvePuzzle(solution);
            if (!correct)
            {
                TempData["PuzzleMessage"] = "That's not the right answer. Try again!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult UpdateEquipment(FormCollection form)
		{
			var player = _adventureService.GetCurrentPlayer();
			var equippedItems = new Dictionary<EquipmentSlot, int>();

			foreach (string key in form.Keys)
			{
				if (Enum.TryParse<EquipmentSlot>(key, out var slot))
				{
					if (int.TryParse(form[key], out int equipmentId))
					{
						equippedItems[slot] = equipmentId;
					}
				}
			}

			_adventureService.UpdatePlayerEquipment(player.Id, equippedItems);
			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_BattleSection", player);
            }
            return RedirectToAction("Index");
		}
    }
}
