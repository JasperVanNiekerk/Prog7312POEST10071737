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
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            Node newNode = new Node(key, value);

            // If root is null, make new node the root and color it black
            if (root == null)
            {
                root = newNode;
                root.Color = Color.Black;
                return;
            }

            // Regular BST insertion
            Node current = root;
            Node parent = null;

            while (current != null)
            {
                parent = current;
                int comparison = key.CompareTo(current.Key);

                if (comparison < 0)
                    current = current.Left;
                else if (comparison > 0)
                    current = current.Right;
                else
                {
                    // Key already exists, add value to the list
                    current.Values.Add(value);
                    return;
                }
            }

            // Set parent of new node
            newNode.Parent = parent;

            // Set new node as left or right child
            int compareResult = key.CompareTo(parent.Key);
            if (compareResult < 0)
                parent.Left = newNode;
            else
                parent.Right = newNode;

            // Fix Red-Black tree violations
            FixViolation(newNode);
        }

        private void FixViolation(Node node)
        {
            if (node == null) return;

            Node parent = null;
            Node grandParent = null;

            while (node != root && node.Color == Color.Red && node.Parent != null && node.Parent.Color == Color.Red)
            {
                parent = node.Parent;
                grandParent = parent.Parent;

                if (grandParent == null)
                    break;

                if (parent == grandParent.Left)
                {
                    Node uncle = grandParent.Right;

                    // Case 1: Uncle is red
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
                        Color tempColor = parent.Color;
                        parent.Color = grandParent.Color;
                        grandParent.Color = tempColor;
                        node = parent;
                    }
                }
                else
                {
                    Node uncle = grandParent.Left;

                    // Case 1: Uncle is red
                    if (uncle != null && uncle.Color == Color.Red)
                    {
                        grandParent.Color = Color.Red;
                        parent.Color = Color.Black;
                        uncle.Color = Color.Black;
                        node = grandParent;
                    }
                    else
                    {
                        // Case 2: Node is left child
                        if (node == parent.Left)
                        {
                            RightRotate(parent);
                            node = parent;
                            parent = node.Parent;
                        }

                        // Case 3: Node is right child
                        LeftRotate(grandParent);
                        Color tempColor = parent.Color;
                        parent.Color = grandParent.Color;
                        grandParent.Color = tempColor;
                        node = parent;
                    }
                }
            }

            root.Color = Color.Black;
        }

        private void LeftRotate(Node x)
        {
            if (x == null || x.Right == null)
                return;

            Node y = x.Right;
            x.Right = y.Left;

            if (y.Left != null)
                y.Left.Parent = x;

            y.Parent = x.Parent;

            if (x.Parent == null)
                root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Left = x;
            x.Parent = y;
        }

        private void RightRotate(Node y)
        {
            if (y == null || y.Left == null)
                return;

            Node x = y.Left;
            y.Left = x.Right;

            if (x.Right != null)
                x.Right.Parent = y;

            x.Parent = y.Parent;

            if (y.Parent == null)
                root = x;
            else if (y == y.Parent.Right)
                y.Parent.Right = x;
            else
                y.Parent.Left = x;

            x.Right = y;
            y.Parent = x;
        }

        public List<TValue> Search(TKey key)
        {
            if (key == null)
                return new List<TValue>();

            Node node = Search(root, key);
            return node?.Values ?? new List<TValue>();
        }

        private Node Search(Node node, TKey key)
        {
            if (node == null || key.Equals(node.Key))
                return node;

            if (key.CompareTo(node.Key) < 0)
                return Search(node.Left, key);
            else
                return Search(node.Right, key);
        }

        // Helper method to check if tree is empty
        public bool IsEmpty()
        {
            return root == null;
        }
    }
}