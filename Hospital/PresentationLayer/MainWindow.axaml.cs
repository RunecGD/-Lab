using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;

namespace HospitalApp
{
    public partial class MainWindow : Window
    {
        private List<Patient> patients = new List<Patient>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnAddPatientClick(object sender, RoutedEventArgs e)
        {
            var addPatientWindow = new AddPatientWindow();
            await addPatientWindow.ShowDialog(this);

            if (addPatientWindow.NewPatient != null)
            {
                patients.Add(addPatientWindow.NewPatient);
                LoadPatients();
            }
            else
            {
                Console.WriteLine("NewPatient is null!");
            }
        }

        private void LoadPatients()
        {
            if (PatientsList != null)
            {
                PatientsList.ItemsSource = patients.Select(p => $"{p.Name} {p.Surname}").ToList();
            }
            else
            {
                Console.WriteLine("PatientsList is null!");
            }
        }
    }
}