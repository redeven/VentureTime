using Lumina.Excel.GeneratedSheets;
using System.Collections.Generic;
using System.Linq;

namespace VentureTime.Utils
{
    internal class Worlds
    {
        private static Dictionary<uint, string>? _worldNames;

        internal static void GetWorlds()
        {
            var _sheet = Svc.GameData.GetExcelSheet<World>()!;
            _worldNames = _sheet.Where(IsValid)
                .ToDictionary(w => w.RowId, w => w.Name.RawString);
        }

        private static bool IsValid(World world)
        {
            if (world.Name.RawData.IsEmpty)
                return false;

            if (world.DataCenter.Row is 0)
                return false;

            if (world.IsPublic)
                return true;

            return char.IsUpper((char)world.Name.RawData[0]);
        }

        internal static bool IsValidWorldId(uint id)
            => _worldNames!.ContainsKey(id);
    }
}
