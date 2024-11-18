using Prog7312POEST10071737.Models;
using System;

namespace Prog7312POEST10071737.Services
{
    public class BinarySearchTree
    {
        private class Node
        {
            public IssueReport Report { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            //___________________________________________________________________________________________________________

            public Node(IssueReport report)
            {
                Report = report;
                Left = Right = null;
            }
        }
        //___________________________________________________________________________________________________________

        private Node root;
        //___________________________________________________________________________________________________________

        public void Insert(IssueReport report)
        {
            root = InsertRec(root, report);
        }
        //___________________________________________________________________________________________________________

        private Node InsertRec(Node root, IssueReport report)
        {
            if (root == null)
            {
                return new Node(report);
            }

            int comparison = string.Compare(report.name, root.Report.name, StringComparison.OrdinalIgnoreCase);

            if (comparison < 0)
            {
                root.Left = InsertRec(root.Left, report);
            }
            else if (comparison > 0)
            {
                root.Right = InsertRec(root.Right, report);
            }

            return root;
        }
        //___________________________________________________________________________________________________________

        public void InorderTraversal(Action<IssueReport> action)
        {
            InorderTraversalRec(root, action);
        }
        //___________________________________________________________________________________________________________

        private void InorderTraversalRec(Node root, Action<IssueReport> action)
        {
            if (root != null)
            {
                InorderTraversalRec(root.Left, action);
                action(root.Report);
                InorderTraversalRec(root.Right, action);
            }
        }
    }
}
//____________________________________EOF_________________________________________________________________________