using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace VioletPixiv
{
    // Extend
    public static class NavigateExtend
    {
        /// <summary>
        /// Extend : Frame.Navigate
        /// When we New a page, auto handle the BackStack and ForwardStack
        /// </summary>
        public static Task NavigateJournal(this System.Windows.Controls.Frame TargetFrame, Object Page)
        {
            // Get ForwardStackLength
            int ForwardStackLength = (TargetFrame.ForwardStack == null)? 0 : TargetFrame.ForwardStack.OfType<JournalEntry>().Count();

            // If ForwardStackLength > 0
            // The Frame will Forward to the head and then Navigate the new page.
            if (ForwardStackLength > 0)
            {
                ForwardToHead(TargetFrame);
                LoadCompletedEventHandler tmp = null;
                tmp = delegate {
                    FrameLoadCompleted(TargetFrame, EventArgs.Empty, Page);
                    TargetFrame.LoadCompleted -= tmp;
                };
                TargetFrame.LoadCompleted += tmp;

            }
            else
            {
                TargetFrame.Navigate(Page);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// When the older one is Loading Completed
        /// Load the new Page
        /// </summary>
        private static void FrameLoadCompleted(object sender, EventArgs e, Object Page, String TagString = "")
        {
            var TargetFrame = (System.Windows.Controls.Frame)sender;
            TargetFrame.Navigate(Page);
        }

        /// <summary>
        /// Navigate To Forward
        /// </summary>
        private static void ForwardToHead(this System.Windows.Controls.Frame TargetFrame)
        {
            while (TargetFrame.CanGoForward)
            {
                TargetFrame.GoForward();
            }
        }




        /// <summary>
        /// Clear content and Frame history
        /// </summary>
        public static void ClearAll(this System.Windows.Controls.Frame TargetFrame)
        {
            TargetFrame.Navigate(null);

            LoadCompletedEventHandler tmp = null;
            tmp = delegate {
                FrameLoadCompletedRemove(TargetFrame, EventArgs.Empty);
                TargetFrame.LoadCompleted -= tmp;
            };
            TargetFrame.LoadCompleted += tmp;
        }

        /// <summary>
        /// Clear Frame history
        /// </summary>
        private static void FrameLoadCompletedRemove(object sender, EventArgs e)
        {
            var TargetFrame = (System.Windows.Controls.Frame)sender;
            while (TargetFrame.CanGoBack) TargetFrame.RemoveBackEntry();
        }

    }
}
