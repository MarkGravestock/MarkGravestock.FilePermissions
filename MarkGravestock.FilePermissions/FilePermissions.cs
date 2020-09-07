using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkGravestock.FilePermissions
{
    public class FilePermissions
    {
        public int ConvertToOctal(string permission)
        {
            if (permission.Length != 9)
            {
                throw new ArgumentOutOfRangeException(nameof(permission), "permission is not correct length");               
            }
            
            int total = 0;

            total = 100 * ConvertPermissionGroupToOctal(permission.Substring(0, 3));
            total = total + 10 * ConvertPermissionGroupToOctal(permission.Substring(3, 3));
            return total + ConvertPermissionGroupToOctal(permission.Substring(6, 3));
        }

        private static int ConvertPermissionGroupToOctal(string permission)
        {
            var individualPermissions = permission.ToCharArray();

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