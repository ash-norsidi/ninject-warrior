using Newtonsoft.Json;
using NinjectWarrior.Models;

namespace NinjectWarrior.Repositories
{
	public class PlayerRepository(IEquipmentRepository equipmentRepository, IWebHostEnvironment hostingEnvironment) : IPlayerRepository
	{
		private readonly IEquipmentRepository _equipmentRepository = equipmentRepository;
		private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment; // Add this field
		private Player? _player;

		public Player GetPlayerById(int id)
		{
			if (_player == null)
			{
				// For demo, always return player with Id = 1
				_player = new Player { Id = id, Name = "Hero", Level = 1, Health = 100, Weapon = WeaponType.Sword, Strength = 10, Defense = 5, Evasion = 5, Luck = 1, Experience = 0, ExperienceToNextLevel = 100 };

				if (_player != null)
				{
					_player.Inventory = new Inventory();
					var itemsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Data", "items.json");
					var itemsJson = File.ReadAllText(itemsPath);
					var items = JsonConvert.DeserializeObject<List<Item>>(itemsJson) ?? [];
					foreach (var item in items)
					{
						_player.Inventory.AddItem(item);
					}

					var equipmentPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Data", "equipment.json");
					var equipmentJson = File.ReadAllText(equipmentPath);
					var equipment = JsonConvert.DeserializeObject<List<Equipment>>(equipmentJson) ?? [];
					foreach (var item in equipment)
					{
						_player.Inventory.AddItem(item);
					}

					_player.EquippedItems = GetEquippedItems(_player.Id);
					_player.CalculateBonuses();
				}
				else
				{
					// For demo, return a default player if not found
					_player = new Player
					{
						Id = id,
						Name = "Hero",
						Level = 1,
						Health = 100,
						Weapon = WeaponType.Sword,
						Strength = 10,
						Defense = 5,
						Evasion = 5,
						Luck = 1,
						Experience = 0,
						ExperienceToNextLevel = 100,
						Inventory = new Inventory(),
						EquippedItems = new Dictionary<EquipmentSlot, IEquipment>()
					};
					_player.CalculateBonuses();
				}
			}

			return _player;
		}

		private IDictionary<EquipmentSlot, IEquipment> GetEquippedItems(int playerId)
		{
			var playerEquipmentPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Data", "player_equipment.json");
			var playerEquipmentJson = File.ReadAllText(playerEquipmentPath);
			var playerEquipment = JsonConvert.DeserializeObject<List<PlayerEquipment>>(playerEquipmentJson) ?? [];

			var equippedItems = new Dictionary<EquipmentSlot, IEquipment>();
			var playerItems = playerEquipment.Where(pe => pe.PlayerId == playerId);

			foreach (var playerItem in playerItems)
			{
				var equipment = _equipmentRepository.GetEquipmentById(playerItem.EquipmentId);
				if (equipment != null)
				{
					equippedItems[equipment.Slot] = equipment;
				}
			}

			return equippedItems;
		}

		public void UpdatePlayerEquipment(int playerId, IDictionary<EquipmentSlot, int> equippedItems)
		{
			var playerEquipmentPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Data", "player_equipment.json");
			var playerEquipmentJson = File.ReadAllText(playerEquipmentPath);
			var playerEquipments = JsonConvert.DeserializeObject<List<PlayerEquipment>>(playerEquipmentJson) ?? new List<PlayerEquipment>();

			playerEquipments.RemoveAll(playerEquipment => playerEquipment.PlayerId == playerId);

			foreach (var item in equippedItems)
			{
				if (item.Value > 0) // Assuming 0 means no item is equipped
				{
					playerEquipments.Add(new PlayerEquipment { PlayerId = playerId, EquipmentId = item.Value });
				}
			}

			File.WriteAllText(playerEquipmentPath, JsonConvert.SerializeObject(playerEquipments, Formatting.Indented));
		}
	}
}
