using NinjectWarrior.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace NinjectWarrior.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly string _equipmentFilePath;
        private List<Equipment> _equipment;

        public EquipmentRepository()
        {
            _equipmentFilePath = HostingEnvironment.MapPath("~/App_Data/equipment.json");
            _equipment = JsonConvert.DeserializeObject<List<Equipment>>(File.ReadAllText(_equipmentFilePath));
        }

        public IEnumerable<Equipment> GetAllEquipment()
        {
            return _equipment;
        }

        public Equipment GetEquipmentById(int id)
        {
            return _equipment.FirstOrDefault(e => e.Id == id);
        }
    }
}
