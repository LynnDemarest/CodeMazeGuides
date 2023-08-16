using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeMazeTest_ActionFuncDelegates;
using static System.Net.Mime.MediaTypeNames;

namespace DelegateTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CountTokens_NowIsTheTime_()
        {
            // Arrange 
            string text = "Now is the time for all good men to come to the aid of their country.";
            // Act
            int result = CodeMazeTest_ActionFuncDelegates.Helpers.CountTokens(text);
            // Assert
            Assert.AreEqual(result, 22);
        }
    }
}