using Avalonia.Controls;
using Avalonia.Interactivity;
using BusinessLayer;

namespace HospitalApp
{
    public partial class AddPatientWindow : Window
    {
        public Patient NewPatient { get; private set; }

        public AddPatientWindow()
        {
            InitializeComponent();
        }

        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            NewPatient = new Patient
            {
                Name = FirstNameTextBox.Text,
                Surname = LastNameTextBox.Text
            };
            Close();
        }
    }
}