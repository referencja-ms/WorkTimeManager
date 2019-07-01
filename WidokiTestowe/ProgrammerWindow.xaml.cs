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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WorkTimeManager
{
    /// <summary>
    /// Interaction logic for ProgrammerWindow.xaml
    /// </summary>
    public partial class ProgrammerWindow : Window
    {
        public List<Klasa> Klasa;
        public ProgrammerWindow()
        {
            InitializeComponent();
            Klasa = new List<Klasa>();
            Klasa.Add(new Klasa("Żaneta", "Mielczarek", 21));
            Klasa.Add(new Klasa("Żanet", "Mielczarek", 21));
            dataGrid1.Items.Refresh();
        }

    }
}
