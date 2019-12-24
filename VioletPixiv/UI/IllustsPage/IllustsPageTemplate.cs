using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using VioletPixiv.UI;
using VioletPixiv.ViewModel;

namespace VioletPixiv
{

    public interface ICollectionPageTemplate
    {
        void InitLoading();
    }

    /// <summary>
    /// This will Update Data from ViewModel
    /// </summary>
    /// <typeparam name="T1">Type in UIObject/ , Which needs to download the image(s).</typeparam>
    /// <typeparam name="T2">Has List<T3> and NextUrl</typeparam>
    public abstract class CollectionPageTemplate<T1, T2> : System.Windows.Controls.Page, ICollectionPageTemplate
        where T1 : NeedToLoadImages 
        where T2 : HasNextUrl
    {

        protected const string TargetScrollViewerName = "ListViewScrollViewer"; // Get Specific name
        protected int PerLoadingImages = 6; // Declare the number of Loading Images Every times.
        protected Task CollectionLoading; // Record the Task
        protected Locker TaskLocker = new Locker(); 

        /// <summary>
        /// Get CollectionViewModel
        /// </summary>
        /// <returns> Return CollectionViewModel Object</returns>
        protected CollectionViewModelTemplate<T1, T2> _CollectionViewModel;
        public CollectionViewModelTemplate<T1, T2> CollectionViewModel
        {
            get
            {
                if(this._CollectionViewModel == null)
                {
                    this._CollectionViewModel = this.DataContext as CollectionViewModelTemplate<T1, T2>;
                }
                return this._CollectionViewModel;
            }
        }

        /// <summary>
        /// Get ScrollViewer to trigger OnScrollChanged event
        /// </summary>
        protected ScrollViewer _MainScrollViewer;
        public ScrollViewer MainScrollViewer
        {
            get
            {
                if (this._MainScrollViewer == null)
                {
                    this._MainScrollViewer = this.FindName(TargetScrollViewerName) as ScrollViewer;
                }
                return this._MainScrollViewer;
            }
        }
        
        /// <summary>
        /// Reset to init and ShowCollection
        /// </summary>
        public void InitLoading()
        {

            // init
            this.CollectionViewModel?.InitializeData();
            this.MainScrollViewer?.ScrollToTop();

            // update ListView data
            this.CollectionLoading = this.CollectionViewModel.ShowCollectionTemplate(this.PerLoadingImages);

        }

        /// <summary>
        /// Update Listview when Scroll to bottom
        /// </summary>
        protected void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var ScrollViewer = sender as ScrollViewer;
            var ScrollGap = ScrollViewer.ScrollableHeight - ScrollViewer.VerticalOffset;

            // Detect Gap > 100, Unlock
            if (ScrollGap > 100) this.TaskLocker.UnLock();

            // When   Task is Completed   and   Unlock
            if (this.CollectionLoading != null && this.CollectionLoading.IsCompleted && !this.TaskLocker.IsLock)
            {
                if (ScrollGap < 100)
                {
                    this.TaskLocker.Lock();
                    this.CollectionLoading = this.CollectionViewModel.ShowCollectionTemplate(this.PerLoadingImages);
                }
            }
        }

    }

    public class IllustsPageTemplate : CollectionPageTemplate< ArtistTemplate, IllustsList > { }

    public class UsersPageTemplate : CollectionPageTemplate< UserTemplate<UserPreviews>, UserPreviewsList > { }
   
}
