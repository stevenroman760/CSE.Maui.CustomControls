﻿using CSE.Maui.CustomControls.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE.Maui.CustomControls.Controls
{
    public class TreeView : ScrollView
    {
        private readonly StackLayout _StackLayout = new StackLayout { Orientation = StackOrientation.Vertical };

        //TODO: This initialises the list, but there is nothing listening to INotifyCollectionChanged so no nodes will get rendered
        private IList<TreeViewNode> _RootNodes = new ObservableCollection<TreeViewNode>();
        private TreeViewNode _SelectedItem;

        /// <summary>
        /// The item that is selected in the tree
        /// TODO: Make this two way - and maybe eventually a bindable property
        /// </summary>
        public TreeViewNode SelectedItem
        {
            get => _SelectedItem;

            set
            {
                if (_SelectedItem == value)
                {
                    return;
                }

                if (_SelectedItem != null)
                {
                    _SelectedItem.IsSelected = false;
                }

                _SelectedItem = value;

                SelectedItemChanged?.Invoke(this, new EventArgs());
            }
        }


        public IList<TreeViewNode> RootNodes
        {
            get => _RootNodes;
            set
            {
                _RootNodes = value;

                if (value is INotifyCollectionChanged notifyCollectionChanged)
                {
                    notifyCollectionChanged.CollectionChanged += (s, e) =>
                    {
                        RenderNodes(_RootNodes, _StackLayout, e, null);
                    };
                }

                RenderNodes(_RootNodes, _StackLayout, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), null);
            }
        }

        /// <summary>
        /// Occurs when the user selects a TreeViewItem
        /// </summary>
        public event EventHandler SelectedItemChanged;

        public TreeView()
        {
            Content = _StackLayout;

            var dropGesture = new DropGestureRecognizer();
            dropGesture.Drop += OnTreeDropped;
            GestureRecognizers.Add(dropGesture);
        }

        private void OnTreeDropped(object sender, DropEventArgs e)
        {
            if (e.Data.Properties.ContainsKey("DraggedNode"))
            {
                var draggedNode = e.Data.Properties["DraggedNode"] as TreeViewNode;

                if (draggedNode != null)
                {
                    draggedNode.ParentTreeViewItem?.ChildrenList.Remove(draggedNode);
                    RootNodes.Add(draggedNode);
                    draggedNode.ParentTreeViewItem = null;
                }
            }
        }

        private void RemoveSelectionRecursive(IEnumerable<TreeViewNode> nodes)
        {
            foreach (var treeViewItem in nodes)
            {
                if (treeViewItem != SelectedItem)
                {
                    treeViewItem.IsSelected = false;
                }

                RemoveSelectionRecursive(treeViewItem.ChildrenList);
            }
        }

        private static void AddItems(IEnumerable<TreeViewNode> childTreeViewItems, StackLayout parent, TreeViewNode parentTreeViewItem)
        {            
            foreach (var childTreeNode in childTreeViewItems)
            {
                if (!parent.Children.Contains(childTreeNode))
                {
                    parent.Children.Add(childTreeNode);
                }

                childTreeNode.ParentTreeViewItem = parentTreeViewItem;
            }
        }

        /// <summary>
        /// TODO: A bit stinky but better than bubbling an event up...
        /// </summary>
        internal void ChildSelected(TreeViewNode child)
        {
            SelectedItem = child;
            child.IsSelected = true;
            child.SelectionBoxView.Color = child.SelectedBackgroundColor;
            child.SelectionBoxView.Opacity = child.SelectedBackgroundOpacity;
            RemoveSelectionRecursive(RootNodes);
        }

        internal static void RenderNodes(IEnumerable<TreeViewNode> childTreeViewItems, StackLayout parent, NotifyCollectionChangedEventArgs e, TreeViewNode parentTreeViewItem)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                //TODO: Reintate this...
                //parent.Children.Clear();
                AddItems(childTreeViewItems, parent, parentTreeViewItem);
            }
            else
            {
                AddItems(e.NewItems.Cast<TreeViewNode>(), parent, parentTreeViewItem);
            }
        }

        // Main code: 
        private TreeViewNode CreateTreeViewNode(object bindingContext, Label label, bool isItem)
        {
            var node = new TreeViewNode
            {
                BindingContext = bindingContext,
                Content = new StackLayout
                {
                    Children =
                    {
                        new ResourceImage
                        {
                            Resource = isItem? "item.png" :"folderopen.png" ,
                            HeightRequest= 16,
                            WidthRequest = 16
                        },
                        label
                    },
                    Orientation = StackOrientation.Horizontal
                }
            };

            //set DataTemplate for expand button content
            node.ExpandButtonTemplate = new DataTemplate(() => new ExpandButtonContent { BindingContext = node });

            return node;
        }

        private void CreateXamlItem(IList<TreeViewNode> children, TreeItem xamlItem)
        {
            var label = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.Black
            };
            label.SetBinding(Label.TextProperty, "ItemName");

            var xamlItemTreeViewNode = CreateTreeViewNode(xamlItem, label, true);
            children.Add(xamlItemTreeViewNode);
        }

        public ObservableCollection<TreeViewNode> ProcessXamlItemGroups(TreeFolder xamlItemGroups)
        {
            var rootNodes = new ObservableCollection<TreeViewNode>();

            foreach (var xamlItemGroup in xamlItemGroups.Children.OrderBy(xig => xig.FolderName))
            {
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Black
                };
                label.SetBinding(Label.TextProperty, "FolderName");

                var groupTreeViewNode = CreateTreeViewNode(xamlItemGroup, label, false);

                rootNodes.Add(groupTreeViewNode);

                groupTreeViewNode.ChildrenList = ProcessXamlItemGroups(xamlItemGroup);

                foreach (var xamlItem in xamlItemGroup.TreeItems)
                {
                    CreateXamlItem(groupTreeViewNode.ChildrenList, xamlItem);
                }
            }

            return rootNodes;
        }
    }
}
