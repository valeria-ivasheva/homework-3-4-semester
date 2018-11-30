using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe;

namespace GameTest
{
    [TestClass]
    public class GameClassTest
    {
        private Game game;

        [TestInitialize]
        public void Initialize()
        {
            game = new Game("x");
        }

        [TestMethod]
        public void RightInitializeTest()
        {
            Assert.AreEqual("x", game.UserOne());
        }

        [TestMethod]
        public void RightInputTest()
        {
            game.InputElement(0);
            game.InputElement(1);
            game.InputElement(2);
            game.InputElement(6);
            game.InputElement(8);
            game.InputElement(8);
            game.InputElement(9);
            game.InputElement(-1);
            Assert.AreEqual("xoxzzzozx", game.NowField());
        }

        [TestMethod]
        public void ResetTest()
        {
            game.Reset();
            Assert.AreEqual("zzzzzzzzz", game.NowField());
            Assert.AreEqual("", game.UserOne());
        }

        [TestMethod]
        public void UserOneWinTest()
        {
            game.InputElement(0);
            game.InputElement(3);
            game.InputElement(2);
            game.InputElement(5);
            game.InputElement(1);
            Assert.IsTrue(game.IsUserOneWin());
        }

        [TestMethod]
        public void NobodyWonTest()
        {
            game.Reset();
            game = new Game("o");
            game.InputElement(4);
            game.InputElement(1);
            game.InputElement(0);
            game.InputElement(5);
            game.InputElement(2);
            game.InputElement(3);
            game.InputElement(8);
            game.InputElement(6);
            game.InputElement(7);
            Assert.IsFalse(game.IsUserOneWin());
            Assert.IsFalse(game.IsUserTwoWin());
        }
    }
}
