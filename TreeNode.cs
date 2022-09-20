using System.Collections.Generic;

namespace Mankala
{
    public class TreeNode
    {
        public int value = -50;
        public int[] _board { get; set; }
        public int type = 2; //1 - player1 which will minimaze   2 - AI which will maximaxe, default 2 for root
        public int pos = -1;

        public List<TreeNode> _children { get; set; }

        public TreeNode(int[] board)
        {
            this._children = new List<TreeNode>();
            _board = board;
        }

        public TreeNode(int[] board, int position, int player)
        {
            this._children = new List<TreeNode>();
            _board = board;
            value = board[13] - board[6];
            pos = position;
            type = player;
        }

        public void AddChild(int[] board, int pos, int type)
        {
            var treeNode = new TreeNode(board, pos, type);
            _children.Add(treeNode);
        }
    }
}
