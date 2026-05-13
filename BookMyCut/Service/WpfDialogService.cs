using System.Windows;

namespace BookMyCut.Services
{
    public class WpfDialogService : IDialogService
    {
        public void AfficherMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
