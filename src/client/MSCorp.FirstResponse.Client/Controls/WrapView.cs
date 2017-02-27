using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Controls
{
    public class WrapView : Layout<View>
    {
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create("Orientation", typeof(StackOrientation), typeof(WrapView), StackOrientation.Vertical,
                BindingMode.TwoWay, propertyChanged: OnSizeChanged);

        public StackOrientation Orientation
        {
            get { return (StackOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create("Spacing", typeof(double), typeof(WrapView), default(double),
                BindingMode.TwoWay, propertyChanged: OnSizeChanged);

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(WrapView), null,
                BindingMode.TwoWay, propertyChanged: OnSizeChanged);

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(WrapView), default(IEnumerable),
                BindingMode.TwoWay, propertyChanged: OnSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemClickCommandProperty =
            BindableProperty.Create("ItemClickCommand", typeof(ICommand), typeof(WrapView), default(ICommand));

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var wrapView = (WrapView)bindable;

            if (oldValue != null)
            {
                var observableCollection = oldValue as INotifyCollectionChanged;

                // Unsubscribe from CollectionChanged on the old collection
                if (observableCollection != null)
                    observableCollection.CollectionChanged -= OnCollectionChanged;
            }

            if (newValue != null)
            {
                var observableCollection = newValue as INotifyCollectionChanged;

                // Subscribe to CollectionChanged on the new collection 
                //and fire the CollectionChanged event to handle the items in the new collection
                if (observableCollection != null)
                    observableCollection.CollectionChanged += OnCollectionChanged;

                wrapView.Children.Clear();
                wrapView.AddItems((IEnumerable)newValue);
            }
        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            var wrapView = (WrapView)sender;
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    wrapView.Children.Clear();
                    break;

                case NotifyCollectionChangedAction.Add:
                    wrapView.AddItems(args.NewItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    wrapView.RemoveItems(args.OldItems);
                    break;
            }
        }

        private void RemoveItems(IEnumerable items)
        {
            foreach (object item in items)
            {
                var child = Children.FirstOrDefault(c => c.BindingContext == item);
                if (child != null)
                    Children.Remove(child);
            }
        }

        private void AddItems(IEnumerable items)
        {
            foreach (object item in items)
            {
                var child = CreateViewFor(item);
                if (child == null)
                    return;

                child.BindingContext = item;
                Children.Add(child);
            }
        }

        /// <summary>
        /// Creates a View for the type of item passed
        /// </summary>
        /// <param name="item">Item to bind to the view</param>
        /// <returns>The view created</returns>
        protected virtual View CreateViewFor(object item)
        {
            var template = GetTemplateFor(item.GetType());
            var content = template.CreateContent();

            if (!(content is View) && !(content is ViewCell))
                throw new Exception();

            var view = (content is View) ? content as View : ((ViewCell)content).View;
            view.BindingContext = item;
               
            //May add this support in later
            view.GestureRecognizers.Add(
                new TapGestureRecognizer { Command = ItemClickCommand, CommandParameter = item });

            return view;
        }

        /// <summary>
        /// Get's a DataTemplate for the Type passed
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The DataTemplate for the type</returns> 
        protected virtual DataTemplate GetTemplateFor(Type type)
        {
            var template = ItemTemplate;

            return template;
        }

        /// <summary>
        /// Called when the spacing or orientation properties are changed - it forces
        /// the control to go back through a layout pass.
        /// </summary>
        private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var wrapView = (WrapView)bindable;
            wrapView.ForceLayout();
        }

        /// <summary>
        /// Called during the measure pass of a layout cycle to get the desired size of an element.
        /// </summary>
        /// <param name="widthConstraint">The available width for the element to use.</param>
        /// <param name="heightConstraint">The available height for the element to use.</param>
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            if (WidthRequest > 0)
                widthConstraint = Math.Min(widthConstraint, WidthRequest);
            if (HeightRequest > 0)
                heightConstraint = Math.Min(heightConstraint, HeightRequest);

            double internalWidth = double.IsPositiveInfinity(widthConstraint) ? double.PositiveInfinity : Math.Max(0, widthConstraint);
            double internalHeight = double.IsPositiveInfinity(heightConstraint) ? double.PositiveInfinity : Math.Max(0, heightConstraint);

            return Orientation == StackOrientation.Vertical
                ? DoVerticalMeasure(internalWidth, internalHeight)
                    : DoHorizontalMeasure(internalWidth, internalHeight);

        }

        private SizeRequest DoVerticalMeasure(double widthConstraint, double heightConstraint)
        {
            int columnCount = 1;

            double width = 0;
            double height = 0;
            double minWidth = 0;
            double minHeight = 0;
            double heightUsed = 0;

            foreach (var item in Children)
            {
                var size = item.GetSizeRequest(widthConstraint, heightConstraint);
                width = Math.Max(width, size.Request.Width);

                var newHeight = height + size.Request.Height + Spacing;
                if (newHeight > heightConstraint)
                {
                    columnCount++;
                    heightUsed = Math.Max(height, heightUsed);
                    height = size.Request.Height;
                }
                else
                    height = newHeight;

                minHeight = Math.Max(minHeight, size.Minimum.Height);
                minWidth = Math.Max(minWidth, size.Minimum.Width);
            }

            if (columnCount > 1)
            {
                height = Math.Max(height, heightUsed);
                width *= columnCount;  // take max width
            }

            return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
        }

        /// <summary>
        /// Does the horizontal measure.
        /// </summary>
        /// <returns>The horizontal measure.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        private SizeRequest DoHorizontalMeasure(double widthConstraint, double heightConstraint)
        {
            int rowCount = 1;

            double width = 0;
            double height = 0;
            double minWidth = 0;
            double minHeight = 0;
            double widthUsed = 0;

            foreach (var item in Children)
            {
                var size = item.GetSizeRequest(widthConstraint, heightConstraint);
                height = Math.Max(height, size.Request.Height);

                var newWidth = width + size.Request.Width + Spacing;
                if (newWidth > widthConstraint)
                {
                    rowCount++;
                    widthUsed = Math.Max(width, widthUsed);
                    width = size.Request.Width;
                }
                else
                    width = newWidth;

                minHeight = Math.Max(minHeight, size.Minimum.Height);
                minWidth = Math.Max(minWidth, size.Minimum.Width);
            }

            if (rowCount > 1)
            {
                width = Math.Max(width, widthUsed);
                height = (height + Spacing) * rowCount - Spacing;
            }

            return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
        }

        /// <summary>
        /// Positions and sizes the children of a Layout.
        /// </summary>
        /// <param name="x">A value representing the x coordinate of the child region bounding box.</param>
        /// <param name="y">A value representing the y coordinate of the child region bounding box.</param>
        /// <param name="width">A value representing the width of the child region bounding box.</param>
        /// <param name="height">A value representing the height of the child region bounding box.</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (Orientation == StackOrientation.Vertical)
            {
                double colWidth = 0;
                double yPos = y, xPos = x;

                foreach (var child in Children.Where(c => c.IsVisible))
                {
                    var request = child.GetSizeRequest(width, height);

                    double childWidth = request.Request.Width;
                    double childHeight = request.Request.Height;
                    colWidth = Math.Max(colWidth, childWidth);

                    if (yPos + childHeight > height)
                    {
                        yPos = y;
                        xPos += colWidth + Spacing;
                        colWidth = 0;
                    }

                    var region = new Rectangle(xPos, yPos, childWidth, childHeight);
                    LayoutChildIntoBoundingRegion(child, region);
                    yPos += region.Height + Spacing;
                }
            }
            else
            {
                double rowHeight = 0;
                double yPos = y, xPos = x;

                foreach (var child in Children.Where(c => c.IsVisible))
                {
                    var request = child.GetSizeRequest(width, height);

                    double childWidth = request.Request.Width;
                    double childHeight = request.Request.Height;
                    rowHeight = Math.Max(rowHeight, childHeight);

                    if (xPos + childWidth > width)
                    {
                        xPos = x;
                        yPos += rowHeight + Spacing;
                        rowHeight = 0;
                    }

                    var region = new Rectangle(xPos, yPos, childWidth, childHeight);
                    LayoutChildIntoBoundingRegion(child, region);
                    xPos += region.Width + Spacing;
                }

            }
        }
    }
}