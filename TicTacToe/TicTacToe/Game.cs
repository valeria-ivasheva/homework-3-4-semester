using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Game
    {
        private string userOne;
        private string userTwo;
        private string field;
        private int move = 0;

        public Game(string user)
        {
            userOne = user;
            userTwo = (user == "x") ? "o" : "x";
            field = "zzzzzzzzz";
        }

        public void InputElement(int index)
        {
            if (index < 0 || index > (field.Length - 1) || field[index] != 'z')
            {
                return;
            }
            move++;
            if (move % 2 == 1)
            {
                field = field.Substring(0, index) + userOne + field.Substring(index + 1);
            }
            else
            {
                field = field.Substring(0, index) + userTwo + field.Substring(index + 1);
            }
        }

        public void Reset()
        {
            userOne = "";
            userTwo = "";
            field = "zzzzzzzzz";
        }

        private bool GameEndWithWin(string temp)
        {
            bool result = false;
            for (int i = 0; i < 3; i++)
            {
                string tempStr = field.Substring(i * 3, 3);
                if (tempStr == temp)
                {
                    result = true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                string tempStr = field.Substring(i, 1) + field.Substring(i + 3, 1) + field.Substring(i + 6, 1);
                if (tempStr == temp)
                {
                    result = true;
                }
            }
            string glDiag = field.Substring(0, 1) + field.Substring(4, 1) + field.Substring(8, 1);
            string pbDiag = field.Substring(2, 1) + field.Substring(4, 1) + field.Substring(6, 1);
            if (glDiag == temp || pbDiag == temp)
            {
                result = true;
            }
            return result;
        }

        public bool IsUserOneWin()
        {
            string temp = (userOne == "x") ? "xxx" : "ooo";
            var result = GameEndWithWin(temp);
            return result;
        }

        public bool IsUserTwoWin()
        {
            string temp = (userOne == "x") ? "ooo" : "xxx";
            var result = GameEndWithWin(temp);
            return result;
        }

        public string UserOne()
        {
            return userOne;
        }

        public string NowField()
        {
            return field;
        }
    }
}
