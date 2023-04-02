namespace courseChien
{
    public class Parieur
    {
        private string _name;//nom du parieur
        private float _account;//solde du parieur

        public string Name
        {
            get => _name;
        }
        
        public float Account
        {
            get => _account;
            set => _account = value;
        }

        public Parieur(string name, float money)
        {
            
        }

        public void updateAccount(float credits)
        {
            _account += credits;
        }
    }
}