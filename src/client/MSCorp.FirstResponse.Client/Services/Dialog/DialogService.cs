using Acr.UserDialogs;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }
        public Task<bool> ConfirmAsync(string message, string title)
        {
            return UserDialogs.Instance.ConfirmAsync(message, title);
        }

        public void ShowLocalNotification(string message)
        {
            UserDialogs.Instance.Toast(message);
        }
    }
}