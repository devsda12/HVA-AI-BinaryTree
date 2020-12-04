using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;

namespace BinaryQuestions
{
    #region Namespace enums
    internal enum Order { PreOrder, InOrder, PostOrder }
    #endregion

    [Serializable] class BTTree
    {
        BTNode rootNode;

        public BTTree(string question, string yesGuess, string noGuess)
        {
            rootNode = new BTNode(question);
            rootNode.setYesNode(new BTNode(yesGuess));
            rootNode.setNoNode(new BTNode(noGuess));

            //Serialize the object on creation
            this.saveQuestionTree();
        }

        public BTTree()
        {
            IFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.OpenRead("serialized.bin"))
            {
                rootNode = (BTNode)formatter.Deserialize(stream);
            }
        }

        public void query()
        {
            PrintOrder(rootNode, Order.PreOrder);
            Console.WriteLine(string.Format("Minimax output: {0}", Minimax(rootNode, true)));
            Console.WriteLine(string.Format("ABPruning output: {0}", ABPruning(rootNode, true)));
            rootNode.query(1);

            //We're at the end of the game now, so we'll save the tree in case the user added new data
            this.saveQuestionTree();
        }

        public void saveQuestionTree()
        {
            IFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Create("serialized.bin"))
            {
                formatter.Serialize(stream, rootNode);
            }
        }    

        #region Pre-order, in-order and post-order methods
        public void PrintOrder(BTNode concerningNode, Order order, string currentStreak = "")
        {
            switch (order)
            {
                case Order.PreOrder:
                    Console.WriteLine(string.Format("CurrentStreak: {0}, Message: {1}", currentStreak, concerningNode.getMessage())); //Printing the message
                    if (concerningNode.getYesNode() != null) PrintOrder(concerningNode.getYesNode(), order, currentStreak + "Y");
                    if (concerningNode.getNoNode() != null) PrintOrder(concerningNode.getNoNode(), order, currentStreak + "N");
                    break;

                case Order.InOrder:
                    if (concerningNode.getYesNode() != null) PrintOrder(concerningNode.getYesNode(), order, currentStreak + "Y");
                    Console.WriteLine(string.Format("CurrentStreak: {0}, Message: {1}", currentStreak, concerningNode.getMessage())); //Printing the message
                    if (concerningNode.getNoNode() != null) PrintOrder(concerningNode.getNoNode(), order, currentStreak + "N");
                    break;

                case Order.PostOrder:
                    if (concerningNode.getYesNode() != null) PrintOrder(concerningNode.getYesNode(), order, currentStreak + "Y");
                    if (concerningNode.getNoNode() != null) PrintOrder(concerningNode.getNoNode(), order, currentStreak + "N");
                    Console.WriteLine(string.Format("CurrentStreak: {0}, Message: {1}", currentStreak, concerningNode.getMessage())); //Printing the message
                    break;
            }
        }
        #endregion

        #region Minimax
        public int Minimax(BTNode concerningNode, bool isMax)
        {
            if (!concerningNode.isQuestion())
                return concerningNode.Evaluation();
            
            if(isMax)
                return Math.Max(Minimax(concerningNode.getYesNode(), !isMax),
                                Minimax(concerningNode.getNoNode(), !isMax));
            else
                return Math.Min(Minimax(concerningNode.getYesNode(), !isMax),
                                Minimax(concerningNode.getNoNode(), !isMax));
        }
        #endregion

        #region Alpha-Beta pruning
        public int ABPruning(BTNode concerningNode, bool isMax, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            Console.WriteLine(string.Format("Now checking node {0}", concerningNode.getMessage()));

            if (!concerningNode.isQuestion())
                return concerningNode.Evaluation();

            if (isMax)
            {
                int best = int.MinValue;

                for(int i = 0; i < 2; i++)
                {
                    int value = ABPruning(i == 0 ? concerningNode.getYesNode() : concerningNode.getNoNode(), false, alpha, beta);

                    best = Math.Max(best, value);
                    alpha = Math.Max(alpha, best);

                    if (beta <= alpha)
                        break;
                }
                return best;
            }
            else
            {
                int best = int.MaxValue;

                for (int i = 0; i < 2; i++)
                {
                    int value = ABPruning(i == 0 ? concerningNode.getYesNode() : concerningNode.getNoNode(), true, alpha, beta);

                    best = Math.Min(best, value);
                    beta = Math.Min(beta, best);

                    if (beta <= alpha)
                        break;
                }
                return best;
            }
        }
        #endregion
    }
}
