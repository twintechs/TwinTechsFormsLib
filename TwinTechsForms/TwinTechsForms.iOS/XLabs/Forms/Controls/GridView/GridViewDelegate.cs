using CoreGraphics;

namespace XLabs.Forms.Controls
{
	using Foundation;
	using UIKit;

	/// <summary>
	/// Class GridViewDelegate.
	/// </summary>
	public class GridViewDelegate: UICollectionViewDelegate
	{
		/// <summary>
		/// Delegate OnItemSelected
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="indexPath">The index path.</param>
		public delegate void OnItemSelected (UICollectionView tableView, NSIndexPath indexPath);

		public delegate void OnScrolled (CGPoint contentOffset);

		/// <summary>
		/// The _on item selected
		/// </summary>
		private readonly OnItemSelected _onItemSelected;
		private readonly OnScrolled _onScrolled;


		/// <summary>
		/// Initializes a new instance of the <see cref="GridViewDelegate"/> class.
		/// </summary>
		/// <param name="onItemSelected">The on item selected.</param>
		public GridViewDelegate (OnItemSelected onItemSelected, OnScrolled onScrolled)
		{
			_onItemSelected = onItemSelected;
			_onScrolled = onScrolled;
		}

		/// <summary>
		/// Items the selected.
		/// </summary>
		/// <param name="collectionView">The collection view.</param>
		/// <param name="indexPath">The index path.</param>
		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			_onItemSelected (collectionView, indexPath);
		}

		/// <summary>
		/// Items the highlighted.
		/// </summary>
		/// <param name="collectionView">The collection view.</param>
		/// <param name="indexPath">The index path.</param>
		public override void ItemHighlighted (UICollectionView collectionView, NSIndexPath indexPath)
		{
		}

		public override void Scrolled (UIScrollView scrollView)
		{
			_onScrolled (scrollView.ContentOffset);
		}

	}
}

