using BifrostRemoteDesktop.BusinessLogic.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BifrostRemoteDesktop.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TrueCase()
        {

            var data = "\u0002MovePointer;{\"$type\":\"BifrostRemoteDesktop.BusinessLogic.Models.Commands.MovePointerCommandArgs, BifrostRemoteDesktop\",\"TargetX\":44.9842529296875,\"TargetY\":651.9842529296875}\u0003";
            string package;

            var expectedPackage = "MovePointer;{\"$type\":\"BifrostRemoteDesktop.BusinessLogic.Models.Commands.MovePointerCommandArgs, BifrostRemoteDesktop\",\"TargetX\":44.9842529296875,\"TargetY\":651.9842529296875}";

            Assert.IsTrue(CommandReceiver.TryFindAndRemoveNextPackage(ref data, out package));

            Assert.AreEqual("", data);
            Assert.AreEqual(expectedPackage, package);

        }

        [TestMethod]
        public void FalseInputCase()
        {

            var data = "awdawdawdawdawdawd";
            string package;
            Assert.IsFalse(CommandReceiver.TryFindAndRemoveNextPackage(ref data, out package));

            Assert.AreEqual(data, "awdawdawdawdawdawd");
            Assert.AreEqual(package, string.Empty);

        }

    }
}
