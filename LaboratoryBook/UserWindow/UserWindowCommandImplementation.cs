using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryBook.UserWindow
{
    public partial class UserWindow : Window
    {
        // Close command event handlers
        public void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        // Change password command event handlers
        public void ChangePassowrdCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(PbxNewPassword.Password)
                &&string.IsNullOrEmpty(PbxOldPassword.Password)
                &&string.IsNullOrEmpty(PbxRepeatPassword.Password))
            {
                e.CanExecute = false;
            }
            else
                e.CanExecute = true;
        }

        public async void ChangePassowrdCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (PbxNewPassword.Password!=PbxRepeatPassword.Password)
            {
                MessageBox.Show(
                    "New password and repeat password do not match",
                    "Set new password error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (PbxNewPassword.Password.Length<6)
            {
                MessageBox.Show(
                   "New password should have more than 5 symbols",
                   "Set new password error",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
                return;
            }

            var user        = this.LaboratoryBookUser;
            var password    = this.PbxOldPassword.Password;
            var newPassword = this.PbxNewPassword.Password;

            var checkOldPasswordTask = new Task<Tuple<bool, string>>(() =>Password.CheckPassword(user.GetUserID(),password));
            var checkPasswordResult = new Tuple<bool, string>(false,"null");
            try
            {
                checkOldPasswordTask.Start();
                checkPasswordResult = await checkOldPasswordTask;
            }
            catch(Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "Set new password error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            if (checkPasswordResult.Item1 == false)
            {
                MessageBox.Show(
                    "Old password does not match",
                    "Ser new password error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var setNewPasswordTask = new Task<bool>(() => Password.SetNewPassword(user.GetUserID(), newPassword));
            var setNewPasswordResult = false;
            try
            {
                setNewPasswordTask.Start();
                setNewPasswordResult = await setNewPasswordTask;
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "Set new password error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            if (setNewPasswordResult == true)
            {
                MessageBox.Show(
                   "Password was successfully changed!",
                   "Set new password error",
                   MessageBoxButton.OK,
                   MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(
                   "Password was not changed",
                   "Set new password error",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
            }
        }

        // ChangeName command event handlers
        public void ChangeNameoseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void ChangeNameCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var user = this.LaboratoryBookUser;

            var changeNameWindow = new ChangeNameWindow.ChangeNameWindow(ref user);
            changeNameWindow.Owner = this;
            changeNameWindow.ShowDialog();

            this.TbkUserName.Text = user.UserName;
        }
    }
}
