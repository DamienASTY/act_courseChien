using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace courseChien
{
    public partial class MainWindow
    {
        
        Parieur[] parieurs = new Parieur[3];
        rb18[] chiens = new rb18[4];
        Pari[] paris = new Pari[3];

        
        int CurrentPersonneSelected;
        bool IsRaceStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            InitParieur();
            InitRb18();
            InitPari();
            InitializeInterface();
        }
        
        private void InitParieur()
        {
            parieurs[0] = new Parieur("Fernando", 50);
            parieurs[1] = new Parieur("Max", 75);
            parieurs[2] = new Parieur("Lewis", 45);
        }
        
        private void InitRb18()
        {
            for (int i = 0; i < chiens.Length; i++)
            {
                chiens[i] = new rb18(i, new Image());
            }
        }
        
        private void SafetyDog()
        {
            for (int i = 0; i < chiens.Length; i++)
            {
                CanvasChiens.Children.Remove(chiens[i].img);
                chiens[i].img.Source = null ;
            }
        }
        
        private void InitPari()
        {
            for (int i = 0; i < paris.Length; i++)
            {
                paris[i] = new Pari(new Parieur("", 0), new rb18(0, new Image()), 0);
            }
        }
        
        private void InitializeInterface()
        {
            BitmapImage imageChien = new BitmapImage();
            imageChien.BeginInit();
            imageChien.UriSource = new Uri("/rb18.png", UriKind.Relative);
            imageChien.EndInit();

            for(int i = 0; i < chiens.Length; i++)
            {
                Image image = new Image();
                image.Source = imageChien;
                image.Height = 35;
                image.Width = 121;
                Canvas.SetTop(image, 58 * i);
                chiens[i].img = image;

                CanvasChiens.Children.Add(image);
            }
            PersonneName.Text = parieurs[0].Name;

            UpdateParieursPanel();
            UpdateParieursState();

        }

        private void UpdateParieursState()
        {
            for (int i = 0; i < parieurs.Length; i++)
            {
                TextBlock textBlockPersonne = ParieursState.Children[i] as TextBlock;

                if (textBlockPersonne != null)
                {

                    Pari pariPersonne = paris.FirstOrDefault(pari => pari.Parieur == parieurs[i]);
                    if (pariPersonne != null)
                    {
                        textBlockPersonne.Text = parieurs[i].Name + " a parié " + pariPersonne.Credit + " écus sur le chiens numéro " + (pariPersonne.Dog.Rank + 1);
                    }
                    else
                    {
                        textBlockPersonne.Text = parieurs[i].Name + " n'a pas encore parié";
                    }
                    
                }
            }
        }

        private void UpdateParieursPanel()
        {
            string ecus = " écus";
            for (int i = 0; i < parieurs.Length; i++)
            {
                RadioButton radioButton = ParieursPanel.Children[i] as RadioButton;

                // Utilisez le RadioButton sélectionné pour effectuer des opérations
                if (radioButton != null)
                {
                    if (parieurs[i].Account <= 1)
                    {
                        ecus = " écu";
                    }
                    radioButton.Content = parieurs[i].Name + " possède " + parieurs[i].Account + ecus;
                }
            }
        }
        
        private void GenerateRace()
        {
            Random random = new Random();
            int WinnerDog = random.Next(0, chiens.Length - 1);
            rb18 ChienCache = chiens[WinnerDog];
            chiens[WinnerDog] = chiens[0];
            chiens[0] = ChienCache;

            MoveChien(chiens[0], 700);

            for (int i = 1; i < chiens.Length; i++)
            {
                int randomDistance = random.Next(200, 650);
                MoveChien(chiens[i], randomDistance);
            }
            RemoveEcuPersonne();
            EndRace(WinnerDog);
        }
        
        private void MoveChien(rb18 chien, int maxDistance)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = maxDistance;
            animation.Duration = TimeSpan.FromSeconds(10);
            animation.AccelerationRatio = 0.2;
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, chien.img);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
            storyboard.Begin();
        }
        
        private void UpdateEcusFromPersonne(int WinnerIndex)
        {
            for(int i = 0; i < parieurs.Length; i++)
            {
                if (paris[i].Dog.Rank == WinnerIndex)
                {
                    parieurs[i].updateAccount((paris[i].Credit * 2));
                }
            }
        }
        
        private void MaskNumericInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumeric(e.Text))
            {
                if(!(int.Parse(ecusNombre.Text + e.Text) <= 500))
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }
        
        private void MaskNumericInputChien(object sender, TextCompositionEventArgs e)
        {
            if (chienNombre.Text.Length >= 1)
            {
                e.Handled= true;
            }
            else
            {
                if (TextIsNumeric(e.Text))
                {
                    if (!(int.Parse(e.Text) >= 1 && int.Parse(e.Text) <= chiens.Length))
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = !TextIsNumeric(e.Text);
                }
            }
        }
        
        private void MaskNumericPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string input = (string)e.DataObject.GetData(typeof(string));
                if (!TextIsNumeric(input)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool TextIsNumeric(string input)
        {
            return input.All(c => Char.IsDigit(c) || Char.IsControl(c));
        }
        
        private void StartRaceButton_Click(object sender, RoutedEventArgs e)
        {

            if (!IsRaceStarted)
            {
                bool canStartRace = true;

                for (int i = 0; i < parieurs.Length; i++)
                {
                    if (paris[i].Parieur.Name == "")
                    {
                        canStartRace = false;
                    }
                }

                if (canStartRace)
                {
                    IsRaceStarted = true;

                    // désactiver les boutons
                    pariButton.IsEnabled = false;
                    StartRaceButton.IsEnabled = false;
                    ResetButton.IsEnabled = false;

                    GenerateRace();
                }
            }
        }
        
        private void RemoveEcuPersonne()
        {
            for(int i = 0; i < parieurs.Length; i++)
            {
                parieurs[i].updateAccount(- paris[i].Credit);
            }

            UpdateParieursPanel();
        }
        
        private async void EndRace(int winner)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            pariButton.IsEnabled = true;
            StartRaceButton.IsEnabled = true;
            ResetButton.IsEnabled = true;

            UpdateEcusFromPersonne(winner);
            UpdateParieursPanel();
            InitPari();
            UpdateParieursState();
            IsRaceStarted = false;
        }
        
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            InitPari();
            InitParieur();
            SafetyDog();
            InitRb18();
            InitializeInterface();

            ecusNombre.Text = null;
            chienNombre.Text = null;
        }
        
        private void pariButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRaceStarted)
            {
                if (IsNumeric(ecusNombre.Text))
                {
                    int ecus = int.Parse(ecusNombre.Text);
                    if (ecus >= 5 && parieurs[CurrentPersonneSelected].Account - ecus >= 0)
                    {
                        if (IsNumeric(chienNombre.Text))
                        {
                            int numeroChien = int.Parse(chienNombre.Text);

                            if (numeroChien >= 1 && numeroChien <= chiens.Length)
                            {
                                paris[CurrentPersonneSelected] = new Pari(parieurs[CurrentPersonneSelected], chiens[numeroChien - 1], ecus);
                                UpdateParieursState();

                            }
                        }
                    }

                }
            }
        }
        
        private void RadioBob_Click(object sender, RoutedEventArgs e)
        {
            int PersonneIndex = ParieursPanel.Children.IndexOf((UIElement)sender);
            CurrentPersonneSelected = PersonneIndex;
            PersonneName.Text = parieurs[CurrentPersonneSelected].Name;
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out int _);
        }
    }
}