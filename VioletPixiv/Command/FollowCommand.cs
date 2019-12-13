using Microsoft.Win32;
using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using VioletPixiv.UIObject.UserControls;

namespace VioletPixiv.Command
{
    /// <summary>
    /// When FollowButton Click
    /// </summary>
    public class FollowCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {
            // Get Button and User
            var TargetFollowButton = parameter as FollowButton;
            var TargetUser = TargetFollowButton.TargetUser as User;

            // Set to Following
            if (TargetFollowButton.IsChecked ?? false || TargetFollowButton.IsChecked == false) 
            {
                // [No await] API - Add User To Following
                _ = MainWindow.PixivWindow.AuthToken.AddUserToFollowing(TargetUser.Id);

            }
            // Set to UnFollowing
            else
            {
                // [No await] API - Delete User From Following
                _ = MainWindow.PixivWindow.AuthToken.DeleteUserFromFollowing(TargetUser.Id);
            }

        }
    }
}
