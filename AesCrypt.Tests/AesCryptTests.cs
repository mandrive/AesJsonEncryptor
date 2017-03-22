using System;
using System.Linq;
using Xunit;
using AesCrypt;

namespace AesCrypt.Tests
{
    public class AesCryptTests
    {
        [Fact]
        public void WhenNoKeyOrIvProvided_ThenKeyIsRandomAndHas32Bytes()
        {
            var aesCrypt = new AesCrypt();

            Assert.Equal(32, aesCrypt.Key.Length);
        }

        [Fact]
        public void WhenNoKeyOrIvProvided_ThenIvIsRandomAndHas16Bytes()
        {
            var aesCrypt = new AesCrypt();

            Assert.Equal(16, aesCrypt.IV.Length);
        }

        [Fact]
        public void WhenNoKeyOrIvProvided_ThenTwoAesCryptsHaveDifferentKeys()
        {
            var aesCryptOne = new AesCrypt();
            var aesCryptTwo = new AesCrypt();

            Assert.False(aesCryptOne.Key.SequenceEqual(aesCryptTwo.Key));
        }

        [Fact]
        public void WhenNoKeyOrIvProvided_ThenTwoAesCryptsHaveDifferentIvs()
        {
            var aesCryptOne = new AesCrypt();
            var aesCryptTwo = new AesCrypt();

            Assert.False(aesCryptOne.IV.SequenceEqual(aesCryptTwo.IV));
        }
    }
}
