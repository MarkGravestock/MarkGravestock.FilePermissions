using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkGravestock.FilePermissions
{
    public class FilePermissions
    {
        private const int PermissionGroupLength = 3;

        public int ConvertToOctal(string permission)
        {
            if (permission.Length != 9)
            {
                throw new ArgumentOutOfRangeException(nameof(permission), "permission is not correct length");
            }

            int total = 0;

            var permissionGroups = SplitGroups(permission, PermissionGroupLength).Reverse().ToList();

            for (int i = 0; i < permissionGroups.Count(); i++)
            {
                total = total + ConvertPermissionGroupToOctalDigit(permissionGroups[i]) * ((int) Math.Pow(10, i));
            }

            return total;
        }

        private IEnumerable<string> SplitGroups(string str, int len)
        {
            return Enumerable.Range(0, str.Length / len).Select(x => str.Substring(x * len, len));
        }
    

        private static int ConvertPermissionGroupToOctalDigit(string permissionGroup)
        {
            var individualPermissions = permissionGroup.ToCharArray();

            return individualPermissions.Aggregate(0, (accumulator, perm) => accumulator + ConvertPermissionToOctal(perm));
        }

        private static int ConvertPermissionToOctal(char perm)
        {
            IDictionary<char, int> permissionToOctal = new Dictionary<char, int> {{'r', 4}, {'w', 2}, {'x', 1}, {'-', 0}};

            if (permissionToOctal.ContainsKey(perm) == false)
            {
                throw new ArgumentException($"Invalid permission: {perm}");
            }
            
            return permissionToOctal[perm];
        }
    }
}