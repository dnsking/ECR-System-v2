using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf.Transitions;
using Notifications.Wpf;

namespace ECR_System_v2.Utils
{
    public class UiUnits
    {
        public static void ClearText(TextBox[] mTextBoxs) {
            foreach (TextBox mTextBox in mTextBoxs)
                mTextBox.Text = "";
        }

        public static void ShowNotification(String message, NotificationType notitype)
        {
            var notificationManager = new NotificationManager();

            notificationManager.Show(new NotificationContent
            {
                Title = "ECR System",
                Message = message,
                Type = notitype
            });
        }
        public static void ShowNotification(String message)
        {
            ShowNotification(message, NotificationType.Information);
        }

        public static void AnimateSlider(Transitioner mTransitioner, int ControlIndex)
        {
            var storyboard = new Storyboard();
            var openingEffect = ((TransitionerSlide)mTransitioner.Items[ControlIndex]).OpeningEffect?.Build(((TransitionerSlide)mTransitioner.Items[ControlIndex]));
            if (openingEffect != null)
                storyboard.Children.Add(openingEffect);
            foreach (var effect in ((TransitionerSlide)mTransitioner.Items[ControlIndex]).OpeningEffects.Select(e => e.Build(((TransitionerSlide)mTransitioner.Items[ControlIndex]))).Where(tl => tl != null))
            {
                storyboard.Children.Add(effect);
            }

            storyboard.Begin(((TransitionerSlide)mTransitioner.Items[ControlIndex]));
        }
    }
}
