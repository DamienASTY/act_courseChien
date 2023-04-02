using System;
using System.Windows.Controls;

namespace courseChien
{
    //la classe chien, mais ils sont tellement rapide que 
    //même Max Verstapeen ne peut pas les rattraper !
    public class rb18
    {
        private int _rank;
        private Image _img;
        private int _pos;
        
        public void Run()
        {
            Random random = new Random();
            _pos += random.Next(0, 5);
        }
        
        public int Rank
        {
            get => _rank;
        }

        public int Pos
        {
            get => _pos;
        }

        public Image img
        {
            get => _img;
            set => _img = value;
        }

        public rb18(int rank, Image img)
        {
            _rank = rank;
            _img = img;
        }
    }
}