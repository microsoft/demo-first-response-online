using System;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);

        Task<bool> ConfirmAsync(string message, string title);

        void ShowLocalNotification(string message, Action action);
    }
}
