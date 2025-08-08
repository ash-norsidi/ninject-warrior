using Newtonsoft.Json;
using NinjectWarrior.Models;

namespace NinjectWarrior.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly string _equipmentFilePath;
        private readonly List<Equipment> _equipment;

		public EquipmentRepository(IWebHostEnvironment webHostEnvironment)
		{
			_equipmentFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "equipment.json");
			var json = File.ReadAllText(_equipmentFilePath);
			_equipment = JsonConvert.DeserializeObject<List<Equipment>>(json) ?? [];
		}

        public IEnumerable<Equipment> GetAllEquipment()
        {
            return _equipment;
        }

		public Equipment GetEquipmentById(int id)
		{
			var equipment = _equipment.FirstOrDefault(e => e.Id == id);
			return equipment ?? throw new InvalidOperationException($"Equipment with id {id} not found.");
		}
	}
}
