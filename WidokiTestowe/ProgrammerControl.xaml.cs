﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorkTimeManager {
    /// <summary>
    /// Interaction logic for ProgrammerControl.xaml
    /// </summary>
    public partial class ProgrammerControl : UserControl {
        public ProgrammerControl() {
            InitializeComponent();
            DataContext = new ViewModels.ProgrammerVM();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }
    }
}
