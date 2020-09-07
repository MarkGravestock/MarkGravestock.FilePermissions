using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MarkGravestock.FilePermissions
{
    public class FilePermissions
    {
        public int ConvertToOctal(string permission)
        {
            if (permission.Length != 9)
            {
                throw new ArgumentOutOfRangeException("permission", "permission is not correct length");               
            }
            
            int total = 0;

            total = 100 * ConvertPermissionGroupToOctal(permission.Substring(0, 3));
            total = total + 10 * ConvertPermissionGroupToOctal(permission.Substring(3, 3));
            return total + ConvertPermissionGroupToOctal(permission.Substring(6, 3));
        }

        public static int ConvertPermissionGroupToOctal(string permission)
        {
            var individualPermissions = permission.ToCharArray().Select(x => new string(x, 1));

            return individualPermissions.Aggregate(0, (accumulator, perm) => accumulator + ConvertPermissionToOctal(perm));
        }

        public static int ConvertPermissionToOctal(string perm)
        {
            IDictionary<string, int> permissionToOctal = new Dictionary<string, int> {{"r", 4}, {"w", 2}, {"x", 1}, {"-", 0}};

            if (permissionToOctal.ContainsKey(perm) == false)
            {
                throw new ArgumentException($"Invalid permission: {perm}");
            }
            
            return permissionToOctal[perm];
        }
    }

    public class FilePermissionsTest
    {
        private readonly FilePermissions filePermissions = new FilePermissions();

        [Theory]
        [InlineData(4, "r")]
        [InlineData(2, "w")]
        [InlineData(1, "x")]
        [InlineData(0, "-")]
        public void can_convert_single_character_to_octal(int octal, string permissions)
        {
            Assert.Equal(octal, FilePermissions.ConvertPermissionToOctal(permissions));
        }

        [Theory]
        [InlineData(7, "rwx")]
        [InlineData(0, "---")]
        public void can_convert_multiple_characters_to_octal(int octal, string permissions)
        {
            Assert.Equal(octal, FilePermissions.ConvertPermissionGroupToOctal(permissions));
        }

        [Theory]
        [InlineData(700, "rwx------")]
        [InlineData(777, "rwxrwxrwx")]
        public void can_convert_groups_octal(int octal, string permissions)
        {
            Assert.Equal(octal, filePermissions.ConvertToOctal(permissions));
        }
        
        [Fact]
        public void it_throws_for_too_long_a_string()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => filePermissions.ConvertToOctal("rwxrwxrwxrwx"));
        }
        
        [Fact]
        public void it_throws_for_too_short_a_string()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => filePermissions.ConvertToOctal("rw"));
        }

        [Fact]
        public void it_throws_for_unexpected_character()
        {
            Assert.Throws<ArgumentException>(() => filePermissions.ConvertToOctal("rwxrwxrwy"));
        }
    }
}