using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTest.DesktopTests.ViewModels.TestMainWindow
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreation()
        {
            MainWindowViewModel mainWindowViewModel = new();
            Assert.IsNotNull(mainWindowViewModel.Router);
        }
    }
}