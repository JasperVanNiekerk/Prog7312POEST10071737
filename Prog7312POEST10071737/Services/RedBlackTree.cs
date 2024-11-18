// Services/RedBlackTree.cs
using System;
using System.Collections.Generic;

namespace Prog7312POEST10071737.Services
{
    public class RedBlackTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private enum Color
        {
            Red,
            Black
        }

        private class Node
        {
            public TKey Key;
            public List<TValue> Values;
            public Color Color;
            public Node Left;
            public Node Right;
            public Node Parent;

            public Node(TKey key, TValue value)
            {
                Key = key;
                Values = new List<TValue> { value };
                Color = Color.Red;
            }
        }

        private Node root;

        public void Insert(TKey key, TValue value)
        {
            Node newNode = new Node(key, value);
            root = BSTInsert(root, newNode);
            FixViolation(newNode);
        }

        private Node BSTInsert(Node root, Node node)
        {
            if (root == null)
                return node;

            if (node.Key.CompareTo(root.Key) < 0)
            {
                root.Left = BSTInsert(root.Left, node);
                root.Left.Parent = root;
            }
            else if (node.Key.CompareTo(root.Key) > 0)
            {
                root.Right = BSTInsert(root.Right, node);
                root.Right.Parent = root;
            }
            else
            {
                // Duplicate key, add value to list
                root.Values.AddRange(node.Values);
            }

            return root;
        }

        private void FixViolation(Node node)
        {
            Node parent = null;
            Node grandParent = null;

            while (node != root && node.Color == Color.Red && node.Parent.Color == Color.Red)
            {
                parent = node.Parent;
                grandParent = parent.Parent;

                if (parent == grandParent.Left)
                {
                    Node uncle = grandParent.Right;

                    // Case 1: Uncle is also red
                    if (uncle != null && uncle.Color == Color.Red)
                    {
                        grandParent.Color = Color.Red;
                        parent.Color = Color.Black;
                        uncle.Color = Color.Black;
                        node = grandParent;
                    }
                    else
                    {
                        // Case 2: Node is right child
                        if (node == parent.Right)
                        {
                            LeftRotate(parent);
                            node = parent;
                            parent = node.Parent;
                        }

                        // Case 3: Node is left child
                        RightRotate(grandParent);
                        SwapColors(parent, grandParent);
                        node = parent;
                    }
                }
                else
                {
                    Node uncle = grandParent.Left;

                    // Mirror Case 1
                    if (uncle != null && uncle.Color == Color.Red)
                    {
                        grandParent.Color = Color.Red;
                        parent.Color = Color.Black;
                        uncle.Color = Color.Black;
                        node = grandParent;
                    }
                    else
                    {
                        // Mirror Case 2
                        if (node == parent.Left)
                        {
                            RightRotate(parent);
                            node = parent;
                            parent = node.Parent;
                        }

                        // Mirror Case 3
                        LeftRotate(grandParent);
                        SwapColors(parent, grandParent);
                        node = parent;
                    }
                }
            }

            root.Color = Color.Black;
        }

        private void SwapColors(Node node1, Node node2)
        {
            Color temp = node1.Color;
            node1.Color = node2.Color;
            node2.Color = temp;
        }

        private void LeftRotate(Node node)
        {
            Node y = node.Right;
            node.Right = y.Left;

            if (y.Left != null)
                y.Left.Parent = node;

            y.Parent = node.Parent;

            if (node.Parent == null)
                root = y;
            else if (node == node.Parent.Left)
                node.Parent.Left = y;
            else
                node.Parent.Right = y;

            y.Left = node;
            node.Parent = y;
        }

        private void RightRotate(Node node)
        {
            Node y = node.Left;
            node.Left = y.Right;

            if (y.Right != null)
                y.Right.Parent = node;

            y.Parent = node.Parent;

            if (node.Parent == null)
                root = y;
            else if (node == node.Parent.Right)
                node.Parent.Right = y;
            else
                node.Parent.Left = y;

            y.Right = node;
            node.Parent = y;
        }

        public List<TValue> Search(TKey key)
        {
            Node node = Search(root, key);
            return node != null ? node.Values : null;
        }

        private Node Search(Node node, TKey key)
        {
            if (node == null)
                return null;

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
                return Search(node.Left, key);
            else if (cmp > 0)
                return Search(node.Right, key);
            else
                return node;
        }
    }
}