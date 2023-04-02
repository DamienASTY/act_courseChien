namespace courseChien
{
    public class Pari
    {
        private Parieur _parieur;
        private rb18 _dog;
        private float _credit;

        public Parieur Parieur
        {
            get => _parieur;
            set => _parieur = value;
        }
        
        public rb18 Dog
        {
            get => _dog;
            set => _dog = value;
        }

        public float Credit
        {
            get => _credit;
        }

        public Pari(Parieur parieur, rb18 dog, float credit)
        {
            _parieur = parieur;
            _dog = dog;
            _credit = credit;
        }
    }
}