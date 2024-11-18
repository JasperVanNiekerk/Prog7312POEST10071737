// Services/AVLTree.cs
using System;
using System.Collections.Generic;

namespace Prog7312POEST10071737.Services
{
    public class AVLTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class Node
        {
            public TKey Key;
            public List<TValue> Values;
            public Node Left;
            public Node Right;
            public int Height;
            //___________________________________________________________________________________________________________

            public Node(TKey key, TValue value)
            {
                Key = key;
                Values = new List<TValue> { value };
                Height = 1;
            }
        }
        //___________________________________________________________________________________________________________

        private Node root;
        //___________________________________________________________________________________________________________

        public void Insert(TKey key, TValue value)
        {
            root = Insert(root, key, value);
        }
        //___________________________________________________________________________________________________________

        private Node Insert(Node node, TKey key, TValue value)
        {
            if (node == null)
                return new Node(key, value);

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
                node.Left = Insert(node.Left, key, value);
            else if (cmp > 0)
                node.Right = Insert(node.Right, key, value);
            else
                node.Values.Add(value); // Duplicate key, add value to list

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Left Left Case
            if (balance > 1 && key.CompareTo(node.Left.Key) < 0)
                return RightRotate(node);

            // Right Right Case
            if (balance < -1 && key.CompareTo(node.Right.Key) > 0)
                return LeftRotate(node);

            // Left Right Case
            if (balance > 1 && key.CompareTo(node.Left.Key) > 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // Right Left Case
            if (balance < -1 && key.CompareTo(node.Right.Key) < 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }
        //___________________________________________________________________________________________________________

        public List<TValue> Search(TKey key)
        {
            Node node = Search(root, key);
            return node != null ? node.Values : null;
        }
        //___________________________________________________________________________________________________________

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
        //___________________________________________________________________________________________________________

        private int GetHeight(Node node)
        {
            return node?.Height ?? 0;
        }
        //___________________________________________________________________________________________________________

        private int GetBalance(Node node)
        {
            if (node == null)
                return 0;
            return GetHeight(node.Left) - GetHeight(node.Right);
        }
        //___________________________________________________________________________________________________________

        private Node RightRotate(Node y)
        {
            Node x = y.Left;
            Node T2 = x.Right;

            // Perform rotation
            x.Right = y;
            y.Left = T2;

            // Update heights
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            // Return new root
            return x;
        }
        //___________________________________________________________________________________________________________

        private Node LeftRotate(Node x)
        {
            Node y = x.Right;
            Node T2 = y.Left;

            // Perform rotation
            y.Left = x;
            x.Right = T2;

            // Update heights
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            // Return new root
            return y;
        }
    }
}
//____________________________________EOF_________________________________________________________________________