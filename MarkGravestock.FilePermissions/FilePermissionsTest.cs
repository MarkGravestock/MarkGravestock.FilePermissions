using System;
using Xunit;

namespace MarkGravestock.FilePermissions
{
    public class FilePermissionsTest
    {
        private readonly FilePermissions sut = new FilePermissions();
        
        [Theory]
        [InlineData(700, "rwx------")]
        [InlineData(777, "rwxrwxrwx")]
        [InlineData(742, "rwxr---w-")]
        public void can_convert_groups_octal(int octal, string permissions)
        {
            Assert.Equal(octal, sut.ConvertToOctal(permissions));
        }
        
        [Fact]
        public void it_throws_for_too_long_a_string()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.ConvertToOctal("rwxrwxrwxrwx"));
        }
        
        [Fact]
        public void it_throws_for_too_short_a_string()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.ConvertToOctal("rw"));
        }

        [Fact]
        public void it_throws_for_unexpected_character()
        {
            Assert.Throws<ArgumentException>(() => sut.ConvertToOctal("rwxrwxrwy"));
        }
    }
}