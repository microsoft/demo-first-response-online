using Acr.UserDialogs;
using System;
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

        public void ShowLocalNotification(string message, Action action)
        {

            UserDialogs.Instance.Toast(new ToastConfig(message)
                    .SetDuration(TimeSpan.FromSeconds(10))
                    .SetAction(x => x
                        .SetAction(action)));
        }
    }
}