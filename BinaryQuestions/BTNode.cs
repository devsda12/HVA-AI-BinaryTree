using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryQuestions
{
    [Serializable] class BTNode
    {
        string message;
        BTNode noNode;
        BTNode yesNode;

        /**
         * Constructor for the nodes: This class holds an String representing 
         * an object if the noNode and yesNode are null and a question if the
         * yesNode and noNode point to a BTNode.
         */
        public BTNode(string nodeMessage)
        {
            message = nodeMessage;
            noNode = null;
            yesNode = null;
        }

        public void query(int q)
        {
            if (q > 20)
            {
                Console.WriteLine("That was the last question. You win!");
            }
            else if (this.isQuestion())
            {
                Console.WriteLine(q + ") " + this.message);
                Console.Write("Enter 'y' for yes and 'n' for no: ");
                char input = getYesOrNo(); //y or n
                if (input == 'y')
                    yesNode.query(q+1);
                else
                    noNode.query(q+1);
            }
            else
                this.onQueryObject(q);
        }

        public void onQueryObject(int q)
        {
            Console.WriteLine(q + ") Are you thinking of a(n) " + this.message + "? ");
            Console.Write("Enter 'y' for yes and 'n' for no: ");
            char input = getYesOrNo(); //y or n
            if (input == 'y')
                Console.Write("I Win!\n");
            else
                updateTree();
        }

        private void updateTree()
        {
            Console.Write("You win! What were you thinking of? ");
            string userObject = Console.ReadLine();
            Console.Write("Please enter a question to distinguish a(n) "
                + this.message + " from " + userObject + ": ");
            string userQuestion = Console.ReadLine();
            Console.Write("If you were thinking of a(n) " + userObject
                + ", what would the answer to that question be (\'yes\' or \'no\')? ");
            char input = getYesOrNo(); //y or n
            if (input == 'y')
            {
                this.noNode = new BTNode(this.message);
                this.yesNode = new BTNode(userObject);
            }
            else
            {
                this.yesNode = new BTNode(this.message);
                this.noNode = new BTNode(userObject);
            }
            Console.Write("Thank you! My knowledge has been increased");
            this.setMessage(userQuestion);
        }

        public bool isQuestion()
        {
            if (noNode == null && yesNode == null)
                return false;
            else
                return true;
        }

        /**
         * Asks a user for yes or no and keeps prompting them until the key
         * Y,y,N,or n is entered
         */
        private char getYesOrNo()
        {
            char inputCharacter = ' ';
            while (inputCharacter != 'y' && inputCharacter != 'n')
            {
                inputCharacter = Console.ReadLine().ElementAt(0);
                inputCharacter = Char.ToLower(inputCharacter);
            }
            return inputCharacter;
        }

        //Mutator Methods
        public void setMessage(string nodeMessage)
        {
            message = nodeMessage;
        }

        public string getMessage()
        {
            return message;
        }

        public void setNoNode(BTNode node)
        {
            noNode = node;
        }

        public BTNode getNoNode()
        {
            return noNode;
        }

        public void setYesNode(BTNode node)
        {
            yesNode = node;
        }

        public BTNode getYesNode()
        {
            return yesNode;
        }
    }
}
