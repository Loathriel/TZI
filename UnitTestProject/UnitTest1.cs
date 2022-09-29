using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EncryptingClasses;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow("привіт, пострижися", "туйдкх,втсфхуйійфВ")]
        public void TestEncoding(string baseMessage, string encodedMessage)
        {
            var Ceasar = new CeasarEncrypter(new UAGenerator(), 3);
            var text = Ceasar.Encrypt(baseMessage);
            Assert.IsTrue(text.Equals(encodedMessage), $"message should be {encodedMessage}, but is {text}");
        }
        [TestMethod]
        public void ProperDecoding()
        {
            var ceasar = new CeasarEncrypter(new ENGenerator(), 4);
            var message = "hi, i'm John";
            var decoded = ceasar.Decrypt(ceasar.Encrypt(message));
            Assert.IsTrue(message.Equals(decoded));
        }
    }
}
