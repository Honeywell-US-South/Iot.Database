using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Database.Helper
{
    internal static class DbValidator
    {
        public static bool IsValidDbName(string dbName)
        {
            // Check for null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(dbName))
                return false;

            // Check for invalid file name characters
            if (dbName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
                return false;

            // Ensure the name isn't too long (Windows max: 128 chars for file name)
            if (dbName.Length > 128)
                return false;

            // Optionally: Ensure it doesn't start with a reserved prefix or is a reserved name
            string[] reservedNames = { "CON.", "PRN.", "AUX.", "NUL.", "COM1.", "COM2.", "LPT1.", "LPT2." };
            if (reservedNames.Any(r => dbName.ToUpper().StartsWith(r)))
                return false;

            return true;
        }
    }
}
