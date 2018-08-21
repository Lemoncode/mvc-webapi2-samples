using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.Utils;

namespace Security.Validators.Tests
{
    [TestClass]
    public class HashValidatorTests
    {
        [TestMethod]
        public void ValidateHashedContent_Wrong_Content_Returns_False()
        {
            // Arrange
            string originalContent = "foo";
            string salt = new SaltGenerator().GenerateRandomSalt();
            string hashedContent = new HashGenerator().SaltedContentHash(originalContent, salt);
            var hashValidator = new HashValidator();

            // Act
            var result = hashValidator.IsValidHashedContent("f", salt, hashedContent);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateHashedContent_Wrong_Salt_Returns_False()
        {
            // Arrange
            string content = "foo";
            string originalSalt = new SaltGenerator().GenerateRandomSalt();
            string hashedContent = new HashGenerator().SaltedContentHash(content, originalSalt);
            var hashValidator = new HashValidator();

            // Act
            var result = hashValidator.IsValidHashedContent("foo", "f", hashedContent);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateHashedContent_With_Right_Inputs_Returns_False()
        {
            // Arrange
            string content = "foo";
            string salt = new SaltGenerator().GenerateRandomSalt();
            string hashedContent = new HashGenerator().SaltedContentHash(content, salt);
            var hashValidator = new HashValidator();

            // Act
            var result = hashValidator.IsValidHashedContent("foo", salt, hashedContent);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
