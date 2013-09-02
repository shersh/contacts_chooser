using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Acceron.Core.Controls
{
    [TemplatePart(Name = ElementHeader, Type = typeof(ContentControl))]
    [TemplatePart(Name = SelectedList, Type = typeof(ListBox))]
    [TemplatePart(Name = SearchTextBox, Type = typeof(TextBox))]
    public class ContactsChooser : Control
    {
        #region Consts
        private const string ElementHeader = "Header";

        private const string SelectedList = "ItemsListBox";

        private const string SearchTextBox = "SearchTextBox";
        #endregion

        #region Controls refernces
        private ListBox _listBox;
        private TextBox _completeBox;
        private ContentControl _header;
        #endregion


        /// <summary>
        /// // Used to listen for changes in the ItemsSource 
        /// (_rootCollection = ItemsSource as INotifyCollectionChanged).
        /// </summary>
        private INotifyCollectionChanged _rootCollection;

        /// <summary>
        /// Ctor
        /// </summary>
        public ContactsChooser()
        {
            DefaultStyleKey = typeof(ContactsChooser);
            this.Loaded += OnLoaded;
        }

        void ContactsChooser_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_completeBox != null)
                _completeBox.Focus();
        }

        #region Dependency properties

        #region Templates


        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(ContactsChooser), new PropertyMetadata(null, HeaderTemplateChanged));

        private static void HeaderTemplateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as ContactsChooser;

            if (control != null && control._header != null)
            {
                control._header.ContentTemplate =
                (DataTemplate)dependencyPropertyChangedEventArgs.NewValue;
            }
        }

        public DataTemplate PopupItemTemplate
        {
            get { return (DataTemplate)GetValue(PopupItemTemplateProperty); }
            set { SetValue(PopupItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupItemTemplateProperty =
            DependencyProperty.Register("PopupItemTemplate", typeof(DataTemplate), typeof(ContactsChooser), new PropertyMetadata(null, CompleteItemTemplateChanged));

        private static void CompleteItemTemplateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var lBox = dependencyObject as ListBox;
            if (lBox == null)
                return;

            lBox.ItemTemplate = (DataTemplate)dependencyPropertyChangedEventArgs.NewValue;
        }

        public Brush PopupBackground
        {
            get { return (Brush)GetValue(PopupBackgroundProperty); }
            set { SetValue(PopupBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupBackgroundProperty =
            DependencyProperty.Register("PopupBackground", typeof(Brush), typeof(ContactsChooser), new PropertyMetadata(new SolidColorBrush(Colors.White)));



        public Brush PopupBorderBrush
        {
            get { return (Brush)GetValue(PopupBorderBrushProperty); }
            set { SetValue(PopupBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupBorderBrushProperty =
            DependencyProperty.Register("PopupBorderBrush", typeof(Brush), typeof(ContactsChooser), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public Thickness PopupBorderThickness
        {
            get { return (Thickness)GetValue(PopupBorderThicknessProperty); }
            set { SetValue(PopupBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupBorderThicknessProperty =
            DependencyProperty.Register("PopupBorderThickness", typeof(Thickness), typeof(ContactsChooser), new PropertyMetadata(new Thickness(1, 1, 1, 1)));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ContactsChooser), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dObject, DependencyPropertyChangedEventArgs e)
        {
            var lBox = dObject as ListBox;
            if (lBox == null)
                return;

            lBox.ItemTemplate = (DataTemplate)e.NewValue;
        }

        #endregion

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(ContactsChooser), new PropertyMetadata(null));


        public IEnumerable SearchItemsSource
        {
            get { return (IEnumerable)GetValue(SearchItemsSourceProperty); }
            set
            {
                SetValue(SearchItemsSourceProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for SearchItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchItemsSourceProperty =
            DependencyProperty.Register("SearchItemsSource", typeof(IEnumerable), typeof(ContactsChooser), new PropertyMetadata(null, (
                o, args) =>
            {
                var chooser = ((ContactsChooser)o);
                var newVal = args.NewValue as IEnumerable;
                if (newVal != null)
                {

                }

                if (chooser._popupListBox != null)
                    chooser._popupListBox.ItemsSource = newVal;
            }));

        /// <summary>
        /// The DataSource property. Where all of the items come from.
        /// </summary>
        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// The DataSource DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList), typeof(ContactsChooser), new PropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((ContactsChooser)obj).OnItemsSourceChanged();
        }

        /// <summary>
        /// Header text. For example "To:".
        /// </summary>
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(ContactsChooser), new PropertyMetadata("Header"));

        private ListBox _popupListBox;
        private bool _isFocusFromPopup;

        #endregion

        private void OnItemsSourceChanged()
        {
            // Reload the whole list.
            LoadDataIntoListBox();
        }

        private void LoadDataIntoListBox()
        {
            if (_listBox != null)
            {
                UnsubscribeFromAllCollections();

                _rootCollection = ItemsSource as INotifyCollectionChanged;

                if (_rootCollection != null)
                {
                    _rootCollection.CollectionChanged += OnCollectionChanged;
                }

                if (ItemsSource != null)
                    foreach (var item in ItemsSource)
                    {
                        _listBox.Items.Insert(_listBox.Items.Count - 1, item);
                    }
            }
        }

        /// <summary>
        /// Called when there is a change in the root or a group collection.
        /// </summary>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (sender == _rootCollection)
                    {
                        foreach (var newItem in e.NewItems)
                        {
                            _listBox.Items.Insert(_listBox.Items.Count - 1, newItem);
                            _listBox.ScrollIntoView(_listBox.Items.Last());
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (sender == _rootCollection)
                    {
                        foreach (var oldItem in e.OldItems)
                        {
                            _listBox.Items.Remove(oldItem);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    LoadDataIntoListBox();
                    break;
            }
        }



        /// <summary>
        /// Unsubscrives from every collection in ItemsSource.
        /// </summary>
        private void UnsubscribeFromAllCollections()
        {
            if (_rootCollection != null)
            {
                _rootCollection.CollectionChanged -= OnCollectionChanged;
            }

            while (_listBox != null && _listBox.Items.Count > 2)
                _listBox.Items.RemoveAt(1);

            _rootCollection = null;
        }

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _listBox = this.GetTemplateChild(SelectedList) as ListBox;

            _completeBox = this.GetTemplateChild(SearchTextBox) as TextBox;


            _header = this.GetTemplateChild(ElementHeader) as ContentControl;
            _popupListBox = this.GetTemplateChild("Selector") as ListBox;
            if (_popupListBox != null) _popupListBox.SelectionChanged += PopupListBoxOnSelectionChanged;

            if (_completeBox != null)
            {
                _completeBox.TextChanged += CompleteBoxOnTextChanged;
                _completeBox.KeyDown += CompleteBoxOnKeyDown;
                _completeBox.LostFocus += CompleteBoxOnLostFocus;
            }

            LoadDataIntoListBox();
        }

        private void CompleteBoxOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_isFocusFromPopup)
            {
                _listBox.ScrollIntoView(_listBox.Items.Last());
                _completeBox.Focus();
                _isFocusFromPopup = false;
            }
        }

        private void CompleteBoxOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Back && string.IsNullOrEmpty(_completeBox.Text))
            {
                TryDeleteLastItem();
            }
        }

        private void TryDeleteLastItem()
        {
            if (ItemsSource.Count > 0)
                ItemsSource.RemoveAt(ItemsSource.Count - 1);
        }

        private void PopupListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (_popupListBox.SelectedItem != null)
            {
                if (CheckExist != null)
                    if (!CheckExist(ItemsSource, _popupListBox.SelectedItem))
                        ItemsSource.Add(_popupListBox.SelectedItem);
                    else
                    {

                    }
                else
                {
                    ItemsSource.Add(_popupListBox.SelectedItem);
                }

                _completeBox.Text = "";
                _popupListBox.SelectedItem = null;
                var e = _popupListBox.ItemsSource as IList;
                if (e != null) e.Clear();
            }

            _isFocusFromPopup = true;
        }

        /// <summary>
        /// Check function for existing item in selected items 
        /// </summary>
        public Func<IList, object, bool> CheckExist { get; set; }

        private void CompleteBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (SearchCommand != null && !string.IsNullOrEmpty(_completeBox.Text))
            {
                SearchCommand.Execute(_completeBox.Text);
            }

            if (string.IsNullOrEmpty(_completeBox.Text))
            {
            }
        }

        #endregion

    }
}
